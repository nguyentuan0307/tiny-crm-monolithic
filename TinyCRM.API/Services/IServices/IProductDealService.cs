using TinyCRM.API.Models.ProductDeal;

namespace TinyCRM.API.Services.IServices
{
    public interface IProductDealService
    {
        public Task<ProductDealDTO> CreateProductDealAsync(Guid id, ProductDealCreateDTO productDealDTO);

        public Task DeleteProductDealAsync(Guid id, Guid productDealId);

        public Task<ProductDealDTO> GetProductDealByIdAsync(Guid id, Guid productDealId);

        public Task<IList<ProductDealDTO>> GetProductDealsByDealIdAsync(Guid id);

        public Task<ProductDealDTO> UpdateProductDealAsync(Guid id, Guid productDealId, ProductDealUpdateDTO productDealDTO);
    }
}