using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TinyCRM.API.Models.Contact
{
    public class ContactSearchDto : EntitySearchDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [EnumDataType(typeof(EnumContactFilterSort))]
        public EnumContactFilterSort? SortFilter { get; set; }
    }
}