using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum LeaseChecklistItemStatusTypes
    {
        [EnumMember(Value = "COMPLT")]
        COMPLT,

        [EnumMember(Value = "INCOMP")]
        INCOMP,

        [EnumMember(Value = "NOTAPP")]
        NOTAPP,
    }
}
