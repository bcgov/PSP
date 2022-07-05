
namespace Pims.Api.Models.Concepts
{
    /// <summary>
    /// DocumentModel class, provides a model to represent document associated to entities.
    /// </summary>
    public class DocumentModel : BaseAppModel
    {

        #region Properties

        /// <summary>
        /// get/set - Document Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - Document Type Id.
        /// </summary>
        public string DocumentTypeId { get; set; }

        /// <summary>
        /// get/set - Document Type.
        /// </summary>
        public string DocumentType { get; set; }

        /// <summary>
        /// get/set - Document Status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// get/set - Document Status Code.
        /// </summary>
        public string StatusCode { get; set; }

        /// <summary>
        /// get/set - Document/File Name.
        /// </summary>
        public string FileName { get; set; }
        #endregion
    }
}
