using System.Text.Json.Serialization;

namespace Pims.Api.Models.Mayan.Metadata
{
    /// <summary>
    /// Represents a mayan metadata type.
    /// </summary>
    public class MetadataType
    {
        /// <summary>
        /// get/set - The default value.
        /// </summary>
        [JsonPropertyName("default")]
        public string Default { get; set; }

        /// <summary>
        /// get/set - The metadata id.
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - The metadata type label.
        /// </summary>
        [JsonPropertyName("label")]
        public string Label { get; set; }

        /// <summary>
        /// get/set - Mayan lookup information.
        /// </summary>
        [JsonPropertyName("lookup")]
        public string Lookup { get; set; }

        /// <summary>
        /// get/set - Metadata name.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// get/set - The metadata parser.
        /// </summary>
        [JsonPropertyName("parser")]
        public string Parser { get; set; }

        /// <summary>
        /// get/set - The parser arguments.
        /// </summary>
        [JsonPropertyName("parser_arguments")]
        public string Parser_arguments { get; set; }

        /// <summary>
        /// get/set - The url path for the metadata.
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; }

        /// <summary>
        /// get/set - The metadata validation.
        /// </summary>
        [JsonPropertyName("validation")]
        public string Validation { get; set; }

        /// <summary>
        /// get/set - The metadata validation arguments.
        /// </summary>
        [JsonPropertyName("validation_arguments")]
        public string Validation_arguments { get; set; }
    }
}
