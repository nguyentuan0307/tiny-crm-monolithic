using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TinyCRM.Domain.Enums;

namespace TinyCRM.Application.Models.Deal;

public class DealDto
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
}