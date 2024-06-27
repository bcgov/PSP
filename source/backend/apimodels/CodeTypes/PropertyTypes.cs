using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum PropertyTypes
    {
        [EnumMember(Value = "CROWNFSRVD")]
        CROWNFSRVD,

        [EnumMember(Value = "CROWNFUSRVD")]
        CROWNFUSRVD,

        [EnumMember(Value = "CROWNPSRVD")]
        CROWNPSRVD,

        [EnumMember(Value = "CROWNPUSRVD")]
        CROWNPUSRVD,

        [EnumMember(Value = "HWYROAD")]
        HWYROAD,

        [EnumMember(Value = "PARKS")]
        PARKS,

        [EnumMember(Value = "RESERVE")]
        RESERVE,

        [EnumMember(Value = "STRATACP")]
        STRATACP,

        [EnumMember(Value = "TITLED")]
        TITLED,

        [EnumMember(Value = "UNKNOWN")]
        UNKNOWN,
    }
}
