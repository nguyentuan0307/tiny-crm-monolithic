﻿using System.ComponentModel.DataAnnotations;

namespace TinyCRM.API.Models
{
    public abstract class EntitySearchDTO
    {
        [StringLength(100, ErrorMessage = "Keyword cannot exceed 100 characters.")]
        public string? KeyWord { get; set; } = string.Empty;
        [Range(1, int.MaxValue, ErrorMessage = "PageIndex must be a non-negative number.")]
        public int PageIndex { get; set; } = 0;
        [Range(1, int.MaxValue, ErrorMessage = "PageSize must be a positive number.")]
        public int PageSize { get; set; } = int.MaxValue;
        [StringLength(100, ErrorMessage = "KeySort cannot exceed 100 characters.")]
        public string? KeySort { get; set; }
        public bool IsAsc { get; set; } = true;
    }
}