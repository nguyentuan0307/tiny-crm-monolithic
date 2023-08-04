using TinyCRM.Domain.Helper.QueryParameters;
using TinyCRM.Domain.Interfaces;

namespace TinyCRM.Domain.Entities.ProductDeals
{
    public interface IProductDealRepository : IRepository<ProductDeal, Guid>
    {
        Task<List<ProductDeal>> GetProductDealsByDealIdAsync(ProductDealQueryParameters productDealQueryParameters);
    }
}