using TinyCRM.Application.Models.User;
using TinyCRM.Infrastructure.Identity.Users;

namespace TinyCRM.Infrastructure.Helper.AutoMapper
{
    public class UserAutoMapper : TinyCRMAutoMapper
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
}