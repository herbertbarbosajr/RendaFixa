using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RendaFixa.Domain.Entities;

namespace RendaFixa.Infrastructure.Data.Configurations
{
    public class ContaInvestidorConfiguration : IEntityTypeConfiguration<ContaInvestidor>
    {
        public void Configure(EntityTypeBuilder<ContaInvestidor> builder)
        {
            builder.ToTable("Contas");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Saldo).IsRequired().HasColumnType("decimal(18,2)");
        }
    }
}
