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
        private string _accessToken;
        private string _refreshToken;
        private JwtSecurityTokenHandler _handler;

        public TokenModel(string jwtToken, string refreshToken)
        {
            _accessToken = jwtToken;
            _refreshToken = refreshToken;
        }

        public void InvalidateToken()
        {
            _accessToken = null;
            _refreshToken = null;
        }

        public void RenewToken(string jwtToken, string refreshToken)
        {
            _accessToken = jwtToken;
            _refreshToken = refreshToken;
        }

        private JwtSecurityTokenHandler GetHandler()
        {
            _handler ??= new JwtSecurityTokenHandler();

            return _handler;
        }

        private int? GetExpiresIn()
        {
            if (_accessToken is null)
            {
                return null;
            }

            return GetHandler().ReadJwtToken(_accessToken).ValidTo.Subtract(DateTime.UtcNow).Seconds;
        }

        private int? GetRefreshExpiresIn()
        {
            if (_refreshToken is null)
            {
                return null;
            }

            return GetHandler().ReadJwtToken(_refreshToken).ValidTo.Subtract(DateTime.UtcNow).Seconds;
        }

        private string GetTokeyType()
        {
            if (_accessToken is null)
            {
                return null;
            }

            return GetHandler().ReadJwtToken(_accessToken).Header.Typ;
        }

        #region Properties

        /// <summary>
        /// get/set - The access token.
        /// </summary>
        [JsonPropertyName("access_token")]
        public string AccessToken => _accessToken;

        /// <summary>
        /// get/set - The refresh token.
        /// </summary>
        [JsonPropertyName("refresh_token")]
        public string RefreshToken => _refreshToken;

        /// <summary>
        /// get/set - When the access token expires.
        /// </summary>
        [JsonPropertyName("expires_in")]
        public int? ExpiresIn => GetExpiresIn();

        /// <summary>
        /// get/set - When the refresh token expires.
        /// </summary>
        [JsonPropertyName("refresh_expires_in")]
        public int? RefreshExpiresIn => GetRefreshExpiresIn();

        /// <summary>
        /// get/set - The access token type.
        /// </summary>
        [JsonPropertyName("token_type")]
        public string TokenType => GetTokeyType();

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
