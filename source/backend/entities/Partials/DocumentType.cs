using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDocument class, provides an entity for the datamodel to manage notes.
    /// </summary>
    public partial class PimsDocumentTyp : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.DocumentTypeId; set => this.DocumentTypeId = value; }
        #endregion
    }
}
