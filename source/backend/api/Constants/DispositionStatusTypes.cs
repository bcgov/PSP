using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Constants
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum DispositionStatusTypes
    {
        [EnumMember(Value = "ACTIVE")]
        ACTIVE,

        [EnumMember(Value = "ARCHIVED")]
        ARCHIVED,

        [EnumMember(Value = "CANCELLED")]
        CANCELLED,

        [EnumMember(Value = "CLOSED")]
        CLOSED,

        [EnumMember(Value = "COMPLETE")]
        COMPLETE,

        [EnumMember(Value = "DRAFT")]
        DRAFT,

        [EnumMember(Value = "HOLD")]
        HOLD,
    }
}
