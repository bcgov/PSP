using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum ResearchFileStatusTypes
    {
        [EnumMember(Value = "ACTIVE")]
        ACTIVE,

        [EnumMember(Value = "ARCHIVED")]
        ARCHIVED,

        [EnumMember(Value = "CLOSED")]
        CLOSED,

        [EnumMember(Value = "INACTIVE")]
        INACTIVE,
    }
}
