using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FixedIncome.Application.Interfaces;
using FixedIncome.Application.Services;
using FixedIncome.Domain.Interfaces;
using FixedIncome.Infrastructure.Data;
using FixedIncome.Infrastructure.Repositories;
using MassTransit;
using FixedIncome.Infrastructure.Messagings;
using FixedIncome.Domain.Validators;
using FixedIncome.Domain.Publishers;

namespace FixedIncome.Ioc.ExtensionDependence
{
    public static class DependenceInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<FixedIncomeDBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                  sqlOptions => sqlOptions.EnableRetryOnFailure(
                   maxRetryCount: 5,
                   maxRetryDelay: TimeSpan.FromSeconds(10),
                   errorNumbersToAdd: null
                  )
                )
            );

            ConfigureMassTransit(services, configuration);

            return services;
        }


        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Registrar os serviços
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IPurchaseValidator, PurchaseValidator>();
            services.AddScoped<IPurchaseEventPublisher, PurchaseEventPublisher>();

            return services;
        }

        private static void ConfigureMassTransit(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<PurchaseRealizedConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["RabbitMQ:HostName"], h =>
                    {
                        h.Username(configuration["RabbitMQ:UserName"]);
                        h.Password(configuration["RabbitMQ:Password"]);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            services.AddScoped<IBus>(provider => provider.GetRequiredService<IBusControl>());
        }

    }
}
