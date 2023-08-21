using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using TinyCRM.Application.Identity;
using TinyCRM.Domain.Exceptions;
using TinyCRM.Infrastructure.Identity.Role;
using TinyCRM.Infrastructure.Identity.Users;

namespace TinyCRM.Infrastructure.Identity.Service;

public class AuthManagerService:IAuthManager
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public AuthManagerService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
    }
    public async Task<(string,string,IList<string>)> SignInAsync(string userNameOrEmail, string password)
    {
        var user = await _userManager.FindByEmailAsync(userNameOrEmail)
                   ?? throw new EntityNotFoundException($"User with Email{userNameOrEmail}] not found");

        var result = await _signInManager.PasswordSignInAsync(user, password, false, true);
        if (!result.Succeeded) throw new InvalidPasswordException("Invalid password");

        var roles = await _userManager.GetRolesAsync(user);

        return (user.Id, user.Email!, roles);
    }
    
    public async Task<List<Claim>> GenerateAuthClaims(string id, string email, IList<string> roles)
    {
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, id),
            new(ClaimTypes.Email, email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        authClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        // foreach (var roleName in roles)
        // {
        //     var role = await _roleManager.FindByNameAsync(roleName);
        //     if (role == null) continue;
        //     var permissions = await GetPermissionsForRoleAsync(role);
        //     authClaims.AddRange(permissions.Select(permission => new Claim("Permission", permission)));
        // }

        return authClaims;
    }
    
    private async Task<IEnumerable<string>> GetPermissionsForRoleAsync(ApplicationRole role)
    {
        var claims = await _roleManager.GetClaimsAsync(role);

        return Enumerable.ToList<string>((from claim in claims where claim.Type == "Permission" select claim.Value));
    }
}