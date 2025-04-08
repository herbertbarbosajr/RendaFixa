using Microsoft.EntityFrameworkCore;
using RendaFixa.Domain.Entities;
using RendaFixa.Infrastructure.Data.Configurations;

namespace RendaFixa.Infrastructure.Data
{
    public class RendaFixaDBContext(DbContextOptions<RendaFixaDBContext> options) : DbContext(options)
    {
        public DbSet<ProdutoRendaFixa> Produtos { get; set; }
        public DbSet<ContaInvestidor> Contas { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProdutoRendaFixaConfiguration());
            modelBuilder.ApplyConfiguration(new ContaInvestidorConfiguration());
        }
    }
}
