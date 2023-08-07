namespace TinyCRM.Application.Models;

public class JwtSettings
{
    public string ValidAudience { get; set; } = null!;
    public string ValidIssuer { get; set; } = null!;
    public string SecretKey { get; set; } = null!;
    public int ExpiryInMinutes { get; set; }
}