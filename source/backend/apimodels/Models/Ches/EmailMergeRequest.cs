using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.Ches
{

    /// <summary>
    /// Represents a mail merge request for CHES.
    /// </summary>
    public class ChesMergeRequest
    {
        /// <summary>
        /// The email address of the sender. 
        /// All email addresses can be plain 'sender@server.com' or formatted '"Sender Name" sender@server.com'.
        /// </summary>
        [JsonPropertyName("from")]
        [System.ComponentModel.DataAnnotations.Required]
        public string From { get; set; } = string.Empty;

        /// <summary>
        /// The email subject.
        /// </summary>
        [JsonPropertyName("subject")]
        [System.ComponentModel.DataAnnotations.Required]
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// A body template of the message as an Unicode string. 
        /// Refer to https://mozilla.github.io/nunjucks/templating.html for template syntax.
        /// </summary>
        [JsonPropertyName("body")]
        [System.ComponentModel.DataAnnotations.Required]
        public string Body { get; set; } = string.Empty;

        /// <summary>
        /// The email body type (html = content with html, text = plaintext).
        /// </summary>
        [JsonPropertyName("bodyType")]
        [System.ComponentModel.DataAnnotations.Required]
        public EmailBodyType BodyType { get; set; } = EmailBodyType.Html;

        /// <summary>
        /// Identifies encoding for text/html strings (defaults to 'utf-8', other values are 'hex' and 'base64').
        /// </summary>
        [JsonPropertyName("encoding")]
        public EmailEncoding Encoding { get; set; } = EmailEncoding.Utf8;

        /// <summary>
        /// Sets message importance headers, either 'high', 'normal' (default) or 'low'..
        /// </summary>
        [JsonPropertyName("priority")]
        public EmailPriority Priority { get; set; } = EmailPriority.Normal;

        /// <summary>
        /// An array of Attachment objects.
        /// </summary>
        [JsonPropertyName("attachments")]
        public List<EmailAttachment>? Attachments { get; set; }

        /// <summary>
        /// An array of context objects.
        /// </summary>
        [JsonPropertyName("contexts")]
        [System.ComponentModel.DataAnnotations.Required]
        public List<EmailContext> Contexts { get; set; } = new();
    }
}