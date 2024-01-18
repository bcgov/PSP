using System.Collections.Generic;
using Pims.Api.Models.Concepts.Document;

using Pims.Api.Models.Requests.Http;

namespace Pims.Api.Models.Requests.Document.Upload
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
        public ExternalResponse<Mayan.Document.DocumentDetailModel> DocumentExternalResponse { get; set; }

        /// <summary>
        /// get/set - Response with metadata from the file storage.
        /// </summary>
        public List<ExternalResponse<Mayan.Document.DocumentMetadataModel>> MetadataExternalResponse { get; set; }
        #endregion
    }
}
