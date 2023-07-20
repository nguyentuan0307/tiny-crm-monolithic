using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TinyCRM.Domain.Entities.ProductDeals;

namespace TinyCRM.Infrastructure.Repositories
{
    public class ProductDealRepository : Repository<ProductDeal>, IProductDealRepository
    {
        public ProductDealRepository(DbFactory dbFactory) : base(dbFactory) { }

        public override Task<ProductDeal?> GetAsync(Expression<Func<ProductDeal, bool>> expression)
        {
            return DbSet.Include(p => p.Product).FirstOrDefaultAsync(expression);

        }
    }
}
