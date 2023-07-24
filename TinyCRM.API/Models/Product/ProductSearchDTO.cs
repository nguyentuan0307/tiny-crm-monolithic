using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TinyCRM.API.Models.Product
{
    public class ProductSearchDTO : EntitySearchDTO
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [EnumDataType(typeof(EnumProductFilterSort))]
        public EnumProductFilterSort SortFilter { get; set; } = EnumProductFilterSort.Id;
    }
}