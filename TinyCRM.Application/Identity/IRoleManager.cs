using TinyCRM.Application.Models.Permissions;

namespace TinyCRM.Application.Identity;

public interface IRoleManager
{
    Task<List<RoleDto>> GetRolesAsync();

    Task<RoleDto> GetRoleAsync(Guid id);

    Task<RoleDto> UpdateAsync(Guid id, RoleUpdateDto role);
}