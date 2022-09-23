using System;
using System.Threading.Tasks;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pims.Api.Helpers.Extensions;
using Pims.Core.Http;
using Pims.Dal.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using KModel = Pims.Keycloak.Models;
using Model = Pims.Api.Models.Concepts;

namespace Pims.Api.Controllers
{
    /// <summary>
    /// UserController class, provides endpoints for managing users.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/users")]
    [Route("users")]
    public class UserController : ControllerBase
    {
        #region Variables
        private readonly Keycloak.Configuration.KeycloakOptions _optionsKeycloak;
        private readonly IProxyRequestClient _requestClient;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a UserController class.
        /// </summary>
        /// <param name="optionsKeycloak"></param>
        /// <param name="requestClient"></param>
        /// <param name="userRepository"></param>
        /// <param name="mapper"></param>
        public UserController(IOptionsMonitor<Keycloak.Configuration.KeycloakOptions> optionsKeycloak, IProxyRequestClient requestClient, IUserRepository userRepository, IMapper mapper)
        {
            _optionsKeycloak = optionsKeycloak.CurrentValue;
            _requestClient = requestClient;
            _userRepository = userRepository;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Redirects user to the keycloak user info endpoint.
        /// </summary>
        /// <returns></returns>
        [HttpGet("info")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(KModel.UserInfoModel), 200)]
        [ProducesResponseType(typeof(Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "user" })]
        public async Task<IActionResult> UserInfoAsync()
        {
            _optionsKeycloak.Validate(); // TODO: Validate configuration automatically.
            _optionsKeycloak.OpenIdConnect.Validate();
            var response = await _requestClient.ProxyGetAsync(Request, $"{_optionsKeycloak.Authority}{_optionsKeycloak.OpenIdConnect.UserInfo}");
            return await response.HandleResponseAsync<KModel.UserInfoModel>();
        }

        /// <summary>
        /// Returns user person info for given keycloakUserId.
        /// </summary>
        /// <param name="keycloakUserId"></param>
        /// <returns>User person info.</returns>
        [HttpGet("info/{keycloakUserId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Model.UserModel), 200)]
        [ProducesResponseType(typeof(Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "userInfo" })]
        public IActionResult UserBasicInfo([FromRoute] Guid keycloakUserId)
        {
            if (keycloakUserId == Guid.Empty)
            {
                return new JsonResult(new Models.ErrorResponseModel("Invalid keycloakUserId", "keycloakUserId should be a valid non empty guid"));
            }
            var entity = _userRepository.GetUserInfo(keycloakUserId);
            var user = _mapper.Map<Model.UserModel>(entity);
            return new JsonResult(user);
        }
        #endregion
    }
}
