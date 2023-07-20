using TinyCRM.Domain.Base;
using TinyCRM.Domain.Entities.Leads;
using TinyCRM.Domain.Entities.ProductDeals;
using TinyCRM.Domain.Enums;

namespace TinyCRM.Domain.Entities.Deals
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
        public Lead Lead { get; set; } = null!;
        public ICollection<ProductDeal> ProductDeals { get; set; }
    }
}
