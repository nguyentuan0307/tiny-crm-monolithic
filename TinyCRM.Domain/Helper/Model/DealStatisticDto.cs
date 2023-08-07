using TinyCRM.Domain.Enums;

namespace TinyCRM.Domain.Helper.Model;

public class DealStatisticDto
{
    public StatusDeal StatusDeal { get; set; }
    public decimal ActualRevenue { get; set; }
}