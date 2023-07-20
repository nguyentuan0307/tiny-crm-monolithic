using TinyCRM.API.Models.Product;

namespace TinyCRM.API.Services.IServices
{
    public interface IProductService
    {
        Task<ProductDTO> CreateProductAsync(ProductCreateDTO productDTO);

        Task DeleteProductAsync(Guid id);

        Task<ProductDTO> GetProductByIdAsync(Guid id);

        Task<IList<ProductDTO>> GetProductsAsync(ProductSearchDTO search);

        Task<ProductDTO> UpdateProductAsync(Guid id, ProductUpdateDTO productDTO);
    }
}