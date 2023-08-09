namespace TinyCRM.Application.Models.Permissions;

public class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<RoleClaim>? Claims { get; set; }
}

public class RoleClaim
{
    public int Id { get; set; }
    public Guid RoleId { get; set; }
    public string ClaimType { get; set; } = null!;
    public string ClaimValue { get; set; } = null!;
}