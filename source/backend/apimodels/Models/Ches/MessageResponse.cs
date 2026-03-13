using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.Ches
{
    /// <summary>
    /// MessageResponse class, provides a model that represents the response when a message was added to the CHES queue.
    /// </summary>
    public class MessageResponse
    {
        /// <summary>
        /// A corresponding message uuid.
        /// </summary>
        [JsonPropertyName("msgId")]
        public string MsgId { get; set; } = string.Empty;

        /// <summary>
        /// A unique string which is associated with the message.
        /// </summary>
        [JsonPropertyName("tag")]
        public string? Tag { get; set; }

        /// <summary>
        /// An array of recipient email addresses that this message will go to.
        /// </summary>
        [JsonPropertyName("to")]
        public List<string> To { get; set; } = new();
    }
}
