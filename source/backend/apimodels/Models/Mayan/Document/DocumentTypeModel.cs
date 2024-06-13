using System.Text.Json.Serialization;

namespace Pims.Api.Models.Mayan.Document
{
    /// <summary>
    /// Represents a document type information.
    /// </summary>
    public class DocumentTypeModel
    {
        /// <summary>
        /// get/set - The document type id.
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - The document type label.
        /// </summary>
        [JsonPropertyName("label")]
        public string Label { get; set; }

        /// <summary>
        /// get/set - The delete time period.
        /// </summary>
        [JsonPropertyName("delete_time_period")]
        public int? DeleteTimePeriod { get; set; }

        /// <summary>
        /// get/set - The delete time unit. (e.g. days, months).
        /// </summary>
        [JsonPropertyName("delete_time_unit")]
        public string DeleteTimeUnit { get; set; }

        /// <summary>
        /// get/set - The expiration date of document stubs (documents with no files).
        /// </summary>
        [JsonPropertyName("document_stub_expiration_interval")]
        public int? DocumentStubExpirationInterval { get; set; }

        /// <summary>
        /// get/set - If document stubs should be removed automatically when above expiry interval reached.
        /// </summary>
        [JsonPropertyName("document_stub_pruning_enabled")]
        public bool? DocumentStubPruningEnabled { get; set; }

        /// <summary>
        /// get/set - The trash time period.
        /// </summary>
        [JsonPropertyName("trash_time_period")]
        public int? TrashTimePeriod { get; set; }

        /// <summary>
        /// get/set - The trash time unit. (e.g. days, months).
        /// </summary>
        [JsonPropertyName("trash_time_unit")]
        public string TrashTimeUnit { get; set; }

        /// <summary>
        /// get/set - File name generator backend.
        /// </summary>
        [JsonPropertyName("filename_generator_backend")]
        public string FilenameGeneratorBackend { get; set; }

        /// <summary>
        /// get/set - File name generator backend arguments.
        /// </summary>
        [JsonPropertyName("filename_generator_backend_arguments")]
        public string FilenameGeneratorBackendArguments { get; set; }
    }
}
