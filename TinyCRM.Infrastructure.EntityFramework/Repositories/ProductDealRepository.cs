using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TinyCRM.Application.Helper.Specification.ProductDeals;
using TinyCRM.Domain.Entities.ProductDeals;
using TinyCRM.Domain.Helper.QueryParameters;

namespace TinyCRM.Infrastructure.Repositories;

public class ProductDealRepository : Repository<ProductDeal, Guid>, IProductDealRepository
{
    public ProductDealRepository(DbFactory dbFactory) : base(dbFactory)
    {
    }

    public async Task<List<ProductDeal>> GetProductDealsByDealIdAsync(
        ProductDealQueryParameters productDealQueryParameters)
    {
        var productDealsByDealIdSpecification =
            new ProductDealsByDealIdSpecification(productDealQueryParameters.DealId!.Value);

        var specification = productDealsByDealIdSpecification.And(
            new ProductDealsByFilterSpecification(productDealQueryParameters.KeyWord));

        return await ListAsync(specification,
            productDealQueryParameters.IncludeTables,
            productDealQueryParameters.Sorting,
            productDealQueryParameters.PageIndex,
            productDealQueryParameters.PageSize);
    }

    public override Task<bool> AnyAsync(Guid id)
    {
        return DbSet.AnyAsync(p => p.Id == id);
    }

    protected override Expression<Func<ProductDeal, bool>> ExpressionForGet(Guid id)
    {
        return p => p.Id == id;
    }
}