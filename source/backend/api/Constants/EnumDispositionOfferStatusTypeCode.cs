using System.Runtime.Serialization;

namespace Pims.Api.Constants
{
    public enum EnumDispositionOfferStatusTypeCode
    {
        [EnumMember(Value = "ACCCEPTED")]
        ACCCEPTED,

        [EnumMember(Value = "COLLAPSED")]
        COLLAPSED,

        [EnumMember(Value = "COUNTERED")]
        COUNTERED,

        [EnumMember(Value = "OPEN")]
        OPEN,

        [EnumMember(Value = "REJECTED")]
        REJECTED,
    }
}
