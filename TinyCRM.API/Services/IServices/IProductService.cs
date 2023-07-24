using TinyCRM.API.Models.Product;

namespace TinyCRM.API.Services.IServices
{
    public interface IProductService
    {
        public Task<ProductDto> CreateProductAsync(ProductCreateDto productDto);

        public Task DeleteProductAsync(Guid id);

        public Task<ProductDto> GetProductByIdAsync(Guid id);

        public Task<IList<ProductDto>> GetProductsAsync(ProductSearchDto search);

        public Task<ProductDto> UpdateProductAsync(Guid id, ProductUpdateDto productDto);
    }
}