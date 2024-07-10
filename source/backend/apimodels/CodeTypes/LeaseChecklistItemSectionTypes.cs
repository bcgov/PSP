using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum LeaseChecklistItemSectionTypes
    {
        [EnumMember(Value = "AGREEPREP")]
        AGREEPREP,

        [EnumMember(Value = "FILEINIT")]
        FILEINIT,

        [EnumMember(Value = "LLCOMPLTN")]
        LLCOMPLTN,

        [EnumMember(Value = "REFERAPPR")]
        REFERAPPR,
    }
}
