using TinyCRM.Domain.Enums;

namespace TinyCRM.Infrastructure.Helpers.Model
{
    public class DealStatistic
    {
        public StatusDeal StatusDeal { get; set; }
        public decimal ActualRevenue { get; set; }
    }
}