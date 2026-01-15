using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]

    public enum PropertyImprovementTypes
    {
        [EnumMember(Value = "RTA")]
        RTA,

        [EnumMember(Value = "OTHER")]
        OTHER,

        [EnumMember(Value = "COMMBLDG")]
        COMMBLDG,
    }
}
