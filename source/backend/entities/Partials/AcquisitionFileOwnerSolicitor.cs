using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAcquisitionOwnerSolicitor class, provides an entity for the datamodel to manage the relationship between Person (Solicitor) and Acquisition Files.
    /// </summary>
    public partial class PimsAcquisitionOwnerSolicitor : StandardIdentityBaseAppEntity<long>, IDisableBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.OwnerSolicitorId; set => this.OwnerSolicitorId = value; }
        #endregion
    }
}
