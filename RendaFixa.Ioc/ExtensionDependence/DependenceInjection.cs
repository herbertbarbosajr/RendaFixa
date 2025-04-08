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

namespace FixedIncome.Ioc.ExtensionDependence
{
    public static class DependenceInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Registrar o DbContext com a string de conexão do SqlServer
            services.AddDbContext<FixedIncomeDBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddMassTransit(x =>
            {
                x.AddConsumer<PurchaseRealizedConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq://localhost", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    cfg.ConfigureEndpoints(context);
                });
            });
            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Registrar os serviços
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();

            return services;
        }
    }
}
