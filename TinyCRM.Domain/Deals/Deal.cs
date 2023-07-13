using TinyCRM.Domain.Accounts;
using TinyCRM.Domain.Base;
using TinyCRM.Domain.Enums;
using TinyCRM.Domain.Leads;
using TinyCRM.Domain.ProductDeals;

namespace TinyCRM.Domain.Deals
{
    public class Deal : AuditEntity<Guid>
    {
        public Deal()
        {
            ProductDeals = new HashSet<ProductDeal>();
        }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public StatusDeal StatusDeal { get; set; }
        public decimal ActualRevenue { get; set; }
        public Guid LeadId { get; set; }
        public Lead? Lead { get; set; }
        public ICollection<ProductDeal> ProductDeals { get; set; }
    }
}
