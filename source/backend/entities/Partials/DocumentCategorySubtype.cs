using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDocumentCategorySubtype class, provides an entity for the datamodel to manage document categories subtypes.
    /// </summary>
    public partial class PimsDocumentCategorySubtype : StandardIdentityBaseAppEntity<long>
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.DocumentCategorySubtypeId; set => this.DocumentCategorySubtypeId = value; }
        #endregion
    }
}
