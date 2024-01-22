using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum AcquisitionStatusTypes
    {

        [EnumMember(Value = "ACTIVE")]
        ACTIVE,

        [EnumMember(Value = "ARCHIV")]
        ARCHIV,

        [EnumMember(Value = "CANCEL")]
        CANCEL,

        [EnumMember(Value = "CLOSED")]
        CLOSED,

        [EnumMember(Value = "COMPLT")]
        COMPLT,

        [EnumMember(Value = "DRAFT")]
        DRAFT,

        [EnumMember(Value = "HOLD")]
        HOLD,
    }
}
