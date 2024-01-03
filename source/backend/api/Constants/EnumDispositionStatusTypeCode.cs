using System.Runtime.Serialization;

namespace Pims.Api.Constants
{
    public enum EnumDispositionStatusTypeCode
    {
        [EnumMember(Value = "LISTED")]
        LISTED,

        [EnumMember(Value = "ONHOLD")]
        ONHOLD,

        [EnumMember(Value = "PENDING")]
        PENDING,

        [EnumMember(Value = "PREMARKET")]
        PREMARKET,

        [EnumMember(Value = "SOLD")]
        SOLD,

        [EnumMember(Value = "UNKNOWN")]
        UNKNOWN,
    }
}
