using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsManagementFileContact class, provides an entity for the datamodel to manage Management File contacts.
    /// </summary>
    public partial class PimsManagementFileContact : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => ManagementFileContactId; set => ManagementFileContactId = value; }
    }
}
