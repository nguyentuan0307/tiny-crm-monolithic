using System.Security.Claims;

namespace TinyCRM.Application.Identity;

public interface IAuthManager
{
    public Task<(string, string, IList<string>)> SignInAsync(string userNameOrEmail, string password);
    public Task<List<Claim>> GenerateAuthClaims(string id, string email, IList<string> roles);
}