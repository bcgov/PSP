using System.Text.Json.Serialization;

namespace Pims.Api.Models.Mayan
{
    /// <summary>
    /// Represents the request from a token request to mayan.
    /// </summary>
    public class TokenRequest
    {
        /// <summary>
        /// get/set - The user to get a token for.
        /// </summary>
        [JsonPropertyName("username")]
        public string Username { get; set; }

        /// <summary>
        /// get/set - The user password.
        /// </summary>
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
