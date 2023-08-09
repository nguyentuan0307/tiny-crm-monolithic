namespace TinyCRM.Application.Models.Permissions;

public class RoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<string>? Permissions { get; set; }
}