namespace Pims.Api.Models.Concepts
{
    /// <summary>
    /// DocumentUploadResponse class, represents the response to upload a document.
    /// </summary>
    public class DocumentUploadResponse
    {
        #region Properties

        /// <summary>
        /// get/set - The new document relationship created.
        /// </summary>
        public DocumentRelationshipModel DocumentRelationship { get; set; }

        /// <summary>
        /// get/set - External response from the file storage.
        /// </summary>
        public ExternalResult<Mayan.Document.DocumentDetail> ExternalResult { get; set; }
        #endregion
    }
}
