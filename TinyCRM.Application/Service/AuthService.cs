using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TinyCRM.Application.Cache.Interface;
using TinyCRM.Application.Identity;
using TinyCRM.Application.Models;
using TinyCRM.Application.Models.User;
using TinyCRM.Application.Service.IServices;

namespace TinyCRM.Application.Service;

public class AuthService : IAuthService
{
    private readonly IAuthManager _authManager;
    private readonly JwtSettings _jwtSettings;
    private readonly IPermissionCacheManager _permissionCacheManager;
    private readonly IRoleManager _roleManager;


    public AuthService(IAuthManager authManager, JwtSettings jwtSettings,
        IPermissionCacheManager permissionCacheManager, IRoleManager roleManager)
    {
        _authManager = authManager;
        _jwtSettings = jwtSettings;
        _permissionCacheManager = permissionCacheManager;
        _roleManager = roleManager;
    }

    public async Task<string> SignInAsync(SignInDto signInDto)
    {
        var (id, email, roles) = await _authManager.SignInAsync(signInDto.Email, signInDto.Password);
        return GenerateToken(id, email, roles);
    }

    public async Task<IEnumerable<string>> GetPermissionsForUserAsync(string userId)
    {
        var permissions = new List<string>();
        foreach (var role in await _roleManager.GetRolesForUserAsync(userId))
        {
            var rolePermissions = await _permissionCacheManager.GetForRoleAsync(role);
            if (rolePermissions == null)
            {
                rolePermissions = await _roleManager.GetPermissionsForRoleAsync(role);
                await _permissionCacheManager.SetForRoleAsync(role, rolePermissions);
            }

            permissions.AddRange(rolePermissions);
        }

        return permissions;
    }

    private static IEnumerable<Claim> GenerateAuthClaims(string id, string email, IEnumerable<string> roles)
    {
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, id),
            new(ClaimTypes.Email, email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        authClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        return authClaims;
    }

    private string GenerateToken(string id, string email, IEnumerable<string> roles)
    {
        var claims = GenerateAuthClaims(id, email, roles);
        var authKey = GenerateAuthKey();
        var token = GenerateJwtToken(claims, authKey);
        return WriteJwtToken(token);
    }

    private JwtSecurityToken GenerateJwtToken(IEnumerable<Claim> authClaims, SecurityKey authKey)
    {
        return new JwtSecurityToken(
            _jwtSettings.ValidIssuer,
            _jwtSettings.ValidAudience,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256)
        );
    }

    private static string WriteJwtToken(SecurityToken token)
    {
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private SymmetricSecurityKey GenerateAuthKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
    }
}