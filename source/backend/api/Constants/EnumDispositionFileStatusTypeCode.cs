using System.Runtime.Serialization;

namespace Pims.Api.Constants
{
    public enum EnumDispositionFileStatusTypeCode
    {
        [EnumMember(Value = "ACTIVE")]
        ACTIVE,

        [EnumMember(Value = "ARCHIVED")]
        ARCHIVED,

        [EnumMember(Value = "CANCELLED")]
        CANCELLED,

        [EnumMember(Value = "COMPLETE")]
        COMPLETE,

        [EnumMember(Value = "DRAFT")]
        DRAFT,

        [EnumMember(Value = "HOLD")]
        HOLD,
    }
}
