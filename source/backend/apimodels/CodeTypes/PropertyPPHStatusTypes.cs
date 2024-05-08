using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum PropertyPPHStatusTypes
    {
        [EnumMember(Value = "ARTERY")]
        ARTERY,

        [EnumMember(Value = "COMBO")]
        COMBO,

        [EnumMember(Value = "NONPPH")]
        NONPPH,

        [EnumMember(Value = "PPH")]
        PPH,

        [EnumMember(Value = "UNKNOWN")]
        UNKNOWN,
    }
}
