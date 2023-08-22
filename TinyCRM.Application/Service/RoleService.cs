using TinyCRM.Application.Cache.Interface;
using TinyCRM.Application.Helper.Common;
using TinyCRM.Application.Identity;
using TinyCRM.Application.Models.Permissions;
using TinyCRM.Application.Service.IServices;
using TinyCRM.Domain.Exceptions;

namespace TinyCRM.Application.Service;

public class RoleService : IRoleService
{
    private readonly IPermissionCacheManager _permissionCacheManager;
    private readonly IRoleManager _roleManager;

    public RoleService(IRoleManager roleManager, IPermissionCacheManager permissionCacheManager)
    {
        _roleManager = roleManager;
        _permissionCacheManager = permissionCacheManager;
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

        var roleDto = await _roleManager.UpdateAsync(id, role);
        await _permissionCacheManager.ClearForRoleAsync(roleDto.Name);
        await _permissionCacheManager.SetForRoleAsync(roleDto.Name, role.Permissions);
        return roleDto;
    }
}