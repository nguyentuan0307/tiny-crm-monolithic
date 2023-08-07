using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TinyCRM.Application.Models.Contact;

public class ContactSearchDto : EntitySearchDto
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [EnumDataType(typeof(EnumContactFilterSort))]
    public EnumContactFilterSort? SortFilter { get; set; }

    public string ConvertSort()
    {
        if (SortFilter == null) return string.Empty;
        var sort = SortFilter.ToString();
        if (sort == EnumContactFilterSort.AccountName.ToString()) sort = "Account.Name";
        sort = SortDirection ? $"{sort} asc" : $"{sort} desc";
        return sort;
    }
}