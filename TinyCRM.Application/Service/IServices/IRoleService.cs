using TinyCRM.Application.Models.Permissions;

namespace TinyCRM.Application.Service.IServices;

public interface IRoleService
{
    Task<List<RoleDto>> GetRolesAsync();

    Task<RoleDto> GetRoleAsync(Guid id);

    Task<RoleDto> UpdateAsync(Guid id, RoleUpdateDto role);
}