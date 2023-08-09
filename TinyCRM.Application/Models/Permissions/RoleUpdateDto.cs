namespace TinyCRM.Application.Models.Permissions;

public class RoleUpdateDto
{
    public ICollection<string> Permissions { get; set; } = new List<string>();
}