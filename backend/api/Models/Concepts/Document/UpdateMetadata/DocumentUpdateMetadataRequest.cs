using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Pims.Api.Models.Concepts
{
    /// <summary>
    /// DocumentUpdateMetadataRequest class, represents a request to update a document metadata.
    /// </summary>
    public class DocumentUpdateMetadataRequest
    {
        #region Properties

        /// <summary>
        /// get/set - The document id.
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
        /// get/set - Initial status code of the document.
        /// </summary>
        public string DocumentStatusCode { get; set; }

        /// <summary>
        /// get/set - The metadata associated with document.
        /// </summary>
        public List<DocumentMetadataUpdateModel> DocumentMetadata { get; set; }
        #endregion
    }
}
