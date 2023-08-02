using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TinyCRM.API.Models.User
{
    public class ProfileUserSearchDto : EntitySearchDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [EnumDataType(typeof(EnumProfileUserFilterSort))]
        public EnumProfileUserFilterSort? SortFilter { get; set; }

        public string ConvertSort()
        {
            if (SortFilter == null) return string.Empty;
            var sort = SortFilter.ToString();
            sort = SortDirection ? $"{sort} asc" : $"{sort} desc";
            return sort;
        }
    }
}