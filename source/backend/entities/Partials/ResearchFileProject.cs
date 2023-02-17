using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsResearchFileProject class, provides an entity for the datamodel to manage the relationship between Projects and Research Files.
    /// </summary>
    public partial class PimsResearchFileProject : StandardIdentityBaseAppEntity<long>, IDisableBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.ResearchFileProjectId; set => this.ResearchFileProjectId = value; }
        #endregion
    }
}
