using TinyCRM.Domain.Entities.ProductDeals;

namespace TinyCRM.Infrastructure.Repositories
{
    public class ProductDealRepository : Repository<ProductDeal>, IProductDealRepository
    {
        public ProductDealRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}