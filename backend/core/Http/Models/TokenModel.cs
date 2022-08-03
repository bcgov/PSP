using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;

namespace Pims.Core.Http.Models
{
    /// <summary>
    /// TokenModel class, provides a model that represents a response for requesting an access token.
    /// </summary>
    public class TokenModel
    {

        public TokenModel()
        {
        }

        public TokenModel(string jwtToken, string refreshToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var decodedJwtToken = handler.ReadJwtToken(jwtToken);
            var decodedRefreshToken = handler.ReadJwtToken(jwtToken);

            AccessToken = jwtToken;
            ExpiresIn = decodedJwtToken.ValidTo.Subtract(DateTime.UtcNow).Seconds;
            RefreshExpiresIn = decodedRefreshToken.ValidTo.Subtract(DateTime.UtcNow).Seconds;
            RefreshToken = refreshToken;
            TokenType = decodedJwtToken.Header.Typ;
        }

        #region Properties

        /// <summary>
        /// get/set - The access token.
        /// </summary>
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// get/set - When the access token expires.
        /// </summary>
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        /// <summary>
        /// get/set - When the refresh token expires.
        /// </summary>
        [JsonPropertyName("refresh_expires_in")]
        public int RefreshExpiresIn { get; set; }

        /// <summary>
        /// get/set - The refresh token.
        /// </summary>
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// get/set - The access token type.
        /// </summary>
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// get/set - The session state ID.
        /// </summary>
        [JsonPropertyName("session_state")]
        public Guid SessionState { get; set; }

        /// <summary>
        /// get/set - The scope of the token.
        /// </summary>
        public string Scope { get; set; }
        #endregion
    }
}
