using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TinyCRM.API.Models.ProductDeal;
using TinyCRM.Domain.Enums;

namespace TinyCRM.API.Models.Deal
{
    public class DealDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public StatusDeal StatusDeal { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "ActualRevenue must be non-negative")]
        public decimal ActualRevenue { get; set; }

        public Guid LeadId { get; set; }
        public ICollection<ProductDealDTO> ProductDeals { get; set; } = null!;
    }
}