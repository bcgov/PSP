using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.Ches
{

    /// <summary>
    /// Represents a status response for a message or transaction.
    /// </summary>
    public class ChesStatusResponse
    {
        /// <summary>
        /// A corresponding transaction uuid.
        /// </summary>
        [JsonPropertyName("txId")]
        public string? TxId { get; set; }

        /// <summary>
        /// A corresponding message uuid.
        /// </summary>
        [JsonPropertyName("msgId")]
        public string? MsgId { get; set; }

        /// <summary>
        /// The current status of the message.
        /// </summary>
        [JsonPropertyName("status")]
        public string? Status { get; set; }

        /// <summary>
        /// A unique string which is associated with the message.
        /// </summary>
        [JsonPropertyName("tag")]
        public string? Tag { get; set; }

        /// <summary>
        /// UTC time this service first received this message queue request.
        /// </summary>
        [JsonPropertyName("createdTS")]
        public long? CreatedTS { get; set; }

        /// <summary>
        /// Desired UTC time for sending the message. 0 = Queue to send immediately.
        /// </summary>
        [JsonPropertyName("delayTS")]
        public long? DelayTS { get; set; }

        /// <summary>
        /// UTC time this message queue request was last updated.
        /// </summary>
        [JsonPropertyName("updatedTS")]
        public long? UpdatedTS { get; set; }

        /// <summary>
        /// A list of status changes to this message.
        /// </summary>
        [JsonPropertyName("statusHistory")]
        public List<ChesStatusHistory>? StatusHistory { get; set; }
    }
}