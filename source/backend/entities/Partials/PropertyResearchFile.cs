using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropertyResearchFile class, provides an entity for the datamodel to manage the relationship between Properties and Research Files.
    /// </summary>
    public partial class PimsPropertyResearchFile : StandardIdentityBaseAppEntity<long>, IDisableBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.PropertyResearchFileId; set => this.PropertyResearchFileId = value; }
        #endregion
    }
}
