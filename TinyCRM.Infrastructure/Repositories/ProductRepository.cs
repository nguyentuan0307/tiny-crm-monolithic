using TinyCRM.Domain.Entities.Products;

namespace TinyCRM.Infrastructure.Repositories
{
    publics class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(DbFactory dbFactory) : base(dbFactory) { }
    }
}
