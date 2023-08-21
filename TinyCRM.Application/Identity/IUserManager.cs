using TinyCRM.Application.Models.User;
using TinyCRM.Domain.Helper.QueryParameters;

namespace TinyCRM.Application.Identity;

public interface IUserManager
{
    Task<List<UserProfileDto>> GetUsersAsync(UserQueryParameters queryParameters);
    Task<UserProfileDto> GetUserAsync(string id);
    Task<UserProfileDto> CreateUserAsync(SignUpDto signUpDto);
    Task AddRoleAsync(string id, string name);
    Task<UserProfileDto> UpdateUserAsync(string id, ProfileUserUpdateDto updateDto);
    Task DeleteUserAsync(string id);
    Task ChangePasswordAsync(string id, UserChangePasswordDto changePasswordDto);
    Task RemoveRolesUserAsync(string id, IEnumerable<string> roles);
    Task AddRolesUserAsync(string id, IEnumerable<string> roles);
    Task<IList<string>> GetRolesAsync(string id);
}