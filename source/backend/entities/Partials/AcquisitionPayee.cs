using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAcquistionPayee class, provides an entity for the datamodel to manage compensation payees.
    /// </summary>
    public partial class PimsAcquisitionPayee : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.AcquisitionPayeeId; set => this.AcquisitionPayeeId = value; }
        #endregion
    }
}
