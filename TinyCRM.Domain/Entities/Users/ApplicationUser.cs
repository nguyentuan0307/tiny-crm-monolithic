using Microsoft.AspNetCore.Identity;

namespace TinyCRM.Domain.Entities.Users
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = null!;
    }
}
