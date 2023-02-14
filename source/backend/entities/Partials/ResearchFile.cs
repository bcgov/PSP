using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsResearchFile class, provides an entity for the datamodel to manage research files.
    /// </summary>
    public partial class PimsResearchFile : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.ResearchFileId; set => this.ResearchFileId = value; }
        #endregion
    }
}
