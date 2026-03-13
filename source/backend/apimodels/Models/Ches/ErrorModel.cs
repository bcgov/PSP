using System.Text.Json.Serialization;

namespace Pims.Api.Models.Ches
{
    /// <summary>
    /// Represents an error response from the CHES service.
    /// </summary>
    public class ErrorModel
    {
        /// <summary>
        /// The error message for the field.
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }

        /// <summary>
        /// Contents of the field that was in error.
        /// </summary>
        [JsonPropertyName("value")]
        public object Value { get; set; }
    }
}