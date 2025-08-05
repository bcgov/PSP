using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum InterestHolderTypes
    {
        [EnumMember(Value = "AOREP")]
        AOREP,

        [EnumMember(Value = "AOSLCTR")]
        AOSLCTR,

        [EnumMember(Value = "INTHLDR")]
        INTHLDR,
    }
}
