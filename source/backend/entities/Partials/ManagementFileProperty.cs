using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsManagementFileProperty class, provides an entity for the datamodel to manage the relationship between the Management Files Properties.
    /// </summary>
    public partial class PimsManagementFileProperty : StandardIdentityBaseAppEntity<long>, IBaseAppEntity, IFilePropertyEntity
    {
        [NotMapped]
        public override long Internal_Id { get => this.ManagementFilePropertyId; set => this.ManagementFilePropertyId = value; }
    }
}
