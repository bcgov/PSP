using System.Collections.Generic;
using Pims.Api.Concepts.Models.Concepts.Http;

namespace Pims.Api.Concepts.Models.Concepts.Document.Upload
{
    /// <summary>
    /// DocumentUploadResponse class, represents the response to upload a document.
    /// </summary>
    public class DocumentUploadResponse
    {
        #region Properties

        /// <summary>
        /// get/set - The new document created.
        /// </summary>
        public DocumentModel Document { get; set; }

        /// <summary>
        /// get/set - Response with document information from the file storage.
        /// </summary>
        public ExternalResult<Mayan.Document.DocumentDetail> DocumentExternalResult { get; set; }

        /// <summary>
        /// get/set - Response with metadata from the file storage.
        /// </summary>
        public List<ExternalResult<Mayan.Document.DocumentMetadata>> MetadataExternalResult { get; set; }
        #endregion
    }
}
