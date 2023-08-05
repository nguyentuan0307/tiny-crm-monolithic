using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TinyCRM.Application.Models.Product
{
    public class ProductSearchDto : EntitySearchDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [EnumDataType(typeof(EnumProductFilterSort))]
        public EnumProductFilterSort? SortFilter { get; set; }

        public string ConvertSort()
        {
            if (SortFilter == null) return string.Empty;
            var sort = SortFilter.ToString();
            sort = SortDirection ? $"{sort} asc" : $"{sort} desc";
            return sort;
        }
    }
}