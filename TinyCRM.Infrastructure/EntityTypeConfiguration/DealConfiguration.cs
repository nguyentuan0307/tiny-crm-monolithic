using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TinyCRM.Domain.Entities.Deals;

namespace TinyCRM.Infrastructure.EntityTypeConfiguration
{
    public class DealConfiguration : IEntityTypeConfiguration<Deal>
    {
        public void Configure(EntityTypeBuilder<Deal> builder)
        {
            builder.Property(p => p.Title).IsRequired().HasMaxLength(256);
            builder.Property(p => p.Description).HasMaxLength(1024);
            builder.Property(p => p.StatusDeal).IsRequired();
            builder.Property(p => p.ActualRevenue).HasColumnType("decimal(18,2)").IsRequired();

            builder.HasOne(l => l.Lead)
                .WithOne(d => d.Deal)
                .HasForeignKey<Deal>(u => u.LeadId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
