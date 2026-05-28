using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.Models.Mayan.Document
{
    /// <summary>
    /// Best-effort model for items returned by:
    /// /api/v4/search/advanced/documents.documentsearchresult/...
    ///
    /// Mayan versions/configs differ, so this includes ExtensionData to capture
    /// unknown fields safely.
    /// </summary>
    public sealed class DocumentSearchResult
    {
        // Commonly present in document-ish payloads.
        [JsonPropertyName("id")]
        public int? Id { get; init; }

        [JsonPropertyName("uuid")]
        public Guid? Uuid { get; init; }

        [JsonPropertyName("label")]
        public string? Label { get; init; }

        [JsonPropertyName("description")]
        public string? Description { get; init; }

        [JsonPropertyName("datetime_created")]
        public DateTimeOffset? DateTimeCreated { get; init; }

        [JsonPropertyName("datetime_modified")]
        public DateTimeOffset? DateTimeModified { get; init; }

        // Search endpoints sometimes include score / snippet-ish fields.
        [JsonPropertyName("score")]
        public double? Score { get; init; }

        [JsonPropertyName("highlights")]
        public object? Highlights { get; init; }

        /// <summary>
        /// Captures any additional fields returned by Mayan EDMS that are not explicitly modeled.
        /// This makes your deserialization resilient to schema differences.
        /// </summary>
        [JsonExtensionData]
        public Dictionary<string, JsonElement>? AdditionalFields { get; init; }
    }
}
