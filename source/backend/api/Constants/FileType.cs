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
        [EnumMember(Value = "project")]
        Project,
        [EnumMember(Value = "lease")]
        Lease,
        [EnumMember(Value = "unknown")] // Used in tests/logic only. This does not correspond to a valid file type in the db.
        Unknown,
    }
}
