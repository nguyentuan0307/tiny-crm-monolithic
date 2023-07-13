using TinyCRM.Domain.Base;
using TinyCRM.Domain.Contacts;
using TinyCRM.Domain.Leads;

namespace TinyCRM.Domain.Accounts
{
    public class Account : AuditEntity<Guid>
    {
        public Account()
        {
            Contacts = new HashSet<Contact>();
            Leads = new HashSet<Lead>();
        }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? Address { get; set; }
        public decimal TotalSale { get; set; }
        public ICollection<Contact> Contacts { get; set; }
        public ICollection<Lead> Leads { get; set; }
    }
}
