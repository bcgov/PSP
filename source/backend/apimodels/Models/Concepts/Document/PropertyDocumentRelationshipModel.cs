using Pims.Api.Models.Concepts.Property;

namespace Pims.Api.Models.Concepts.Document
{
    /// <summary>
    /// DocumentRelationshipModel class, provides a model to represent document relationships.
    /// </summary>
    public class PropertyDocumentRelationshipModel : DocumentRelationshipModel
    {
        public PropertyModel Property { get; set; }
    }
}
