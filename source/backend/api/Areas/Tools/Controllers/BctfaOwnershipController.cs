using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Areas.Reports.Controllers;
using Pims.Core.Api.Policies;
using Pims.Core.Extensions;
using Pims.Core.Security;
using Swashbuckle.AspNetCore.Annotations;
using Model = Pims.Ltsa.Models;

namespace Pims.Api.Areas.Tools.Controllers
{
    /// <summary>
    /// BctfaOwnershipController class, provides endpoints to update BCTFA ownership.
    /// </summary>
    [Authorize]
    [ApiController]
    [Area("tools")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[area]/bctfa")]
    [Route("[area]/bctfa")]
    public class BctfaOwnershipController : ControllerBase
    {
        #region Variables
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _user;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a BctfaOwnershipController class.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="user"></param>
        public BctfaOwnershipController(ILogger<DispositionController> logger, ClaimsPrincipal user)
        {
            _logger = logger;
            _user = user;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Upload a newline delimited list of pid numbers reflecting BCTFA ownership as per LTSA.
        /// </summary>
        /// <param name="ownershipFile">the file containing the list of pids.</param>
        /// <returns>The orders created within LTSA.</returns>
        [HttpPut("ownership")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(Pims.Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "tools-ltsa" })]
        [HasPermission(Permissions.BctfaOwnershipEdit)]
        public async Task<IActionResult> PutBctfaOwnership(IFormFile ownershipFile)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(DispositionController),
                nameof(LtsaController),
                _user.GetUsername(),
                DateTime.Now);

            _logger.LogDebug(ownershipFile.Serialize());

            // TODO: placeholder endpoint to facilitate LTSA integration.
            return new OkResult();
        }

        /// <summary>
        /// Upload a json structure of pids representing BCTFA ownership.
        /// </summary>
        /// <param name="ownershipData">the json structure containing the list of pids.</param>
        /// <returns>The orders created within LTSA.</returns>
        [HttpPut("ownership/list")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(Pims.Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "tools-ltsa" })]
        [HasPermission(Permissions.BctfaOwnershipEdit)]
        public async Task<IActionResult> PutBctfaOwnershipJson([FromBody] Model.BctfaOwnershipList ownershipData)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(DispositionController),
                nameof(LtsaController),
                _user.GetUsername(),
                DateTime.Now);

            _logger.LogDebug(ownershipData.Serialize());

            // TODO: placeholder endpoint to facilitate LTSA integration.
            return new OkResult();
        }
        #endregion
    }
}
