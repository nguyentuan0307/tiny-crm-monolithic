using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TinyCRM.Domain.Entities.Products;
using TinyCRM.Domain.Helper.QueryParameters;
using TinyCRM.Domain.Helper.Specification.Products;

namespace TinyCRM.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product, Guid>, IProductRepository
    {
        public ProductRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public IQueryable<Product> GetProducts(ProductQueryParameters productQueryParameters)
        {
            var specification = new ProductsByFilterSpecification(productQueryParameters.KeyWord);
            return List(specification: specification,
                includeTables: productQueryParameters.IncludeTables,
                sorting: productQueryParameters.Sorting,
                pageIndex: productQueryParameters.PageIndex,
                pageSize: productQueryParameters.PageSize);
        }

        public Task<bool> ProductCodeIsExistAsync(string code, Guid id)
        {
            return DbSet.AnyAsync(p => p.Code.Equals(code) && p.Id != id);
        }

        protected override Expression<Func<Product, bool>> ExpressionForGet(Guid id)
        {
            return p => p.Id == id;
        }

        public override Task<bool> AnyAsync(Guid id)
        {
            return DbSet.AnyAsync(p => p.Id == id);
        }
    }
}