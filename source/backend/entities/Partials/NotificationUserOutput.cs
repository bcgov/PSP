using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsNotificationUserOutput describes the details of the notification sent the user including the sent date and read date.
    /// </summary>
    public partial class PimsNotificationUserOutput : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => NotificationUserOutputId; set => NotificationUserOutputId = value; }
    }
}
