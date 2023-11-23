using System.Collections.Generic;
using Pims.Api.Concepts.Models.Concepts.Http;

namespace Pims.Api.Concepts.Models.Concepts.Document.UpdateMetadata
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
        public List<ExternalResult<Mayan.Document.DocumentMetadata>> MetadataExternalResult { get; set; }
        #endregion
    }
}
