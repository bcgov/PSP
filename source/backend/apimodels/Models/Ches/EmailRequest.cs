using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.Ches
{
    /// <summary>
    /// Represents a request to send an email via CHES.
    /// </summary>
    public class EmailRequest
    {
        /// <summary>
        /// An array of recipients email addresses that will appear on the To: field.
        /// </summary>
        [JsonPropertyName("to")]
        [System.ComponentModel.DataAnnotations.Required]
        public List<string> To { get; set; } = new();

        /// <summary>
        /// An array of recipients email addresses that will appear on the CC: field.
        /// </summary>
        [JsonPropertyName("cc")]
        public List<string> Cc { get; set; } = new();

        /// <summary>
        /// An array of recipients email addresses that will appear on the BCC: field.
        /// </summary>
        [JsonPropertyName("bcc")]
        public List<string> Bcc { get; set; } = new();

        /// <summary>
        /// The email address of the sender.
        /// All email addresses can be plain 'sender@server.com' or formatted '"Sender Name" sender@server.com'.
        /// </summary>
        [JsonPropertyName("from")]
        public string From { get; set; }

        /// <summary>
        /// The email subject.
        /// </summary>
        [JsonPropertyName("subject")]
        [System.ComponentModel.DataAnnotations.Required]
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// The body of the message as an Unicode string.
        /// </summary>
        [JsonPropertyName("body")]
        [System.ComponentModel.DataAnnotations.Required]
        public string Body { get; set; } = string.Empty;

        /// <summary>
        /// The email body type (html = content with html, text = plaintext).
        /// </summary>
        [JsonPropertyName("bodyType")]
        [JsonConverter(typeof(EmailBodyTypeJsonConverter))]
        [System.ComponentModel.DataAnnotations.Required]
        public EmailBodyType BodyType { get; set; } = EmailBodyType.Html;

        /// <summary>
        /// Identifies encoding for text/html strings (defaults to 'utf-8', other values are 'hex' and 'base64').
        /// </summary>
        [JsonPropertyName("encoding")]
        [JsonConverter(typeof(EmailEncodingJsonConverter))]
        public EmailEncoding Encoding { get; set; } = EmailEncoding.Utf8;

        /// <summary>
        /// Sets message importance headers, either 'high', 'normal' (default) or 'low'.
        /// </summary>
        [JsonPropertyName("priority")]
        [JsonConverter(typeof(EmailPriorityJsonConverter))]
        public EmailPriority Priority { get; set; } = EmailPriority.Normal;

        /// <summary>
        /// A unique string which is associated with the message.
        /// </summary>
        [JsonPropertyName("tag")]
        public string Tag { get; set; }

        /// <summary>
        /// Desired UTC time for sending the message. 0 = Queue to send immediately.
        /// </summary>
        [JsonPropertyName("delayTS")]
        public long? DelayTS { get; set; }

        /// <summary>
        /// An array of Attachment objects.
        /// </summary>
        [JsonPropertyName("attachments")]
        public List<EmailAttachment> Attachments { get; set; } = new List<EmailAttachment>();
    }

}
