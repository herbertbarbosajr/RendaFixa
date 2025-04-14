using FixedIncome.Infrastructure.Events;

namespace FixedIncome.Domain.Interfaces
{
    public interface IPurchaseEventPublisher
    {
        Task PublishAsync(PurchaseRealizedEvent @event);
    }
}
