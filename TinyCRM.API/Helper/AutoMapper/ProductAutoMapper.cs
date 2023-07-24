using AutoMapper;
using TinyCRM.API.Models.Product;
using TinyCRM.Domain.Entities.Products;

namespace TinyCRM.API.Helper.AutoMapper
{
    public class ProductAutoMapper : Profile
    {
        public ProductAutoMapper()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ProductCreateDto, Product>();
            CreateMap<ProductUpdateDto, Product>();
        }
    }
}