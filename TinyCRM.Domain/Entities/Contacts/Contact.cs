using TinyCRM.Domain.Entities.Accounts;
using TinyCRM.Domain.Entities.Base;

namespace TinyCRM.Domain.Entities.Contacts;

public class Contact : AuditEntity<Guid>
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public Guid AccountId { get; set; }
    public Account? Account { get; set; }
}