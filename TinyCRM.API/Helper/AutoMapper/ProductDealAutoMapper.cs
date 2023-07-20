using AutoMapper;
using TinyCRM.API.Models.ProductDeal;
using TinyCRM.Domain.Entities.Deals;
using TinyCRM.Domain.Entities.ProductDeals;

namespace TinyCRM.API.Helper.AutoMapper
{
    public class ProductDealAutoMapper : Profile
    {
        public ProductDealAutoMapper()
        {
            CreateMap<ProductDeal, ProductDealDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty));
            CreateMap<ProductDealUpdateDTO, ProductDeal>()
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Price * src.Quantity));
            CreateMap<ProductDealCreateDTO, ProductDeal>()
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Price * src.Quantity));
        }
    }
}
