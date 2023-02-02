using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Constants
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum CacheKeys
    {
        [EnumMember(Value = "lookup")]
        Lookup,
    }
}
