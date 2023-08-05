using TinyCRM.Application.Models.Deal;
using TinyCRM.Domain.Entities.Deals;

namespace TinyCRM.Infrastructure.Helper.AutoMapper
{
    public class DealAutoMapper : TinyCRMAutoMapper
    {
        public DealAutoMapper()
        {
            CreateMap<Deal, DealDto>().ReverseMap();
            CreateMap<DealUpdateDto, Deal>();
        }
    }
}