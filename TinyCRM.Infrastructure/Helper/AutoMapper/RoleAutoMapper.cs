using Microsoft.AspNetCore.Identity;
using TinyCRM.Application.Models.Permissions;
using TinyCRM.Infrastructure.Identity.Role;

namespace TinyCRM.Infrastructure.Helper.AutoMapper;

public class RoleAutoMapper : InfraAutoMapper
{
    public RoleAutoMapper()
    {
        CreateMap<RoleUpdateDto, ApplicationRole>()
            .ForMember(r => r.Claims, opt => opt.MapFrom(dto => dto.Permissions!.Select(p => new IdentityRoleClaim<string>() { ClaimType = "Permission", ClaimValue = p })));

        CreateMap<ApplicationRole, RoleDto>()
            .ForMember(dest => dest.Permissions, opt => opt.MapFrom(r => r.Claims!.Select(p => p.ClaimValue).ToList()));
    }
}