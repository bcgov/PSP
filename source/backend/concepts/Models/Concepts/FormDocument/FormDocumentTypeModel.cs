using Pims.Api.Concepts.Models.Base;

namespace Pims.Api.Concepts.Models.Concepts.FormDocument
{
    /// <summary>
    /// FormDocumentTypeModel class, provides a model to represent form document types.
    /// </summary>
    public class FormDocumentTypeModel : BaseAuditModel
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
