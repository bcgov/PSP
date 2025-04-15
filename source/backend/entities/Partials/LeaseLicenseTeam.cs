using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsLeaseLicenseTeam class, provides an entity for the datamodel to manage the relationship between the Lease Files and it's team.
    /// </summary>
    public partial class PimsLeaseLicenseTeam : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.LeaseLicenseTeamId; set => this.LeaseLicenseTeamId = value; }
        #endregion
    }
}
