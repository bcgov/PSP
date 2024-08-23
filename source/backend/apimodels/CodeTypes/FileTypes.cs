using System.Runtime.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    public enum FileTypes
    {
        [EnumMember(Value = "acquisition")]
        Acquisition,

        [EnumMember(Value = "research")]
        Research,

        [EnumMember(Value = "disposition")]
        Disposition,

        [EnumMember(Value = "lease")]
        Lease,
    }
}
