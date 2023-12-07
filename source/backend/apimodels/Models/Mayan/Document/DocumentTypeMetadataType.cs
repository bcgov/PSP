using System.Text.Json.Serialization;
using Pims.Api.Models.Mayan.Metadata;

namespace Pims.Api.Models.Mayan.Document
{
    /// <summary>
    /// Represents a document type information.
    /// </summary>
    public class DocumentTypeMetadataType
    {
        /// <summary>
        /// get/set - The document type metadata type relationship id.
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - The document type definition.
        /// </summary>
        [JsonPropertyName("document_type")]
        public DocumentType DocumentType { get; set; }

        /// <summary>
        /// get/set - The metadata type definition.
        /// </summary>
        [JsonPropertyName("metadata_type")]
        public MetadataType MetadataType { get; set; }

        /// <summary>
        /// get/set - The metadata required flag.
        /// </summary>
        [JsonPropertyName("required")]
        public bool Required { get; set; }

        /// <summary>
        /// get/set - Url request path for this information.
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
