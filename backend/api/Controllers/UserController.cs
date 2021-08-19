using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Helpers.Extensions;
using Pims.Api.Models.User;
using Pims.Core.Http;
using Pims.Dal;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading.Tasks;
using Entity = Pims.Dal.Entities;
using KModel = Pims.Keycloak.Models;
using Model = Pims.Api.Models.User;

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
        private readonly ILogger<UserController> _logger;
        private readonly Keycloak.Configuration.KeycloakOptions _optionsKeycloak;
        private readonly IProxyRequestClient _requestClient;
        private readonly IPimsService _pimsService;
        private readonly IMapper _mapper;
        private readonly PimsOptions _options;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a UserController class.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="optionsKeycloak"></param>
        /// <param name="options"></param>
        /// <param name="pimsService"></param>
        /// <param name="mapper"></param>
        /// <param name="requestClient"></param>
        public UserController(ILogger<UserController> logger, IOptionsMonitor<Keycloak.Configuration.KeycloakOptions> optionsKeycloak, IOptions<PimsOptions> options, IPimsService pimsService, IMapper mapper, IProxyRequestClient requestClient)
        {
            _logger = logger;
            _optionsKeycloak = optionsKeycloak.CurrentValue;
            _requestClient = requestClient;
            _pimsService = pimsService;
            _mapper = mapper;
            _options = options.Value;
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
        #endregion
    }
}
