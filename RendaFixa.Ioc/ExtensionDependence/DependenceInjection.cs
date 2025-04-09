using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FixedIncome.Application.Interfaces;
using FixedIncome.Application.Services;
using FixedIncome.Domain.Interfaces;
using FixedIncome.Infrastructure.Data;
using FixedIncome.Infrastructure.Messagings;
using FixedIncome.Infrastructure.Repositories;
using FixedIncome.Infrastructure.Interfaces;

namespace FixedIncome.Ioc.ExtensionDependence
{
    public static class DependenceInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Registrar o DbContext com a string de conexão do SqlServer
            services.AddDbContext<FixedIncomeDBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Configurar o MassTransit com RabbitMQ
            ConfigureMassTransit(services, configuration);

            return services;
        }

        private static void ConfigureMassTransit(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<PurchaseRealizedConsumer>();

                //x.UsingRabbitMq((context, cfg) =>
                //{
                //    var rabbitMqHost = configuration["RabbitMQ:Host"] ?? "rabbitmq";
                //    var rabbitMqUser = configuration["RabbitMQ:User"] ?? "guest";
                //    var rabbitMqPassword = configuration["RabbitMQ:Password"] ?? "guest";
                //    var queueName = configuration["RabbitMQ:QueueName"] ?? "purchase-realized-queue";

                //    cfg.Host(rabbitMqHost, h =>
                //    {
                //        h.Username(rabbitMqUser);
                //        h.Password(rabbitMqPassword);
                //    });

                //    cfg.ReceiveEndpoint(queueName, e =>
                //    {
                //        e.ConfigureConsumer<PurchaseRealizedConsumer>(context);
                //    });

                //    cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                //    cfg.UseInMemoryOutbox();
                //});
            });
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Registrar os serviços
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IMessagePublisher, RabbitMqPublisher>();

            return services;
        }
    }
}
