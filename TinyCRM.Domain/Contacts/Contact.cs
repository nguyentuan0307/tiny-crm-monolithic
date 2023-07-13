using TinyCRM.Domain.Accounts;
using TinyCRM.Domain.Base;

namespace TinyCRM.Domain.Contacts
{
    public class Contact : AuditEntity<Guid>
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public Guid AccountId { get; set; }
        public Account? Account { get; set; }
    }
}
