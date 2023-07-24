using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TinyCRM.Domain.Enums;

namespace TinyCRM.API.Models.Lead
{
    public class LeadUpdateDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public Guid AccountId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [EnumDataType(typeof(StatusLead))]
        public StatusLead StatusLead { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [EnumDataType(typeof(SourceLead))]
        public SourceLead SourceLead { get; set; }

        public decimal EstimatedRevenue { get; set; }
    }
}