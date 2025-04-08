using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RendaFixa.Domain.Entities;

namespace RendaFixa.Infrastructure.Data.Configurations
{
    public class ProdutoRendaFixaConfiguration : IEntityTypeConfiguration<ProdutoRendaFixa>
    {
        public void Configure(EntityTypeBuilder<ProdutoRendaFixa> builder)
        {
            builder.ToTable("Produtos");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Nome).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Indexador).IsRequired().HasMaxLength(50);
            builder.Property(p => p.PrecoUnitario).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(p => p.Taxa).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(p => p.Estoque).IsRequired();
        }
    }
}
