using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsLeaseConsultation class, provides an entity for the datamodel to manage lease consulations.
    /// </summary>
    public partial class PimsLeaseConsultation : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.LeaseConsultationId; set => this.LeaseConsultationId = value; }
        #endregion
    }
}
