using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsNotificationUserOutput partial class for custom logic and inheritance.
    /// </summary>
    public partial class PimsNotificationUserOutput : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => NotificationUserOutputId; set => NotificationUserOutputId = value; }
    }
}
