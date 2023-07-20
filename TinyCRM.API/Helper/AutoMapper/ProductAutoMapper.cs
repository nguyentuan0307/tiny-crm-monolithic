using AutoMapper;
using TinyCRM.API.Models.Product;
using TinyCRM.Domain.Entities.Products;

namespace TinyCRM.API.Helper.AutoMapper
{
    public class ProductAutoMapper : Profile
    {
        public ProductAutoMapper()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<ProductCreateDTO, Product>();
            CreateMap<ProductUpdateDTO, Product>();
        }
    }
}
