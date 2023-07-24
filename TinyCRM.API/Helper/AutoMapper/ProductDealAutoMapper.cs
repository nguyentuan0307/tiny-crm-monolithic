using AutoMapper;
using TinyCRM.API.Models.ProductDeal;
using TinyCRM.Domain.Entities.ProductDeals;

namespace TinyCRM.API.Helper.AutoMapper
{
    public class ProductDealAutoMapper : Profile
    {
        public ProductDealAutoMapper()
        {
            CreateMap<ProductDeal, ProductDealDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty));
            CreateMap<ProductDealUpdateDto, ProductDeal>()
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Price * src.Quantity));
            CreateMap<ProductDealCreateDto, ProductDeal>()
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Price * src.Quantity));
        }
    }
}