using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCRM.Domain.Contacts;

namespace TinyCRM.Infrastructure.EntityTypeConfiguration
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(256);
            builder.Property(p => p.Email).IsRequired().HasMaxLength(256);
            builder.Property(p => p.Phone).IsRequired().HasMaxLength(20);
            builder.HasOne(a => a.Account)
                .WithMany(c => c.Contacts)
                .HasForeignKey(a => a.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
