using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsNotification partial class for custom logic and inheritance.
    /// </summary>
    public partial class PimsNotification : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => NotificationId; set => NotificationId = value; }
    }
}
