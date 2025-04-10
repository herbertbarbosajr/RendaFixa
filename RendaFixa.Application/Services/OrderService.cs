using FixedIncome.Application.DTO_s;
using FixedIncome.Application.Interfaces;
using FixedIncome.Domain.Interfaces;
using FixedIncome.Infrastructure.Events;
using MassTransit;

public class OrderService(IProductRepository productRepo, IAccountRepository accountRepo, IBus bus) : IOrderService
{
    private readonly IProductRepository _productRepo = productRepo;
    private readonly IAccountRepository _accountRepo = accountRepo;
    private readonly IBus _bus = bus;

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

        // Publica o evento no RabbitMQ
        var evento = new PurchaseRealizedEvent
        {
            ProductId = request.ProductId,
            Amount = request.Quantity,
            Total = totalPrice,
            Date = DateTime.UtcNow
        };

        await _bus.Publish(evento);

        return true;
    }
}
