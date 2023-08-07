using TinyCRM.Application.Models.Product;
using TinyCRM.Domain.Entities.Products;

namespace TinyCRM.Application.Helper.AutoMapper;

public class ProductAutoMapper : TinyCrmAutoMapper
{
    public ProductAutoMapper()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<ProductCreateDto, Product>();
        CreateMap<ProductUpdateDto, Product>();
    }
}