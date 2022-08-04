
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
        /// get/set - Document Type.
        /// </summary>
        public DocumentTypeModel DocumentType { get; set; }

        /// <summary>
        /// get/set - The document status type.
        /// </summary>
        public TypeModel<string> DocumentStatusTypeCode { get; set; }

        /// <summary>
        /// get/set - Document/File Name.
        /// </summary>
        public string FileName { get; set; }
        #endregion
    }
}
