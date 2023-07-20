using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TinyCRM.Domain.Entities.ProductDeals;

namespace TinyCRM.Infrastructure.EntityTypeConfiguration
{
    public class ProductDealConfiguration : IEntityTypeConfiguration<ProductDeal>
    {
        public void Configure(EntityTypeBuilder<ProductDeal> builder)
        {
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(p => p.Quantity).IsRequired();
            builder.Property(p => p.TotalAmount).HasColumnType("decimal(18,2)").IsRequired();

            builder.HasOne(p => p.Product)
                .WithMany(pd => pd.ProductDeals)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.Deal)
                .WithMany(pd => pd.ProductDeals)
                .HasForeignKey(d => d.DealId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}