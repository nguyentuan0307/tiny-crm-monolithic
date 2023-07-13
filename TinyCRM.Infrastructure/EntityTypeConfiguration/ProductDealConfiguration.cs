using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCRM.Domain.ProductDeals;

namespace TinyCRM.Infrastructure.EntityTypeConfiguration
{
    public class ProductDealConfiguration : IEntityTypeConfiguration<ProductDeal>
    {
        public void Configure(EntityTypeBuilder<ProductDeal> builder)
        {
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(p => p.Quantity).IsRequired();

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
