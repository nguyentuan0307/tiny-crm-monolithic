using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using TinyCRM.Application.Helper.Common;
using TinyCRM.Domain.Const;
using TinyCRM.Infrastructure.Identity.Role;

namespace TinyCRM.Infrastructure.SeedData;

public class PermissionContributor
{
    private readonly RoleManager<ApplicationRole> _roleManager;

    public PermissionContributor(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task SeedPermissionsAsync()
    {
        await SeedRolesAsync();
        var superAdmin = await _roleManager.FindByNameAsync(ConstRole.SuperAdmin);
        var admin = await _roleManager.FindByNameAsync(ConstRole.Admin);
        var user = await _roleManager.FindByNameAsync(ConstRole.User);

        await SeedSuperAdminPermissionsAsync(superAdmin!);
        await SeedUserPermissionsAsync(user!);
        await SeedAdminPermissionsAsync(admin!);
    }

    private async Task SeedSuperAdminPermissionsAsync(ApplicationRole superAdmin)
    {
        var permissionsClaims = Utilities.GetPermissionClaims();
        var valuePermissions = permissionsClaims.Select(p => p.Value).ToList();
        var valuePermissionsSuperAdmin = (await _roleManager.GetClaimsAsync(superAdmin)).Select(p => p.Value).ToList();

        var hashSetValuePermissions = new HashSet<string>(valuePermissions);
        var hashSetValuePermissionsSuperAdmin = new HashSet<string>(valuePermissionsSuperAdmin);

        var permissionsToAdd = hashSetValuePermissions.Except(hashSetValuePermissionsSuperAdmin);

        foreach (var permissionToAdd in permissionsToAdd)
            await _roleManager.AddClaimAsync(superAdmin, new Claim("Permission", permissionToAdd));

        var permissionsToRemove = hashSetValuePermissionsSuperAdmin.Except(hashSetValuePermissions);

        foreach (var permissionToRemove in permissionsToRemove)
            await _roleManager.RemoveClaimAsync(superAdmin, new Claim("Permission", permissionToRemove));
    }

    private async Task SeedUserPermissionsAsync(ApplicationRole user)
    {
        var permissionsClaims = Utilities.GetPermissionClaims();
        var userClaimsExisting = (await _roleManager.GetClaimsAsync(user)).Any();
        if (userClaimsExisting)
        {
            var userClaims = await _roleManager.GetClaimsAsync(user);
            foreach (var claim in userClaims)
                if (!permissionsClaims.Contains(claim))
                    await _roleManager.RemoveClaimAsync(user, claim);
        }

        foreach (var claim in permissionsClaims.Where(claim =>
                     claim.Value.Contains("Read") && !claim.Value.Contains("Roles")))
            await _roleManager.AddClaimAsync(user, claim);
    }

    private async Task SeedAdminPermissionsAsync(ApplicationRole admin)
    {
        var permissionsClaims = Utilities.GetPermissionClaims();
        var adminClaimsExisting = (await _roleManager.GetClaimsAsync(admin)).Any();
        if (adminClaimsExisting)
        {
            var adminClaims = await _roleManager.GetClaimsAsync(admin);
            foreach (var claim in adminClaims)
                if (!permissionsClaims.Contains(claim))
                    await _roleManager.RemoveClaimAsync(admin, claim);
        }

        foreach (var claim in permissionsClaims.Where(claim =>
                     !claim.Value.Contains("Roles") && !claim.Value.Contains("CreateAdmin")))
            await _roleManager.AddClaimAsync(admin, claim);
    }

    private async Task SeedRolesAsync()
    {
        var roles = new[] { ConstRole.SuperAdmin, ConstRole.Admin, ConstRole.User };

        foreach (var role in roles)
        {
            var roleExist = await _roleManager.RoleExistsAsync(role);
            if (!roleExist) await _roleManager.CreateAsync(new ApplicationRole(role));
        }
    }
}