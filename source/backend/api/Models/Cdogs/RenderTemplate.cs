using System.Text.Json.Serialization;

namespace Pims.Api.Models.Cdogs
{
    /// <summary>
    /// Represents a template for rendering files.
    /// </summary>
    public class RenderTemplate
    {
        /// <summary>
        /// get/set - The template content.
        /// </summary>
        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// get/set - Content encoding type.
        /// </summary>
        [JsonPropertyName("encodingType")]
        public string EncodingType { get; set; } = "base64";

        /// <summary>
        /// get/set - File type.
        /// </summary>
        [JsonPropertyName("fileType")]
        public string FileType { get; set; } = "docx";
    }
}
