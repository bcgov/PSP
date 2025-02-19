using System;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.Mayan.Document
{
    /// <summary>
    /// Represents the document result information. Note, this does not contain the stored document.
    /// </summary>
    public class DocumentDetailModel
    {
        /// <summary>
        /// get/set - The document id.
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// get/set - Document label.
        /// </summary>
        [JsonPropertyName("label")]
        public string Label { get; set; }

        /// <summary>
        /// get/set - Document language.
        /// </summary>
        [JsonPropertyName("language")]
        public string Language { get; set; }

        /// <summary>
        /// get/set - Total number of results.
        /// </summary>
        [JsonPropertyName("datetime_created")]
        public DateTime DatetimeCreated { get; set; }

        /// <summary>
        /// get/set - The results of the query.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// get/set - The document uid.
        /// </summary>
        [JsonPropertyName("uuid")]
        public string Uuid { get; set; }

        /// <summary>
        /// get/set - The results of the query.
        /// </summary>
        [JsonPropertyName("file_latest")]
        public FileLatestModel FileLatest { get; set; }

        /// <summary>
        /// get/set - The document type information.
        /// </summary>
        [JsonPropertyName("document_type")]
        public DocumentTypeModel DocumentType { get; set; }
    }
}
