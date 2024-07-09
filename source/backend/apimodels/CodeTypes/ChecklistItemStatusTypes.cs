using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum ChecklistItemStatusTypes
    {
        [EnumMember(Value = "COMPLT")]
        COMPLT,

        [EnumMember(Value = "INCOMP")]
        INCOMP,

        [EnumMember(Value = "NOTAPP")]
        NOTAPP,
    }
}
