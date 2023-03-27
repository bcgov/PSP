using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsResearchFileNote class, provides an entity for the datamodel to manage research file notes.
    /// </summary>
    public partial class PimsResearchFileNote : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.ResearchFileNoteId; set => this.ResearchFileNoteId = value; }
        #endregion
    }
}
