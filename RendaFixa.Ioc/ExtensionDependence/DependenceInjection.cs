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
            
            services.AddDbContext<FixedIncomeDBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            
            ConfigureMassTransit(services, configuration);

            return services;
        }

        private static void ConfigureMassTransit(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<PurchaseRealizedConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });
            
            services.AddScoped<IBus>(provider => provider.GetRequiredService<IBusControl>());
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
