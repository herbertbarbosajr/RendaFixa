using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                
                var orderMessage = JsonSerializer.Deserialize<OrderMessage>(message);

                ProcessMessage(orderMessage);
                return Task.CompletedTask;
            };

            channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            Console.WriteLine("Consuming messages. Press [enter] to exit.");
            Console.ReadLine();
        }

        private void ProcessMessage(OrderMessage? orderMessage)
        {
            if (orderMessage != null)
            {
                Console.WriteLine($"Received Order: AccountId={orderMessage.AccountId}, ProductId={orderMessage.ProductId}, Quantity={orderMessage.Quantity}");
                
            }
        }
    }
}
