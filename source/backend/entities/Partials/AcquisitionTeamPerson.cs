using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAcquisitionFileTeam class, provides an entity for the datamodel to manage the relationship between the Acquisition Files and it's team.
    /// </summary>
    public partial class PimsAcquisitionFileTeam : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.AcquisitionFileTeamId; set => this.AcquisitionFileTeamId = value; }
        #endregion
    }
}
