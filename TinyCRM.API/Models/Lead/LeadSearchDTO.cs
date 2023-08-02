using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TinyCRM.API.Models.Lead
{
    public class LeadSearchDto : EntitySearchDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [EnumDataType(typeof(EnumLeadFilterSort))]
        public EnumLeadFilterSort? SortFilter { get; set; }

        public string ConvertSort()
        {
            if (SortFilter == null) return string.Empty;
            var sort = SortFilter.ToString();
            if (sort == EnumLeadFilterSort.AccountName.ToString()) sort = "Account.Name";
            sort = SortDirection ? $"{sort} asc" : $"{sort} desc";
            return sort;
        }
    }
}