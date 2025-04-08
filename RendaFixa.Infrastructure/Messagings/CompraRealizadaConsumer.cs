using MassTransit;
using RendaFixa.Infrastructure.Events;

namespace RendaFixa.Infrastructure.Messagings
{
    public class CompraRealizadaConsumer : IConsumer<CompraRealizadaEvent>
    {
        public Task Consume(ConsumeContext<CompraRealizadaEvent> context)
        {
            var msg = context.Message;
            Console.WriteLine($"Compra registrada: Produto {msg.ProdutoId}, Quantidade {msg.Quantidade}, Total: {msg.Total}");
            return Task.CompletedTask;
        }
    }
}
