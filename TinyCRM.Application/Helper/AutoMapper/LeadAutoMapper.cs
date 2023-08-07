using TinyCRM.Application.Models.Lead;
using TinyCRM.Domain.Entities.Leads;

namespace TinyCRM.Application.Helper.AutoMapper;

public class LeadAutoMapper : TinyCrmAutoMapper
{
    public LeadAutoMapper()
    {
        CreateMap<Lead, LeadDto>()
            .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.Account.Name))
            .ReverseMap();
        CreateMap<LeadCreateDto, Lead>();
        CreateMap<LeadUpdateDto, Lead>();
        CreateMap<DisqualifyDto, Lead>();
    }
}