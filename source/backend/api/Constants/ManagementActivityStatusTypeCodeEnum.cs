using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Constants
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum ManagementActivityStatusTypeCode
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
