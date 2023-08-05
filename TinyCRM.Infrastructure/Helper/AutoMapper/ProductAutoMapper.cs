using TinyCRM.Application.Models.Product;
using TinyCRM.Domain.Entities.Products;

namespace TinyCRM.Infrastructure.Helper.AutoMapper
{
    public class ProductAutoMapper : TinyCRMAutoMapper
    {
        public ProductAutoMapper()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ProductCreateDto, Product>();
            CreateMap<ProductUpdateDto, Product>();
        }
    }
}