using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]

    public enum DispositionStatusTypes
    {
        [EnumMember(Value = "LISTED")]
        LISTED,

        [EnumMember(Value = "ONHOLD")]
        ONHOLD,

        [EnumMember(Value = "PENDING")]
        PENDING,

        [EnumMember(Value = "PREMARKET")]
        PREMARKET,

        [EnumMember(Value = "SOLD")]
        SOLD,

        [EnumMember(Value = "UNKNOWN")]
        UNKNOWN,
    }
}
