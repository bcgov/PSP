using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Pims.Api.Models.Concepts
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
