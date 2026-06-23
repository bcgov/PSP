using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsNotificationType class, provides an entity for the datamodel to manage notification types.
    /// </summary>
    public partial class PimsNotificationType : ITypeEntity<string>
    {
        [NotMapped]
        public string Id { get => NotificationTypeCode; set => NotificationTypeCode = value; }
    }
}
