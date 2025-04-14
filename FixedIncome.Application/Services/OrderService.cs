using FixedIncome.Application.DTO_s;
using FixedIncome.Application.Interfaces;
using FixedIncome.Domain.Interfaces;
using FixedIncome.Infrastructure.Events;
using MassTransit;

namespace FixedIncome.Application.Services;
public class OrderService(IProductRepository productRepo, IAccountRepository accountRepo,
                           IBus bus, IPurchaseValidator purchaseValidator,
                           IPurchaseEventPublisher purchaseEventPublisher) : IOrderService
{
    private readonly IProductRepository _productRepo = productRepo;
    private readonly IAccountRepository _accountRepo = accountRepo;
    private readonly IBus _bus = bus;
    private readonly IPurchaseValidator _purchaseValidator = purchaseValidator;
    private readonly IPurchaseEventPublisher _purchaseEventPublisher = purchaseEventPublisher;

    public async Task<bool> PurchaseAsync(PurchaseRequest request)
    {
        var account = await _accountRepo.GetByIdAsync(request.AccountId);

        var product = await _productRepo.GetByIdAsync(request.ProductId);

        _purchaseValidator.OrderValidator(account, product, request.Quantity);

        var totalPrice = _purchaseValidator.ExecutePurchase(account, product, request.Quantity);

        account.Balance -= totalPrice;
        product.Stock -= request.Quantity;

        await _accountRepo.UpdateAsync(account);
        await _productRepo.UpdateAsync(product);

        // Publica o evento no RabbitMQ
        await _purchaseEventPublisher.PublishAsync(
            new PurchaseRealizedEvent
            {
                ProductId = request.ProductId,
                Amount = request.Quantity,
                Total = totalPrice,
                Date = DateTime.UtcNow
            });

        return true;
    }
}
