using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinyCRM.Application.Models.Permissions;
using TinyCRM.Application.Service.IServices;

namespace TinyCRM.API.Controllers;

[ApiController]
[Route("api/roles")]
public class RoleController : Controller
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    [Authorize(Policy = TinyCrmPermissions.Roles.Read)]
    public async Task<IActionResult> GetRolesAsync()
    {
        var roles = await _roleService.GetRolesAsync();
        return Ok(roles);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = TinyCrmPermissions.Roles.Read)]
    public async Task<IActionResult> GetRoleAsync(Guid id)
    {
        var role = await _roleService.GetRoleAsync(id);
        return Ok(role);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = TinyCrmPermissions.Roles.Edit)]
    public async Task<IActionResult> UpdateAsync(Guid id, RoleUpdateDto role)
    {
        var updatedRole = await _roleService.UpdateAsync(id, role);
        return Ok(updatedRole);
    }
}