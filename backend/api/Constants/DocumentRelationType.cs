using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Constants
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum DocumentRelationType
    {
        [EnumMember(Value = "activities")]
        Activities,
        [EnumMember(Value = "files")]
        Files,
    }
}
