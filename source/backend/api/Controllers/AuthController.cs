using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pims.Core.Extensions;
using Pims.Core.Http.Configuration;
using Pims.Dal.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using Model = Pims.Api.Models.Auth;

namespace Pims.Api.Controllers
{
    /// <summary>
    /// AuthController class, provides endpoints for authentication.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/auth")]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        #region Variables
        private readonly Keycloak.Configuration.KeycloakOptions _optionsKeycloak;
        private readonly OpenIdConnectOptions _optionsOpenIdConnect;
        private readonly IUserRepository _userRepository;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a AuthController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="optionsKeycloak"></param>
        /// <param name="optionsOpenIdConnect"></param>
        /// <param name="userRepository"></param>
        public AuthController(IOptionsMonitor<Keycloak.Configuration.KeycloakOptions> optionsKeycloak, IOptionsMonitor<OpenIdConnectOptions> optionsOpenIdConnect, IUserRepository userRepository)
        {
            _optionsKeycloak = optionsKeycloak.CurrentValue;
            _optionsOpenIdConnect = optionsOpenIdConnect.CurrentValue;
            _userRepository = userRepository;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Activates the new authenticated user with PIMS.
        /// If the user is new it will return 201 if successful.
        /// If the user exists already it will return 200 if successful.
        /// Note - This requires KeyCloak client mapping to include the appropriate claims to activate the user (email, family name, given name, groups, realm roles).
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("activate")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Model.UserModel), 200)]
        [SwaggerOperation(Tags = new[] { "auth" })]
        public IActionResult Activate()
        {
            var key = this.User.GetUserKey();
            var exists = _userRepository.UserExists(key);

            var user = _userRepository.Activate();

            if (!exists)
            {
                // brand-new users cannot have claims mismatch
                return new CreatedResult($"{user.GuidIdentifierValue}", new Model.UserModel(user.Internal_Id, user.GuidIdentifierValue.Value, true));
            }

            bool hasValidClaims = _userRepository.ValidateClaims(user);

            return new JsonResult(new Model.UserModel(user.Internal_Id, user.GuidIdentifierValue.Value, hasValidClaims));
        }

        /// <summary>
        /// Redirect to the keycloak login page.
        /// </summary>
        /// <returns></returns>
        [HttpGet("login")]
        [SwaggerOperation(Tags = new[] { "auth" })]
        public IActionResult Login(string redirect_uri)
        {
            var uri = new UriBuilder($"{_optionsOpenIdConnect.Authority}{_optionsOpenIdConnect.Login}");
            uri.AppendQuery("client_id", _optionsKeycloak.Client);
            uri.AppendQuery("redirect_uri", redirect_uri);
            return Redirect(uri.ToString());
        }

        /// <summary>
        /// Log the current user out.
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        [SwaggerOperation(Tags = new[] { "auth" })]
        public IActionResult Logout(string redirect_uri)
        {
            var uri = new UriBuilder($"{_optionsOpenIdConnect.Authority}{_optionsOpenIdConnect.Logout}");
            uri.AppendQuery("client_id", _optionsKeycloak.Client);
            uri.AppendQuery("redirect_uri", redirect_uri);
            return Redirect(uri.ToString());
        }

        /// <summary>
        /// Return a list of claims for the current user.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("claims")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Model.ClaimModel>), 200)]
        [SwaggerOperation(Tags = new[] { "auth" })]
        public IActionResult Claims()
        {
            return new JsonResult(User.Claims.Select(c => new Model.ClaimModel(c.Type, c.Value)));
        }
        #endregion
    }
}
