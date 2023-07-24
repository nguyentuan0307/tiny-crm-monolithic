using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TinyCRM.API.Models.ProductDeal
{
    public class ProductDealSearchDTO : EntitySearchDTO
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [EnumDataType(typeof(EnumProductDealFilterSort))]
        public EnumProductDealFilterSort SortFilter { get; set; } = EnumProductDealFilterSort.Id;
    }
}
