using TinyCRM.Application.Helper.Common;
using TinyCRM.Application.Identity;
using TinyCRM.Application.Models.Permissions;
using TinyCRM.Application.Service.IServices;
using TinyCRM.Domain.Exceptions;

namespace TinyCRM.Application.Service;

public class RoleService:IRoleService
{
    private readonly IRoleManager _roleManager;

    public RoleService(IRoleManager roleManager)
    {
        _roleManager = roleManager;
    }
    public async Task<List<RoleDto>> GetRolesAsync()
    {
         return await _roleManager.GetRolesAsync();
    }

    public async Task<RoleDto> GetRoleAsync(Guid id)
    {
        return await _roleManager.GetRoleAsync(id);
    }

    public async Task<RoleDto> UpdateAsync(Guid id, RoleUpdateDto role)
    {
        role.Permissions = role.Permissions.Distinct().ToList();
        var invalidPermissions = Utilities.ListInvalidPermissions(role.Permissions);
        if (invalidPermissions.Any())
            throw new InvalidUpdateException($"Invalid permissions[{string.Join(", ", invalidPermissions)}]");

        return await _roleManager.UpdateAsync(id, role);
    }
}