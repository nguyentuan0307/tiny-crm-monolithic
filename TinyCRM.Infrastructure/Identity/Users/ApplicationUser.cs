using Microsoft.AspNetCore.Identity;

namespace TinyCRM.Infrastructure.Identity.Users;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; } = null!;
}