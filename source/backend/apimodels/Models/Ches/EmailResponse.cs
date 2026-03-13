using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Pims.Api.Models.Ches
{
    /// <summary>
    /// Represents the response from CHES after sending an email.
    /// </summary>
    public class EmailResponse
    {
        /// <summary>
        /// A corresponding transaction uuid.
        /// </summary>
        [JsonPropertyName("txId")]
        public string TxId { get; set; } = string.Empty;

        /// <summary>
        /// Array of objects.
        /// Each object contains msgId, tag and to fields.
        /// </summary>
        [JsonPropertyName("messages")]
        public List<MessageResponse> Messages { get; set; } = new();
    }

}
