using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.Ches
{
    /// <summary>
    /// EmailContext provides the template variables for an email.
    /// </summary>
    public class EmailContext : IEmailContext
    {
        /// <summary>
        /// An array of recipients email addresses that will appear on the To: field.
        /// </summary>
        [JsonPropertyName("to")]
        public List<string> To { get; set; } = new List<string>();

        /// <summary>
        /// An array of recipients email addresses that will appear on the cc: field.
        /// </summary>
        [JsonPropertyName("cc")]
        public List<string> Cc { get; set; } = new List<string>();

        /// <summary>
        /// An array of recipients email addresses that will appear on the bcc: field.
        /// </summary>
        [JsonPropertyName("bcc")]
        public List<string> Bcc { get; set; } = new List<string>();

        /// <summary>
        /// A freeform JSON object of key-value pairs.
        /// All keys must be alphanumeric or underscore.
        /// </summary>
        [JsonPropertyName("context")]
        public Dictionary<string, object> Context { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Desired UTC time for sending the message. 0 = Queue to send immediately
        /// </summary>
        [JsonPropertyName("delayTS")]
        public long DelayTS { get; set; }

        /// <summary>
        /// A unique string to be associated with the message
        /// </summary>
        [JsonPropertyName("tag")]
        public string Tag { get; set; }
    }
}