﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TinyCRM.API.Models.Deal
{
    public class DealSearchDto : EntitySearchDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [EnumDataType(typeof(EnumDealFilterSort))]
        public EnumDealFilterSort SortFilter { get; set; } = EnumDealFilterSort.Id;
    }
}