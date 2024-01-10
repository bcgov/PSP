using Pims.Api.Models.Base;
using Pims.Api.Models.CodeTypes;

namespace Pims.Api.Models.Concepts.Document
{
    /// <summary>
    /// DocumentRelationshipModel class, provides a model to represent document relationships.
    /// </summary>
    public class DocumentRelationshipModel : BaseAuditModel
    {
        #region Properties

        /// <summary>
        /// get/set - The id of the relationship.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - Id of the other entity in the relationship (i.e. activity-id, file-id, etc).
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// get/set - The document that is linked though this relationship.
        /// </summary>
        public DocumentModel Document { get; set; }

        /// <summary>
        /// get/set - The relationship type.
        /// </summary>
        public DocumentRelationType RelationshipType { get; set; }
        #endregion
    }
}
