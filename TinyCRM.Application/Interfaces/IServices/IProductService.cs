using TinyCRM.Application.Models.Product;

namespace TinyCRM.Application.Interfaces.IServices
{
    public interface IProductService
    {
        public Task<ProductDto> CreateProductAsync(ProductCreateDto productDto);

        public Task DeleteProductAsync(Guid id);

        public Task<ProductDto> GetProductByIdAsync(Guid id);

        public Task<List<ProductDto>> GetProductsAsync(ProductSearchDto search);

        public Task<ProductDto> UpdateProductAsync(Guid id, ProductUpdateDto productDto);
    }
}