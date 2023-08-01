using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using TinyCRM.API.Models.User;

namespace TinyCRM.API.Services.IServices
{
    public interface IUserService
    {
        Task<IdentityResult> SignUpAsync(SignUpDto signUpDto);

        Task<string> SignInAsync(SignInDto signInDto);

        Task<UserProfileDto> GetProfileAsync(string id);

        Task<IEnumerable<UserProfileDto>> GetProfileUsersAsync(ProfileUserSearchDto search);

        Task<IdentityResult> SignUpAdminAsync(SignUpDto signUpDto);

        Task<UserProfileDto> UpdateProfileAsync(string id, ProfileUserUpdateDto updateDto, ClaimsPrincipal user);

        Task<IdentityResult> ChangePasswordAsync(string id, UserChangePasswordDto changePasswordDto, ClaimsPrincipal user);
    }
}