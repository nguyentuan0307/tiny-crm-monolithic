using AutoMapper;
using TinyCRM.API.Models.User;
using TinyCRM.Domain.Entities.Users;

namespace TinyCRM.API.Helper.AutoMapper
{
    public class UserAutoMapper : Profile
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