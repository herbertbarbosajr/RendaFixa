using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace FixedIncome.Application.Messages
{
    public class OrderMessageConsumer
    {
        private readonly string _queueName = "order-queue";

        public async Task StartConsuming()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

           await channel.QueueDeclareAsync(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                
                var orderMessage = JsonSerializer.Deserialize<OrderMessage>(message);

                ProcessMessage(orderMessage);
                return Task.CompletedTask;
            };

            await channel.BasicConsumeAsync(queue: _queueName, autoAck: true, consumer: consumer);

            Console.WriteLine("Consuming messages. Press [enter] to exit.");
            Console.ReadLine();
        }

        private static void ProcessMessage(OrderMessage? orderMessage)
        {
            if (orderMessage != null)
            {
                Console.WriteLine($"Received Order: AccountId={orderMessage.AccountId}, ProductId={orderMessage.ProductId}, Quantity={orderMessage.Quantity}");
                
            }
        }
    }
}
