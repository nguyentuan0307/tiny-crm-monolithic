using System.Text.Json.Serialization;
using TinyCRM.Domain.Enums;

namespace TinyCRM.Application.Models.Lead
{
    public class LeadDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public Guid AccountId { get; set; }
        public string? AccountName { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public StatusLead StatusLead { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SourceLead SourceLead { get; set; }

        public DateTime DateQuanlified { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ReasonDisqualification? ReasonDisqualification { get; set; }

        public string? DescriptionDisqualification { get; set; }
        public decimal EstimatedRevenue { get; set; }
    }
}