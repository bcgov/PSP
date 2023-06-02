using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAcquisitionOwnerRep class, provides an entity for the datamodel to manage the relationship between Person (Representative) and Acquisition Files.
    /// </summary>
    public partial class PimsAcquisitionOwnerRep : StandardIdentityBaseAppEntity<long>, IDisableBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.OwnerRepresentativeId; set => this.OwnerRepresentativeId = value; }
        #endregion
    }
}
