using TinyCRM.API.Models.ProductDeal;

namespace TinyCRM.API.Services.IServices
{
    public interface IProductDealService
    {
        Task<ProductDealDTO> CreateProductDealAsync(Guid id, ProductDealCreateDTO productDealDTO);

        Task DeleteProductDealAsync(Guid id, Guid productDealId);

        Task<ProductDealDTO> GetProductDealByIdAsync(Guid id, Guid productDealId);

        Task<IList<ProductDealDTO>> GetProductDealsByDealIdAsync(Guid id);

        Task<ProductDealDTO> UpdateProductDealAsync(Guid id, Guid productDealId, ProductDealUpdateDTO productDealDTO);
    }
}