using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum AreaUnitTypes
    {
        [EnumMember(Value = "ACRE")]
        ACRE,

        [EnumMember(Value = "FEET2")]
        FEET2,

        [EnumMember(Value = "HA")]
        HA,

        [EnumMember(Value = "M2")]
        M2,
    }
}
