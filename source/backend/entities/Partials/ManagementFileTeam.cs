using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsManagementFileTeam class, provides an entity for the datamodel to manage the relationship between the Management Files and it's team.
    /// </summary>
    public partial class PimsManagementFileTeam : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.PimsManagementFileTeamId; set => this.PimsManagementFileTeamId = value; }
        #endregion
    }
}
