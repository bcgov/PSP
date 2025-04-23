using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum ManagementFileStatusTypes
    {
        [EnumMember(Value = "ACTIVE")]
        ACTIVE,

        [EnumMember(Value = "ARCHIVED")]
        ARCHIVED,

        [EnumMember(Value = "CANCELLED")]
        CANCELLED,

        [EnumMember(Value = "COMPLETE")]
        COMPLETE,

        [EnumMember(Value = "DRAFT")]
        DRAFT,

        [EnumMember(Value = "HOLD")]
        HOLD,

        [EnumMember(Value = "3RDPARTY")]
        THIRDRDPARTY,
    }
}
