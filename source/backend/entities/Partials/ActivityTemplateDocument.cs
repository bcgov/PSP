using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsActivityTemplateDocument class, provides an entity for the datamodel to manage Activity template document entities.
    /// </summary>
    public partial class PimsActivityTemplateDocument : IdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Id { get => this.ActivityTemplateDocumentId; set => this.ActivityTemplateDocumentId = value; }
        #endregion
    }
}
