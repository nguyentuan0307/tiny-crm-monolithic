using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TinyCRM.Domain.Entities.Products;

namespace TinyCRM.Infrastructure.EntityTypeConfiguration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Code).IsRequired().HasMaxLength(256);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(256);
        builder.Property(p => p.TypeProduct).IsRequired();
        builder.Property(p => p.Price).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(p => p.Status).IsRequired();

        builder.HasIndex(p => p.Code).IsUnique();
    }
}