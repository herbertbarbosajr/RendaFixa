using FixedIncome.Application.DTO_s;
using FixedIncome.Application.Interfaces;
using FixedIncome.Domain.Interfaces;
using FixedIncome.Infrastructure.Interfaces;
using System.Text.Json;

namespace FixedIncome.Application.Services
{
    public class OrderService(IProductRepository productRepo, IAccountRepository accountRepo, IMessagePublisher messagePublisher) : IOrderService
    {
        private readonly IProductRepository _productRepo = productRepo;
        private readonly IAccountRepository _accountRepo = accountRepo;
        private readonly IMessagePublisher _messagePublisher = messagePublisher;

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

            account.Balance -= totalPrice;
            product.Stock -= request.Quantity;

            await _accountRepo.UpdateAsync(account);
            await _productRepo.UpdateAsync(product);

            var orderMessage = new
            {
                request.AccountId,
                request.ProductId,
                request.Quantity,
                TotalPrice = totalPrice,
                Timestamp = DateTime.UtcNow
            };

            var message = JsonSerializer.Serialize(orderMessage);
            _messagePublisher.Publish("order-queue", message);


            return true;
        }
    }

}
