using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TinyCRM.Domain.Entities.ProductDeals;
using TinyCRM.Domain.Helper.QueryParameters;
using TinyCRM.Domain.Helper.Specification.ProductDeals;

namespace TinyCRM.Infrastructure.Repositories
{
    public class ProductDealRepository : Repository<ProductDeal, Guid>, IProductDealRepository
    {
        public ProductDealRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public IQueryable<ProductDeal> GetProductDealsByDealId(ProductDealQueryParameters productDealQueryParameters)
        {
            var productDealsByDealIdSpecification = new ProductDealsByDealIdSpecification(productDealQueryParameters.DealId!.Value);

            var specification = productDealsByDealIdSpecification.And(
                    new ProductDealsByFilterSpecification(productDealQueryParameters.KeyWord));

            return List(specification: specification,
                includeTables: productDealQueryParameters.IncludeTables,
                sorting: productDealQueryParameters.Sorting,
                pageIndex: productDealQueryParameters.PageIndex,
                pageSize: productDealQueryParameters.PageSize);
        }

        protected override Expression<Func<ProductDeal, bool>> ExpressionForGet(Guid id)
        {
            return p => p.Id == id;
        }

        public override Task<bool> AnyAsync(Guid id)
        {
            return DbSet.AnyAsync(p => p.Id == id);
        }
    }
}