using TinyCRM.API.Models.ProductDeal;

namespace TinyCRM.API.Services.IServices
{
    public interface IProductDealService
    {
        public Task<ProductDealDto> CreateProductDealAsync(Guid dealId, ProductDealCreateDto productDealDto);

        public Task DeleteProductDealAsync(Guid dealId, Guid productDealId);

        public Task<ProductDealDto> GetProductDealByIdAsync(Guid dealId, Guid productDealId);

        public Task<IList<ProductDealDto>> GetProductDealsByDealIdAsync(Guid dealId, ProductDealSearchDto search);

        public Task<ProductDealDto> UpdateProductDealAsync(Guid dealId, Guid productDealId, ProductDealUpdateDto productDealDto);
    }
}