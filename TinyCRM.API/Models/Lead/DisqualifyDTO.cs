﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TinyCRM.Domain.Enums;

namespace TinyCRM.API.Models.Lead
{
    public class DisqualifyDTO
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [EnumDataType(typeof(ReasonDisqualification))]
        public ReasonDisqualification ReasonDisqualification { get; set; }

        public string? DescriptionDisqualification { get; set; }
    }
}