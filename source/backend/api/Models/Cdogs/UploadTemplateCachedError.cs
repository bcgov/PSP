using System.Net;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.Cdogs
{
    /// <summary>
    /// Represents an error when a template has already been uploaded.
    /// </summary>
    public class UploadTemplateCachedError
    {
        /// <summary>
        /// get/set - The http error type.
        /// </summary>
        [JsonPropertyName("type")]
        public string ErrorType { get; set; }

        /// <summary>
        /// get/set - Error title.
        /// </summary>
        [JsonPropertyName("title")]
        public string ErrorTitle { get; set; }

        /// <summary>
        /// get/set - The http status error.
        /// </summary>
        [JsonPropertyName("status")]
        public HttpStatusCode HttpStatus { get; set; }

        /// <summary>
        /// get/set - The description of the error.
        /// </summary>
        [JsonPropertyName("detail")]
        public string ErrorDetail { get; set; }

        /// <summary>
        /// get/set - The hash corresponding to the file already uploaded.
        /// </summary>
        [JsonPropertyName("hash")]
        public string FileHash { get; set; }
    }
}
