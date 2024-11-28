using System.Collections.Generic;
using Pims.Api.Models.Concepts.Document;

namespace Pims.Api.Models.Requests.Document.UpdateMetadata
{
    /// <summary>
    /// DocumentUpdateRequest class, represents a response to update a document.
    /// </summary>
    public class DocumentUpdateRequest
    {
        #region Properties

        /// <summary>
        /// get/set - The pims document id.
        /// </summary>
        public long DocumentId { get; set; }

        /// <summary>
        /// get/set - The mayan document id.
        /// </summary>
        public long MayanDocumentId { get; set; }

        /// <summary>
        /// get/set - The type for the document to be uploaded.
        /// </summary>
        public long DocumentTypeId { get; set; }

        /// <summary>
        /// get/set - Status code of the document.
        /// </summary>
        public string DocumentStatusCode { get; set; }

        /// <summary>
        /// get/set - The metadata associated with document.
        /// </summary>
        public List<DocumentMetadataUpdateModel> DocumentMetadata { get; set; }
        #endregion
    }
}
