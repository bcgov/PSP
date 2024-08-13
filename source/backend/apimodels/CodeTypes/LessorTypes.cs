using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum LessorTypes
    {
        [EnumMember(Value = "Organization")]
        ORG,

        [EnumMember(Value = "Person")]
        PER,

        [EnumMember(Value = "Unknown")]
        UNK,
    }
}
