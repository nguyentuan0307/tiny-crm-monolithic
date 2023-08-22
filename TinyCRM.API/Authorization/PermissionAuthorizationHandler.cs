using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using TinyCRM.Application.Service.IServices;

namespace TinyCRM.API.Authorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IAuthService _authService;

    public PermissionAuthorizationHandler(IAuthService authService)
    {
        _authService = authService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId != null)
        {
            var permissions = await _authService.GetPermissionsForUserAsync(userId);

            var isAuthorized = permissions.Contains(requirement.Permission);
            if (isAuthorized) context.Succeed(requirement);
        }
    }
}