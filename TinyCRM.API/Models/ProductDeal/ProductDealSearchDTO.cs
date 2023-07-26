using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TinyCRM.API.Models.ProductDeal
{
    public class ProductDealSearchDto : EntitySearchDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [EnumDataType(typeof(EnumProductDealFilterSort))]
        public EnumProductDealFilterSort? SortFilter { get; set; }
    }
}