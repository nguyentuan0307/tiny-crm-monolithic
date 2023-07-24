using Microsoft.EntityFrameworkCore;
using TinyCRM.Domain.Entities.Accounts;
using TinyCRM.Domain.Entities.Contacts;
using TinyCRM.Domain.Entities.Deals;
using TinyCRM.Domain.Entities.Leads;
using TinyCRM.Domain.Entities.ProductDeals;
using TinyCRM.Domain.Entities.Products;

namespace TinyCRM.Infrastructure
{
    public class AppDataContext : DbContext
    {
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<Contact> Contacts { get; set; } = null!;
        public DbSet<Lead> Leads { get; set; } = null!;
        public DbSet<Deal> Deals { get; set; } = null!;
        public DbSet<ProductDeal> ProductDeals { get; set; } = null!;

        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDataContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}