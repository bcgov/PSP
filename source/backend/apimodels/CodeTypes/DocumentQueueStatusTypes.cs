using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum DocumentQueueStatusTypes
    {
        [EnumMember(Value = "MAYAN_ERROR")]
        MAYAN_ERROR,

        [EnumMember(Value = "PENDING")]
        PENDING,

        [EnumMember(Value = "PIMS_ERROR")]
        PIMS_ERROR,

        [EnumMember(Value = "PROCESSING")]
        PROCESSING,

        [EnumMember(Value = "SUCCESS")]
        SUCCESS,
    }
}
