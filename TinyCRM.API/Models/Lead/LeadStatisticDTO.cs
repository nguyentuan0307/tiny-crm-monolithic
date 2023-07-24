namespace TinyCRM.API.Models.Lead
{
    public class LeadStatisticDto
    {
        public int ProspectLeads { get; set; }
        public int OpenLeads { get; set; }
        public int QualifiedLeads { get; set; }
        public int DisqualifiedLeads { get; set; }
        public decimal AvgEstimatedRevenue { get; set; }
    }
}