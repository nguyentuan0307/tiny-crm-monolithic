using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TinyCRM.API.Models.Lead
{
    public class LeadSearchDto : EntitySearchDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [EnumDataType(typeof(EnumLeadFilterSort))]
        public EnumLeadFilterSort? SortFilter { get; set; }
    }
}