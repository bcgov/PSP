using System.Text.Json.Serialization;

namespace Pims.Api.Models.Cdogs
{
    /// <summary>
    /// Represents a JWT (JSON Web Token) response.
    /// </summary>
    public class JwtResponse
    {
        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the token expiration in minutes.
        /// </summary>
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Gets or sets the refresh token expiration in minutes.
        /// </summary>
        [JsonPropertyName("refresh_expires_in")]
        public int RefreshExpiresIn { get; set; }

        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the token type.
        /// </summary>
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// Gets or sets the not-before-policy.
        /// </summary>
        [JsonPropertyName("not-before-policy")]
        public int NotBeforePolicy { get; set; }

        /// <summary>
        /// Gets or sets the session state.
        /// </summary>
        [JsonPropertyName("session_state")]
        public string SessionState { get; set; }

        /// <summary>
        /// Gets or sets the scope.
        /// </summary>
        [JsonPropertyName("scope")]
        public string Scope { get; set; }
    }
}
