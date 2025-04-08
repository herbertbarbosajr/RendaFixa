using MassTransit;
using FixedIncome.Infrastructure.Events;

namespace FixedIncome.Infrastructure.Messagings
{
    public class PurchaseRealizedConsumer : IConsumer<PurchaseRealizedEvent>
    {
        public Task Consume(ConsumeContext<PurchaseRealizedEvent> context)
        {
            var msg = context.Message;
            Console.WriteLine($"Purchase registred: Product {msg.ProductId}, Amount {msg.Amount}, Total: {msg.Total}");
            return Task.CompletedTask;
        }
    }
}
