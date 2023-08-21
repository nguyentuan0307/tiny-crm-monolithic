using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TinyCRM.Application.Identity;
using TinyCRM.Application.Models;
using TinyCRM.Application.Models.User;
using TinyCRM.Application.Service.IServices;

namespace TinyCRM.Application.Service;

public class AuthService:IAuthService
{
    private readonly IAuthManager _authManager;
    private readonly JwtSettings _jwtSettings;

    public AuthService(IAuthManager authManager,IOptions<JwtSettings> jwtOptions)
    {
        _authManager = authManager;
        _jwtSettings = jwtOptions.Value;
    }
    public async Task<string> SignInAsync(SignInDto signInDto)
    {
        var (id,email,roles) = await _authManager.SignInAsync(signInDto.Email, signInDto.Password);
        var claims = await _authManager.GenerateAuthClaims(id, email, roles);
        return GenerateToken(claims);
    }
    private string GenerateToken(IEnumerable<Claim> claims)
    {
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