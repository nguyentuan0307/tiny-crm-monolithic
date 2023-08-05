using System.ComponentModel.DataAnnotations;

namespace TinyCRM.Application.Models
{
    public abstract class EntitySearchDto
    {
        [StringLength(100, ErrorMessage = "Keyword cannot exceed 100 characters.")]
        public string? KeyWord { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "PageIndex must be a non-negative number.")]
        public int PageIndex { get; set; } = 1;

        [Range(1, int.MaxValue, ErrorMessage = "PageSize must be a positive number.")]
        public int PageSize { get; set; } = int.MaxValue;

        public bool SortDirection { get; set; } = true;
    }
}