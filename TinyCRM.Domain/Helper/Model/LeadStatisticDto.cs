using TinyCRM.Domain.Enums;

namespace TinyCRM.Domain.Helper.Model;

public class LeadStatisticDto
{
    public StatusLead StatusLead { get; set; }
    public decimal EstimatedRevenue { get; set; }
}