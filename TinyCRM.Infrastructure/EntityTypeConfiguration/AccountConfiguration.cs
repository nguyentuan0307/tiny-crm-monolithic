using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TinyCRM.Domain.Entities.Accounts;

namespace TinyCRM.Infrastructure.EntityTypeConfiguration
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(256);
            builder.Property(p => p.Email).IsRequired().HasMaxLength(256);
            builder.Property(p => p.Phone).IsRequired().HasMaxLength(20);
            builder.Property(p => p.Address).HasMaxLength(512);
            builder.Property(p => p.TotalSale).HasColumnType("decimal(18,2)");

            var accounts = new List<Account>();

            for (int i = 1; i <= 10; i++)
            {
                accounts.Add(new Account()
                {
                    Id = Guid.NewGuid(),
                    Name = $"Account {i}",
                    Email = $"account{i}@gmail.com",
                    Address = $"Address - {i}",
                    Phone = i.ToString(),
                    TotalSale = 0,
                    CreatedDate = DateTime.Now,
                });
            }

            builder.HasData(accounts);
        }
    }
}
