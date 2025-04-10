using System.Text;
using FixedIncome.Domain.Entities;
using FixedIncome.Infrastructure.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace FixedIncome.Infrastructure.Messagings;

public class RabbitMqPublisher : IMessagePublisher
{
    public async void Publish(string queueName, string message)
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri("amqp://guest:guest@localhost:5672/") // Use a propriedade Uri para configurar a conexão
        };

        // Cria conexão e canal de forma síncrona
       using var connectionBuilder = await factory.CreateConnectionAsync();

        using var channel = await connectionBuilder.CreateChannelAsync();

        await channel.QueueDeclareAsync(
          queue: queueName,
          durable: false,
          exclusive: false,
          autoDelete: false,
          arguments: null
      );

        var body = Encoding.UTF8.GetBytes(message);

        // Publica a mensagem
        // await channel.BasicPublishAsync<CustomerAccount>(
        //    exchange: "",
        //    routingKey: queueName,
        //    mandatory: false,
        //    basicProperties: null,
        //    body: body,
        //    cancellationToken: default

        //);



        Console.WriteLine($"📤 Mensagem publicada na fila '{queueName}': {message}");
    }

}

