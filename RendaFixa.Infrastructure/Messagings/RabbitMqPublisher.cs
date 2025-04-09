using System.Text;
using FixedIncome.Infrastructure.Interfaces;
using RabbitMQ.Client;


namespace FixedIncome.Infrastructure.Messagings;

public class RabbitMqPublisher : IMessagePublisher
{
    public void Publish(string queueName, string message)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };

        // Cria conexão e canal de forma síncrona
        using var connection = factory.CreateConnection();

        using var channel = connection.CreateModel();

        // Declara a fila (caso não exista)
        channel.QueueDeclare(
            queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var body = Encoding.UTF8.GetBytes(message);

        // Publica a mensagem
        channel.BasicPublish(
            exchange: "",
            routingKey: queueName,
            basicProperties: null,
            body: body
        );

        Console.WriteLine($"📤 Mensagem publicada na fila '{queueName}': {message}");
    }
}

