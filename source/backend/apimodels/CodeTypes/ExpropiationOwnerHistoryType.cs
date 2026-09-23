using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum ExpropiationOwnerHistoryType
    {
        [EnumMember(Value = "ADVPMTSRVDDT")]
        ADVPMTSRVDDT,

        [EnumMember(Value = "APPEFFCTVDT")]
        APPEFFCTVDT,

        [EnumMember(Value = "CERTEXPRAPPR")]
        CERTEXPRAPPR,

        [EnumMember(Value = "EXPRVSTNGDT")]
        EXPRVSTNGDT,

        [EnumMember(Value = "NOTCSRVDDT")]
        NOTCSRVDDT,
    }
}
