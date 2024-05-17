using System.Text.Json.Serialization;

namespace Pims.Api.Models.Mayan.Document
{
    /// <summary>
    /// Represents a mayan file page.
    /// </summary>
    public class FilePageModel
    {
        /// <summary>
        /// get/set - id of the file page.
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - id of the document.
        /// </summary>
        [JsonPropertyName("document_file_id")]
        public long DocumentFileId { get; set; }

        /// <summary>
        /// get/set - label of the document file.
        /// </summary>
        [JsonPropertyName("document_file_url")]
        public string Label { get; set; }

        /// <summary>
        /// get/set - url of the image for this page.
        /// </summary>
        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// get/set - the number of the page within the file.
        /// </summary>
        [JsonPropertyName("page_number")]
        public long PageNumber { get; set; }

        /// <summary>
        /// get/set - The url of the page.
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
