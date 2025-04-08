using Microsoft.EntityFrameworkCore;
using FixedIncome.Domain.Entities;
using FixedIncome.Infrastructure.Data.Configurations;

namespace FixedIncome.Infrastructure.Data
{
    public class FixedIncomeDBContext(DbContextOptions<FixedIncomeDBContext> options) : DbContext(options)
    {
        public DbSet<FixedIncomeProduct> Products { get; set; }
        public DbSet<CustomerAccount> Accounts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FixedIncomeProductConfiguration());
            modelBuilder.ApplyConfiguration(new InvestorAccountConfiguration());
        }
    }
}
