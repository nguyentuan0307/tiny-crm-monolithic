using TinyCRM.Application.Models.ProductDeal;
using TinyCRM.Domain.Entities.ProductDeals;

namespace TinyCRM.Application.Helper.AutoMapper;

public class ProductDealAutoMapper : TinyCrmAutoMapper
{
    public ProductDealAutoMapper()
    {
        CreateMap<ProductDeal, ProductDealDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));
        CreateMap<ProductDealUpdateDto, ProductDeal>()
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Price * src.Quantity));
        CreateMap<ProductDealCreateDto, ProductDeal>()
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Price * src.Quantity));
    }
}