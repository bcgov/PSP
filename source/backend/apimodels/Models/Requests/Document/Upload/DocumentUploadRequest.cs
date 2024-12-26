using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Pims.Api.Models.Concepts.Document;

namespace Pims.Api.Models.Requests.Document.Upload
{
    /// <summary>
    /// DocumentUploadRequest class, represents a request to upload a document.
    /// </summary>
    public class DocumentUploadRequest
    {
        #region Properties

        /// <summary>
        /// get/set - The mayan document type for the document to be uploaded.
        /// </summary>
        public long DocumentTypeMayanId { get; set; }

        /// <summary>
        /// get/set - The type for the document to be uploaded.
        /// </summary>
        public long DocumentTypeId { get; set; }

        /// <summary>
        /// get/set - The id of the document to be uploaded (in PIMS).
        /// </summary>
        public long DocumentId { get; set; }

        /// <summary>
        /// get/set - Initial status code of the document.
        /// </summary>
        public string DocumentStatusCode { get; set; }

        /// <summary>
        /// get/set - The document to be uploaded.
        /// </summary>
        public IFormFile File { get; set; }

        /// <summary>
        /// get/set - The metadata associated with document.
        /// </summary>
        public List<DocumentMetadataUpdateModel> DocumentMetadata { get; set; }
        #endregion
    }
}
