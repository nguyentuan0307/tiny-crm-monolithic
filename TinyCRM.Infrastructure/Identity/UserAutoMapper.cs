using TinyCRM.Application.Helper.AutoMapper;
using TinyCRM.Application.Models.User;
using TinyCRM.Infrastructure.Identity.Users;

namespace TinyCRM.Infrastructure.Identity;

public class UserAutoMapper : TinyCrmAutoMapper
{
    public UserAutoMapper()
    {
        CreateMap<SignUpDto, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
        CreateMap<ApplicationUser, UserProfileDto>();
        CreateMap<ProfileUserUpdateDto, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
    }
}