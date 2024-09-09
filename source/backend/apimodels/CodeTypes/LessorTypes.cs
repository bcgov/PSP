using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum LessorTypes
    {
        [EnumMember(Value = "ORG")]
        ORG,

        [EnumMember(Value = "PER")]
        PER,

        [EnumMember(Value = "UNK")]
        UNK,
    }
}
