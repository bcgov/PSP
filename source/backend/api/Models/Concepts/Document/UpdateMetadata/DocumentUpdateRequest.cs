using System.Collections.Generic;

namespace Pims.Api.Models.Concepts
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
