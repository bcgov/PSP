using System.Text.Json.Serialization;
using Pims.Api.Models.Mayan.Metadata;

namespace Pims.Api.Models.Mayan.Document
{
    /// <summary>
    /// Represents the document metada. Note, this does not contain the stored document.
    /// </summary>
    public class DocumentMetadataModel
    {
        /// <summary>
        /// get/set - Document detail information.
        /// </summary>
        [JsonPropertyName("document")]
        public DocumentDetailModel Document { get; set; }

        /// <summary>
        /// get/set - The document metadata id.
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// get/set - The metadata type.
        /// </summary>
        [JsonPropertyName("metadata_type")]
        public MetadataTypeModel MetadataType { get; set; }

        /// <summary>
        /// get/set - The source url.
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; }

        /// <summary>
        /// get/set - Metadata value.
        /// </summary>
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}
