using TinyCRM.Application.Models.Deal;
using TinyCRM.Domain.Entities.Deals;

namespace TinyCRM.Application.Helper.AutoMapper;

public class DealAutoMapper : TinyCrmAutoMapper
{
    public DealAutoMapper()
    {
        CreateMap<Deal, DealDto>().ReverseMap();
        CreateMap<DealUpdateDto, Deal>();
    }
}