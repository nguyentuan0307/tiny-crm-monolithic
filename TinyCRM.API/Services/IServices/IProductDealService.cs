using TinyCRM.API.Models.ProductDeal;

namespace TinyCRM.API.Services.IServices
{
    public interface IProductDealService
    {
        public Task<ProductDealDTO> CreateProductDealAsync(Guid dealId, ProductDealCreateDTO productDealDTO);

        public Task DeleteProductDealAsync(Guid dealId, Guid productDealId);

        public Task<ProductDealDTO> GetProductDealByIdAsync(Guid dealId, Guid productDealId);

        public Task<IList<ProductDealDTO>> GetProductDealsByDealIdAsync(Guid dealId, ProductDealSearchDTO search);

        public Task<ProductDealDTO> UpdateProductDealAsync(Guid dealId, Guid productDealId, ProductDealUpdateDTO productDealDTO);
    }
}