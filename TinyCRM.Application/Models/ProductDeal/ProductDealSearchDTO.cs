using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TinyCRM.Application.Models.ProductDeal;

public class ProductDealSearchDto : EntitySearchDto
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [EnumDataType(typeof(EnumProductDealFilterSort))]
    public EnumProductDealFilterSort? SortFilter { get; set; }

    public string ConvertSort()
    {
        if (SortFilter == null) return string.Empty;
        var sort = SortFilter.ToString();
        if (sort == EnumProductDealFilterSort.ProductCode.ToString()) sort = "Product.Code";
        if (sort == EnumProductDealFilterSort.ProductName.ToString()) sort = "Product.Name";
        sort = SortDirection ? $"{sort} asc" : $"{sort} desc";
        return sort;
    }
}