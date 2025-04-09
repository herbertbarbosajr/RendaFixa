using MassTransit;
using FixedIncome.Application.DTO_s;
using FixedIncome.Application.Interfaces;
using FixedIncome.Domain.Interfaces;
using FixedIncome.Infrastructure.Events;
using FixedIncome.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace FixedIncome.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly IAccountRepository _accountRepo;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository productRepo, IAccountRepository accountRepo, IPublishEndpoint publishEndpoint, ILogger<ProductService> logger)
        {
            _productRepo = productRepo;
            _accountRepo = accountRepo;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task<List<ProductDto>> GetOrderedProducts()
        {
            var products = await _productRepo.GetAllOrderedByTaxDescAsync();
            return [.. products
                         .OrderByDescending(p => p.Tax)
                         .Select(p => new ProductDto
                         {
                            BondAsset = p.BondAsset,
                            Index = p.Index,
                            Tax = p.Tax,
                            IssuerName = p.IssuerName,
                            UnitPrice = p.UnitPrice,
                            Stock = p.Stock
                         })];

        }

        public async Task MakePurchase(int productId, int amount)
        {
            if (productId <= 0)
                throw new ArgumentException("Product ID must be greater than zero", nameof(productId));

            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero", nameof(amount));

            var product = await GetEntityByIdAsync(_productRepo.GetByIdAsync, productId, "Product");
            var account = await GetEntityByIdAsync(_accountRepo.GetByIdAsync, productId, "Account");

            ValidatePurchase(product, account, amount);
            UpdateEntities(product, account, amount);

            await _productRepo.UpdateAsync(product);
            await _accountRepo.UpdateAsync(account);

            await _publishEndpoint.Publish(new PurchaseRealizedEvent(productId, amount, product.UnitPrice * amount, DateTime.UtcNow));

            _logger.LogInformation("Purchase completed for ProductId: {ProductId}, Amount: {Amount}", productId, amount);
        }


        public async Task<decimal> GetBalance(int id)
        {
            var account = await _accountRepo.GetByIdAsync(id)
                          ?? throw new Exception("Account not found");

            return account.Balance;
        }

        private static void ValidatePurchase(FixedIncomeProduct product, CustomerAccount account, int amount)
        {
            if (product.Stock < amount)
                throw new InvalidOperationException("Insufficient stock");

            if (account.Balance < product.UnitPrice * amount)
                throw new InvalidOperationException("Insufficient balance");
        }

        private static void UpdateEntities(FixedIncomeProduct product, CustomerAccount account, int amount)
        {
            product.DebitStock(amount);
            account.Balance -= product.UnitPrice * amount;
        }

        private static async Task<T> GetEntityByIdAsync<T>(Func<int, Task<T?>> getByIdFunc, int id, string entityName) where T : class
        {
            return await getByIdFunc(id) ?? throw new KeyNotFoundException($"{entityName} not found");
        }
    }
}
