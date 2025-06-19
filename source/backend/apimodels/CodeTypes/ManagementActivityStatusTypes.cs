using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum ManagementActivityStatusTypes
    {
        [EnumMember(Value = "CANCELLED")]
        CANCELLED,

        [EnumMember(Value = "COMPLETED")]
        COMPLETED,

        [EnumMember(Value = "INPROGRESS")]
        INPROGRESS,

        [EnumMember(Value = "NOTSTARTED")]
        NOTSTARTED,

        [EnumMember(Value = "ONHOLD")]
        ONHOLD,
    }
}
