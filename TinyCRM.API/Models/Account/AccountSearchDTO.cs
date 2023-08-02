using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TinyCRM.API.Models.Account
{
    public class AccountSearchDto : EntitySearchDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [EnumDataType(typeof(EnumAccountFilterSort))]
        public EnumAccountFilterSort? SortFilter { get; set; }

        public string ConvertSort()
        {
            if (SortFilter == null) return string.Empty;
            var sort = SortFilter.ToString();
            sort = SortDirection ? $"{sort} asc" : $"{sort} desc";
            return sort;
        }
    }
}