using AutoMapper;
using TinyCRM.API.Models.Deal;
using TinyCRM.Domain.Entities.Deals;

namespace TinyCRM.API.Helper.AutoMapper
{
    public class DealAutoMapper : Profile
    {
        public DealAutoMapper()
        {
            CreateMap<Deal, DealDTO>().ReverseMap();
            CreateMap<DealUpdateDTO, Deal>();
        }
    }
}