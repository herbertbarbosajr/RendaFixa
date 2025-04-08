using MassTransit;
using FixedIncome.Application.DTO_s;
using FixedIncome.Application.Interfaces;
using FixedIncome.Domain.Interfaces;
using FixedIncome.Infrastructure.Events;
using FixedIncome.Domain.Entities;

namespace FixedIncome.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly IAccountRepository _accountRepo;
        private readonly IPublishEndpoint _publishEndpoint;

        public ProductService(IProductRepository productRepo, IAccountRepository accountRepo, IPublishEndpoint publishEndpoint)
        {
            _productRepo = productRepo;
            _accountRepo = accountRepo;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<List<FixedIncomeProduct>> GetOrderedProducts()
        {
            var products = await _productRepo.GetAllOrderedByTaxDescAsync();
            return products.OrderByDescending(p => p.Tax).Select(p => new FixedIncomeProduct
            {
                Id = p.Id,
                BondAsset = p.BondAsset,
                Index = p.Index,
                Tax = p.Tax,
                IssuerName = p.IssuerName,
                UnitPrice = p.UnitPrice,
                Stock = p.Stock
            }).ToList();
        }

        public async Task MakePurchase(int productId, int amount)
        {
            var product = await _productRepo.GetByIdAsync(productId)
                          ?? throw new Exception("Product not found");

            var account = await _accountRepo.GetByIdAsync(productId)
                          ?? throw new Exception("Account not found");

            product.DebitStock(amount);
            account.Balance -= product.UnitPrice * amount;

            await _productRepo.UpdateAsync(product);
            await _accountRepo.UpdateAsync(account);

            await _publishEndpoint.Publish(new PurchaseRealizedEvent(productId, amount, product.UnitPrice * amount, DateTime.UtcNow));
        }

        public async Task<decimal> GetBalance(int id)
        {
            var account = await _accountRepo.GetByIdAsync(id)
                          ?? throw new Exception("Account not found");

            return account.Balance;
        }
    }
}
