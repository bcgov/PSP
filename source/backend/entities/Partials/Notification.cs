using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Describes the user notification and related field type (e.g. acquisition, disposition, management) provided to the user.
    /// </summary>
    public partial class PimsNotification : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => NotificationId; set => NotificationId = value; }
    }
}
