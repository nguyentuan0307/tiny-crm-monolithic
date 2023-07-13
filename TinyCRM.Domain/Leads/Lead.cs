using TinyCRM.Domain.Accounts;
using TinyCRM.Domain.Base;
using TinyCRM.Domain.Deals;
using TinyCRM.Domain.Enums;

namespace TinyCRM.Domain.Leads
{
    public class Lead : AuditEntity<Guid>
    {
        public Lead()
        {
            Deals = new HashSet<Deal>();
        }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public Guid AccountId { get; set; }
        public Account? Account { get; set; }
        public StatusLead StatusLead { get; set; }
        public SourceLead SourceLead { get; set; }
        public DateTime DateQuanlified { get; set; }
        public decimal EstimatedRevenue { get; set; }
        public ICollection<Deal> Deals { get; set; }
    }
}
