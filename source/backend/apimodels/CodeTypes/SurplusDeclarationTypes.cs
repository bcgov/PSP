using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum SurplusDeclarationTypes
    {
        [EnumMember(Value = "EXPIRED")]
        EXPIRED,

        [EnumMember(Value = "NO")]
        NO,

        [EnumMember(Value = "UNKNOWN")]
        UNKNOWN,

        [EnumMember(Value = "YES")]
        YES,
    }
}
