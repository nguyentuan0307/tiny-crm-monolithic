using TinyCRM.API.Models.Product;

namespace TinyCRM.API.Services.IServices
{
    public interface IProductService
    {
        public Task<ProductDTO> CreateProductAsync(ProductCreateDTO productDTO);

        public Task DeleteProductAsync(Guid id);

        public Task<ProductDTO> GetProductByIdAsync(Guid id);

        public Task<IList<ProductDTO>> GetProductsAsync(ProductSearchDTO search);

        public Task<ProductDTO> UpdateProductAsync(Guid id, ProductUpdateDTO productDTO);
    }
}