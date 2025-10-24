using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum LeaseProgramTypes
    {
        [EnumMember(Value = "AGRIC")]
        AGRIC,

        [EnumMember(Value = "COMMBLDG")]
        COMMBLDG,

        [EnumMember(Value = "ENGINEER")]
        ENGINEER,

        [EnumMember(Value = "LCLGOVT")]
        LCLGOVT,

        [EnumMember(Value = "MOTIUSE")]
        MOTIUSE,

        [EnumMember(Value = "OTHER")]
        OTHER,

        [EnumMember(Value = "PARKING")]
        PARKING,

        [EnumMember(Value = "PUBTRANS")]
        PUBTRANS,

        [EnumMember(Value = "RAIL")]
        RAIL,

        [EnumMember(Value = "RAILTRAIL")]
        RAILTRAIL,

        [EnumMember(Value = "RESRENTAL")]
        RESRENTAL,

        [EnumMember(Value = "TMEP")]
        TMEP,

        [EnumMember(Value = "UTILITY")]
        UTILITY,
    }
}
