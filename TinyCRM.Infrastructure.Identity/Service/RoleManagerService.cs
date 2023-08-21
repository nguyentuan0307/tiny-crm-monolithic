using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TinyCRM.Application.Identity;
using TinyCRM.Application.Models.Permissions;
using TinyCRM.Domain.Const;
using TinyCRM.Domain.Exceptions;
using TinyCRM.Infrastructure.Identity.Role;

namespace TinyCRM.Infrastructure.Identity.Service;

public class RoleManagerService : IRoleManager
{
    private readonly IMapper _mapper;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public RoleManagerService(RoleManager<ApplicationRole> roleManager, IMapper mapper)
    {
        _roleManager = roleManager;
        _mapper = mapper;
    }

    public async Task<List<RoleDto>> GetRolesAsync()
    {
        var appRole = await FindRolesAsync();

        return _mapper.Map<List<RoleDto>>(appRole);
    }

    public async Task<RoleDto> GetRoleAsync(Guid id)
    {
        var appRole = await FindRoleAsync(id);

        return _mapper.Map<RoleDto>(appRole);
    }

    public async Task<RoleDto> UpdateAsync(Guid id, RoleUpdateDto role)
    {
        var appRole = await FindRoleAsync(id);

        if (appRole.Name == ConstRole.SuperAdmin)
            throw new InvalidUpdateException("Cannot update super admin role");
        
        _mapper.Map(role, appRole);
        var result = await _roleManager.UpdateAsync(appRole);

        if (!result.Succeeded)
            throw new InvalidUpdateException(result.Errors.First().Description);

        return _mapper.Map<RoleDto>(appRole);
    }

    private async Task<ApplicationRole> FindRoleAsync(Guid id)
    {
        var appRole = await _roleManager.Roles.Include(p => p.Claims).FirstOrDefaultAsync(ar => ar.Id == id.ToString())
                      ?? throw new EntityNotFoundException($"Role with id {id} not found");
        return appRole;
    }

    private async Task<List<ApplicationRole>> FindRolesAsync()
    {
        var appRoles = await _roleManager.Roles.Include(p => p.Claims).ToListAsync();
        return appRoles;
    }
}