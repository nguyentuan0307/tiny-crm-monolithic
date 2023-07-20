using AutoMapper;
using TinyCRM.API.Models.Lead;
using TinyCRM.Domain.Entities.Leads;

namespace TinyCRM.API.Helper.AutoMapper
{
    public class LeadAutoMapper : Profile
    {
        public LeadAutoMapper()
        {
            CreateMap<Lead, LeadDTO>().ReverseMap();
            CreateMap<LeadCreateDTO, Lead>();
            CreateMap<LeadUpdateDTO, Lead>();
            CreateMap<DisqualifyDTO, Lead>();
        }
    }
}
