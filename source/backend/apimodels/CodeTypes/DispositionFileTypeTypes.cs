using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum DispositionFileTypeTypes
    {
        [EnumMember(Value = "CLOSURE")]
        CLOSURE,

        [EnumMember(Value = "DIRECT")]
        DIRECT,

        [EnumMember(Value = "OPEN")]
        OPEN,

        [EnumMember(Value = "OTHER")]
        OTHER,

        [EnumMember(Value = "SRW")]
        SRW,
    }
}
