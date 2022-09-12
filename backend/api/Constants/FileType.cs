using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Constants
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum FileType
    {
        [EnumMember(Value = "acquisition")]
        Acquisition,
        [EnumMember(Value = "research")]
        Research,
    }
}
