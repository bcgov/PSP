using System.Runtime.Serialization;

namespace Pims.Dal.Entities.Enums
{
    public enum NotificationOutputTypeCode
    {
        [EnumMember(Value = "EMAIL")]
        EMAIL,
        [EnumMember(Value = "PIMS")]
        PIMS,
    }
}