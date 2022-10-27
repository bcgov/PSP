namespace Pims.Api.Models.Concepts
{
    /// <summary>
    /// DocumentUploadRelationshipResponse class, represents the response to upload a document.
    /// </summary>
    public class DocumentUploadRelationshipResponse
    {
        #region Properties

        /// <summary>
        /// get/set - The new document relationship created.
        /// </summary>
        public DocumentRelationshipModel DocumentRelationship { get; set; }

        /// <summary>
        /// get/set - The new document upload response.
        /// </summary>
        public DocumentUploadResponse UploadResponse { get; set; }
        #endregion
    }
}
