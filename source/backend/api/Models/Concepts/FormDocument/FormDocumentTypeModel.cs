namespace Pims.Api.Models.Concepts
{
    /// <summary>
    /// FormDocumentTypeModel class, provides a model to represent form document types.
    /// </summary>
    public class FormDocumentTypeModel : BaseAppModel
    {
        /// <summary>
        /// get/set - Form type code.
        /// </summary>
        public string FormTypeCode { get; set; }

        /// <summary>
        /// get/set - Form type document id.
        /// </summary>
        public long? DocumentId { get; set; }

        /// <summary>
        /// get/set - Form type description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - Display order of the type.
        /// </summary>
        public long? DisplayOrder { get; set; }
    }
}
