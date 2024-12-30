using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.Document
{
    /// <summary>
    /// DocumentTypeModel class, provides a model to represent document associated to entities.
    /// </summary>
    public class DocumentTypeModel : BaseAuditModel
    {

        #region Properties

        /// <summary>
        /// get/set - Document type Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - The document type description.
        /// </summary>
        public string DocumentType { get; set; }

        /// <summary>
        /// get/set - The document type description.
        /// </summary>
        public string DocumentTypeDescription { get; set; }

        /// <summary>
        /// get/set - The document type purpose.
        /// </summary>
        public string DocumentTypePurpose { get; set; }

        /// <summary>
        /// get/set - The document type id in mayan.
        /// </summary>
        public long? MayanId { get; set; }

        /// <summary>
        /// get/set - The document type is disabled and is maintained for reference only.
        /// </summary>
        public bool IsDisabled { get; set; }

        #endregion
    }
}
