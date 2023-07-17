using TinyCRM.API.Models.Product;

namespace TinyCRM.API.Services.IServices
{
    public interface IProductService
    {
        Task<ProductDTO> CreateProductAsync(ProductCreateDTO productDTO);
        Task DeleteProductAsync(string id);
        Task<ProductDTO> GetProductByIdAsync(string id);
        Task<List<ProductDTO>> GetProductsAsync(ProductSearchDTO search);
        Task<ProductDTO> UpdateProductAsync(string id, ProductUpdateDTO productDTO);
    }
}
