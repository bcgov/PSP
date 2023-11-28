using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDispositionFileTeam class, provides an entity for the datamodel to manage the relationship between the Disposition Files and it's team.
    /// </summary>
    public partial class PimsDispositionFileTeam : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.DispositionFileTeamId; set => this.DispositionFileTeamId = value; }
        #endregion
    }
}
