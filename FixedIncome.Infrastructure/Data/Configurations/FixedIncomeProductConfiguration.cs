using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FixedIncome.Domain.Entities;

namespace FixedIncome.Infrastructure.Data.Configurations
{
    public class FixedIncomeProductConfiguration : IEntityTypeConfiguration<FixedIncomeProduct>
    {
        public void Configure(EntityTypeBuilder<FixedIncomeProduct> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.BondAsset).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Index).IsRequired().HasMaxLength(50);
            builder.Property(p => p.UnitPrice).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(p => p.Tax).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(p => p.Stock).IsRequired();
        }
    }
}
