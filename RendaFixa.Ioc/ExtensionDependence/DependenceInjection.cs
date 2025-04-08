using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RendaFixa.Application.Interfaces;
using RendaFixa.Application.Services;
using RendaFixa.Domain.Interfaces;
using RendaFixa.Infrastructure.Data;
using RendaFixa.Infrastructure.Messagings;
using RendaFixa.Infrastructure.Repositories;

namespace RendaFixa.Ioc.ExtensionDependence
{
    public static class DependenceInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Registrar o DbContext com a string de conexão do SqlServer
            services.AddDbContext<RendaFixaDBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddMassTransit(x =>
            {
                x.AddConsumer<CompraRealizadaConsumer>();
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
            services.AddScoped<IProdutoService, ProdutoService>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IContaRepository, ContaRepository>();

            return services;
        }
    }
}
