using System;
using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Concepts;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Extensions;
using Pims.Core.Json;
using Pims.Dal.Entities;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Disposition.Controllers
{
    /// <summary>
    /// DispositionFileController class, provides endpoints for interacting with disposition files.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("dispositionfiles")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class DispositionFileController : ControllerBase
    {
        #region Variables
        private readonly IDispositionFileService _dispositionService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a DispositionFileController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="dispositionService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        ///
        public DispositionFileController(IDispositionFileService dispositionService, IMapper mapper, ILogger<DispositionFileController> logger)
        {
            _dispositionService = dispositionService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Gets the specified disposition file.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:long}")]
        [HasPermission(Permissions.DispositionView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DispositionFileModel), 200)]
        [SwaggerOperation(Tags = new[] { "dispositionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetDispositionFile(long id)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(DispositionFileController),
                nameof(GetDispositionFile),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _dispositionService.GetType());

            var dispositionFile = _dispositionService.GetById(id);
            return new JsonResult(_mapper.Map<DispositionFileModel>(dispositionFile));
        }

        /// <summary>
        /// Gets the specified disposition file last updated-by information.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:long}/updateInfo")]
        [HasPermission(Permissions.DispositionView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Dal.Entities.Models.LastUpdatedByModel), 200)]
        [SwaggerOperation(Tags = new[] { "dispositionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetLastUpdatedBy(long id)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(DispositionFileController),
                nameof(GetLastUpdatedBy),
                User.GetUsername(),
                DateTime.Now);

            var lastUpdated = _dispositionService.GetLastUpdateInformation(id);
            return new JsonResult(lastUpdated);
        }

        /// <summary>
        /// Get the disposition file properties.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:long}/properties")]
        [HasPermission(Permissions.DispositionView, Permissions.PropertyView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DispositionFilePropertyModel>), 200)]
        [SwaggerOperation(Tags = new[] { "dispositionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetDispositionFileProperties(long id)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(DispositionFileController),
                nameof(GetDispositionFileProperties),
                User.GetUsername(),
                DateTime.Now);

            var dispositionfileProperties = _dispositionService.GetProperties(id);

            return new JsonResult(_mapper.Map<IEnumerable<DispositionFilePropertyModel>>(dispositionfileProperties));
        }

        /// <summary>
        /// Get all unique persons that belong to at least one disposition file as a team member.
        /// </summary>
        /// <returns></returns>
        [HttpGet("team-members")]
        [HasPermission(Permissions.DispositionView)]
        [HasPermission(Permissions.ContactView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DispositionFileTeamModel>), 200)]
        [SwaggerOperation(Tags = new[] { "dispositionfile" })]
        public IActionResult GetDispositionTeamMembers()
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(DispositionFileController),
                nameof(GetDispositionTeamMembers),
                User.GetUsername(),
                DateTime.Now);

            var team = _dispositionService.GetTeamMembers();

            return new JsonResult(_mapper.Map<IEnumerable<DispositionFileTeamModel>>(team));
        }

        #endregion
    }
}
