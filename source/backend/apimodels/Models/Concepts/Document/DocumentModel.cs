using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.Document
{
    /// <summary>
    /// DocumentModel class, provides a model to represent document associated to entities.
    /// </summary>
    public class DocumentModel : BaseAuditModel
    {

        #region Properties

        /// <summary>
        /// get/set - Document Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - The document id on the external storage.
        /// </summary>
        public int? MayanDocumentId { get; set; }

        /// <summary>
        /// get/set - Document Type.
        /// </summary>
        public DocumentTypeModel DocumentType { get; set; }

        /// <summary>
        /// get/set - The document status type.
        /// </summary>
        public CodeTypeModel<string> StatusTypeCode { get; set; }

        /// <summary>
        /// get/set - Document/File Name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// get/set - The document queue status type.
        /// </summary>
        public CodeTypeModel<string> DocumentQueueStatusTypeCode { get; set; }
        #endregion
    }
}
