using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsNotificationUser partial class for custom logic and inheritance.
    /// </summary>
    public partial class PimsNotificationUser : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => NotificationUserId; set => NotificationUserId = value; }
    }
}
