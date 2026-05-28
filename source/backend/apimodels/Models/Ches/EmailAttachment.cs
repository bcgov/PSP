
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
#nullable enable

namespace Pims.Api.Models.Ches
{
    /// <summary>
    /// Represents an attachment for CHES email.
    /// </summary>
    public class EmailAttachment : IAttachment
    {
        /// <summary>
        /// Buffer or a Stream contents for the attachment.
        /// </summary>
        [JsonPropertyName("content")]
        public string? Content { get; set; }

        /// <summary>
        /// Optional content type for the attachment.
        /// If not set, it will be derived from the filename property.
        /// </summary>
        [JsonPropertyName("contentType")]
        public string? ContentType { get; set; }

        /// <summary>
        /// Filename to be reported as the name of the attached file.
        /// Use of unicode is allowed.
        /// </summary>
        [JsonPropertyName("filename")]
        public string? Filename { get; set; }
    }
}
