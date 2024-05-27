using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]

    public enum LeaseStatusTypes
    {
        [EnumMember(Value = "ACTIVE")]
        ACTIVE,

        [EnumMember(Value = "ARCHIVED")]
        ARCHIVED,

        [EnumMember(Value = "DISCARD")]
        DISCARD,

        [EnumMember(Value = "DRAFT")]
        DRAFT,

        [EnumMember(Value = "DUPLICATE")]
        DUPLICATE,

        [EnumMember(Value = "EXPIRED")]
        EXPIRED,

        [EnumMember(Value = "INACTIVE")]
        INACTIVE,

        [EnumMember(Value = "TERMINATED")]
        TERMINATED,
    }
}
