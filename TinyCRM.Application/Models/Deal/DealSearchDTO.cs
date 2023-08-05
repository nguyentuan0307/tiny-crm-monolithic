using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TinyCRM.Application.Models.Deal
{
    public class DealSearchDto : EntitySearchDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [EnumDataType(typeof(EnumDealFilterSort))]
        public EnumDealFilterSort? SortFilter { get; set; }

        public string ConvertSort()
        {
            if (SortFilter == null) return string.Empty;
            var sort = SortFilter.ToString();
            sort = SortDirection ? $"{sort} asc" : $"{sort} desc";
            return sort;
        }
    }
}