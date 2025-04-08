using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FixedIncome.Domain.Entities;

namespace FixedIncome.Infrastructure.Data.Configurations
{
    public class InvestorAccountConfiguration : IEntityTypeConfiguration<CustomerAccount>
    {
        public void Configure(EntityTypeBuilder<CustomerAccount> builder)
        {
            builder.ToTable("Accounts");
            builder.HasKey(c => c.Account);
            builder.Property(c => c.Balance).IsRequired().HasColumnType("decimal(18,2)");
        }
    }
}
