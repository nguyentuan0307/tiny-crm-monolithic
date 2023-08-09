using Microsoft.AspNetCore.Identity;

namespace TinyCRM.Infrastructure.Identity.Role
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole()
        { }

        public ApplicationRole(string role) : base(roleName: role)
        { }

        public ICollection<IdentityRoleClaim<string>>? Claims { get; set; }
    }
}