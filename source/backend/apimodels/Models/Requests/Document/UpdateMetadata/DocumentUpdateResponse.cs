using System.Collections.Generic;
using Pims.Api.Models.Requests.Http;

namespace Pims.Api.Models.Requests.Document.UpdateMetadata
{
    /// <summary>
    /// DocumentUpdateResponse class, represents a response to update a document.
    /// </summary>
    public class DocumentUpdateResponse
    {
        #region Properties

        /// <summary>
        /// get/set - Response with metadata from the file storage.
        /// </summary>
        public List<ExternalResponse<Mayan.Document.DocumentMetadataModel>> MetadataExternalResponse { get; set; }
        #endregion
    }
}
