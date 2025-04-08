using FixedIncome.Application.DTO_s;
using FixedIncome.Application.Interfaces;
using FixedIncome.Domain.Interfaces;

namespace FixedIncome.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IProductRepository _productRepo;
        private readonly IAccountRepository _accountRepo;

        public OrderService(IProductRepository productRepo, IAccountRepository accountRepo)
        {
            _productRepo = productRepo;
            _accountRepo = accountRepo;
        }

        public async Task<bool> PurchaseAsync(PurchaseRequest request)
        {
            var account = await _accountRepo.GetByIdAsync(request.AccountId);
            if (account == null)
                throw new InvalidOperationException("Conta não encontrada.");

            var product = await _productRepo.GetByIdAsync(request.ProductId);
            if (product == null)
                throw new InvalidOperationException("Produto não encontrado.");

            var totalPrice = product.UnitPrice * request.Quantity;

            if (account.Balance < totalPrice)
                throw new InvalidOperationException("Saldo insuficiente.");

            if (product.Stock < request.Quantity)
                throw new InvalidOperationException("Estoque insuficiente.");

            // Atualiza os dados
            account.Balance -= totalPrice;
            product.Stock -= request.Quantity;

            // Salva
            await _accountRepo.UpdateAsync(account);
            await _productRepo.UpdateAsync(product);

            return true;
        }
    }

}
