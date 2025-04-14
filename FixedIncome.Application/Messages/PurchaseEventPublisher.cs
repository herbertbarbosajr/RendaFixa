using FixedIncome.Domain.Interfaces;
using FixedIncome.Infrastructure.Events;
using MassTransit;

namespace FixedIncome.Domain.Publishers
{
    public class PurchaseEventPublisher(IBus bus) : IPurchaseEventPublisher
    {
        public async Task PublishAsync(PurchaseRealizedEvent @event)
        {
            await bus.Publish(@event);
        }
    }
}
