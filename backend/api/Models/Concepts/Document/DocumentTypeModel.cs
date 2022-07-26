namespace Pims.Api.Models.Concepts
{
    /// <summary>
    /// DocumentTypeModel class, provides a model to represent document associated to entities.
    /// </summary>
    public class DocumentTypeModel : BaseAppModel
    {

        #region Properties

        /// <summary>
        /// get/set - Document Type Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - The document Type description.
        /// </summary>
        public string DocumentType { get; set; }

        #endregion
    }
}
