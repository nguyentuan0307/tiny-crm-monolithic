using TinyCRM.Domain.Helper.QueryParameters;
using TinyCRM.Domain.Interfaces;

namespace TinyCRM.Domain.Entities.Products;

public interface IProductRepository : IRepository<Product, Guid>
{
    Task<List<Product>> GetProductsAsync(ProductQueryParameters productQueryParameters);

    Task<bool> ProductCodeIsExistAsync(string code, Guid id);
}