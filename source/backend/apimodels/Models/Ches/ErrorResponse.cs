using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.Ches
{
    /// <summary>
    /// ErrorResponse class, provides a model that represents an error returned from CHES.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// What type of problem, link to explanation of problem.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// Title of problem, generally the Http Status Code description.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// The Http Status code.
        /// </summary>
        [JsonPropertyName("status")]
        public long Status { get; set; }

        /// <summary>
        /// Short description of why this problem was raised.
        /// </summary>
        [JsonPropertyName("detail")]
        public string Detail { get; set; }

        /// <summary>
        /// A list of errors associated with the response.
        /// </summary>
        [JsonPropertyName("errors")]
        public List<ErrorModel> Errors { get; set; } = new List<ErrorModel>();
    }
}