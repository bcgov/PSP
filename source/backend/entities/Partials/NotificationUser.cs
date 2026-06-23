using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Catalogs the notification sent to a user.  Multiple notifications may be sent to a user.
    /// </summary>
    public partial class PimsNotificationUser : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => NotificationUserId; set => NotificationUserId = value; }
    }
}
