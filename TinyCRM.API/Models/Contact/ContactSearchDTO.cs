using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TinyCRM.API.Models.Contact
{
    public class ContactSearchDTO : EntitySearchDTO
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [EnumDataType(typeof(EnumContactFilterSort))]
        public EnumContactFilterSort SortFilter { get; set; } = EnumContactFilterSort.Id;
    }
}