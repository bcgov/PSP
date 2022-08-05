using System.Text.Json.Serialization;

namespace Pims.Api.Models.Mayan
{
    /// <summary>
    /// Represents the return from a token request to mayan.
    /// </summary>
    public class TokenResult
    {
        /// <summary>
        /// get/set - The token requested.
        /// </summary>
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
