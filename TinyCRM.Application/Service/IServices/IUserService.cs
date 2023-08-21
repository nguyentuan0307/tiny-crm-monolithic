using System.Security.Claims;
using TinyCRM.Application.Models.User;

namespace TinyCRM.Application.Service.IServices;

public interface IUserService
{
    Task<UserProfileDto> SignUpAsync(SignUpDto signUpDto);
    
    Task<UserProfileDto> GetProfileAsync(string id);

    Task<List<UserProfileDto>> GetProfilesAsync(ProfileUserSearchDto search);

    Task<UserProfileDto> SignUpAdminAsync(SignUpDto signUpDto);

    Task<UserProfileDto> UpdateProfileAsync(string id, ProfileUserUpdateDto updateDto, ClaimsPrincipal user);

    Task ChangePasswordAsync(string id, UserChangePasswordDto changePasswordDto, ClaimsPrincipal user);

    Task DeleteAsync(string id, bool isSuperAdmin = false);

    Task UpdateRoleAsync(string id, string[] roleIds);
}