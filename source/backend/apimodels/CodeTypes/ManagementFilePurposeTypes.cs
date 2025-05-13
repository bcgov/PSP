using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum ManagementFilePurposeTypes
    {
        [EnumMember(Value = "AGRICULT")]
        AGRICULT,
        [EnumMember(Value = "BCFERRY")]
        BCFERRY,
        [EnumMember(Value = "BCTRANS")]
        BCTRANS,
        [EnumMember(Value = "COMMBLDG")]
        COMMBLDG,
        [EnumMember(Value = "ENCAMP")]
        ENCAMP,
        [EnumMember(Value = "ENGINEER")]
        ENGINEER,
        [EnumMember(Value = "GENERAL")]
        GENERAL,
        [EnumMember(Value = "GOVERNMT")]
        GOVERNMT,
        [EnumMember(Value = "MOTTUSE")]
        MOTTUSE,
        [EnumMember(Value = "OILGAS")]
        OILGAS,
        [EnumMember(Value = "OTHER")]
        OTHER,
        [EnumMember(Value = "PARKING")]
        PARKING,
        [EnumMember(Value = "RAIL")]
        RAIL,
        [EnumMember(Value = "RESRENTL")]
        RESRENTL,
        [EnumMember(Value = "TRAILS")]
        TRAILS,
        [EnumMember(Value = "UTILITY")]
        UTILITY,
    }
}
