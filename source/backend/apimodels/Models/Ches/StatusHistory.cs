using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.Ches
{

    /// <summary>
    /// Represents a status history entry.
    /// </summary>
    public class ChesStatusHistory
    {
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("timestamp")]
        public long? Timestamp { get; set; }
    }
}