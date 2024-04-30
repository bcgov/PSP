using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum TakeTypes
    {
        [EnumMember(Value = "TOTAL")]
        TOTAL,

        [EnumMember(Value = "PARTIAL")]
        PARTIAL,
    }
}
