using System;
using System.Collections.Generic;
using System.Linq;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Concepts.ManagementFile;
using Pims.Api.Services;
using Pims.Core.Api.Policies;
using Pims.Core.Extensions;
using Pims.Core.Json;
using Pims.Core.Security;
using Pims.Dal.Exceptions;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Management.Controllers
{
    /// <summary>
    /// ManagementFileController class, provides endpoints for interacting with management files.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("managementfiles")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class ManagementFileController : ControllerBase
    {
        #region Variables
        private readonly IManagementFileService _managementService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a ManagementFileController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="managementService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        ///
        public ManagementFileController(IManagementFileService managementService, IMapper mapper, ILogger<ManagementFileController> logger)
        {
            _managementService = managementService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Gets the specified management file.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:long}")]
        [HasPermission(Permissions.ManagementView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ManagementFileModel), 200)]
        [SwaggerOperation(Tags = new[] { "managementfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetManagementFile(long id)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(ManagementFileController),
                nameof(GetManagementFile),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _managementService.GetType());

            var managementFile = _managementService.GetById(id);
            return new JsonResult(_mapper.Map<ManagementFileModel>(managementFile));
        }

        /// <summary>
        /// Creates a new Management File entity.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [TypeFilter(typeof(NullJsonResultFilter))]
        [HasPermission(Permissions.ManagementAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ManagementFileModel), 200)]
        [SwaggerOperation(Tags = new[] { "managementfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult AddManagementFile([FromBody] ManagementFileModel model, [FromQuery] string[] userOverrideCodes)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(ManagementFileController),
                nameof(AddManagementFile),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _managementService.GetType());

            var managementFileEntity = _mapper.Map<Dal.Entities.PimsManagementFile>(model);
            var managementFile = _managementService.Add(managementFileEntity, userOverrideCodes.Select(oc => UserOverrideCode.Parse(oc)));

            return new JsonResult(_mapper.Map<ManagementFileModel>(managementFile));
        }

        [HttpPut("{id:long}")]
        [TypeFilter(typeof(NullJsonResultFilter))]
        [HasPermission(Permissions.ManagementEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ManagementFileModel), 200)]
        [SwaggerOperation(Tags = new[] { "managementfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateManagementFile([FromRoute] long id, [FromBody] ManagementFileModel model, [FromQuery] string[] userOverrideCodes)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(ManagementFileController),
                nameof(UpdateManagementFile),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _managementService.GetType());

            var managementFileEntity = _mapper.Map<Dal.Entities.PimsManagementFile>(model);
            var managementFile = _managementService.Update(id, managementFileEntity, userOverrideCodes.Select(oc => UserOverrideCode.Parse(oc)));

            return new JsonResult(_mapper.Map<ManagementFileModel>(managementFile));
        }

        /// <summary>
        /// Gets the specified management file last updated-by information.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:long}/updateInfo")]
        [HasPermission(Permissions.ManagementView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Dal.Entities.Models.LastUpdatedByModel), 200)]
        [SwaggerOperation(Tags = new[] { "managementfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetLastUpdatedBy(long id)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(ManagementFileController),
                nameof(GetLastUpdatedBy),
                User.GetUsername(),
                DateTime.Now);

            var lastUpdated = _managementService.GetLastUpdateInformation(id);
            return new JsonResult(lastUpdated);
        }

        /// <summary>
        /// Get the management file properties.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:long}/properties")]
        [HasPermission(Permissions.ManagementView, Permissions.PropertyView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<ManagementFilePropertyModel>), 200)]
        [SwaggerOperation(Tags = new[] { "managementfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetManagementFileProperties(long id)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(ManagementFileController),
                nameof(GetManagementFileProperties),
                User.GetUsername(),
                DateTime.Now);

            var managementfileProperties = _managementService.GetProperties(id);

            return new JsonResult(_mapper.Map<IEnumerable<ManagementFilePropertyModel>>(managementfileProperties));
        }

        /// <summary>
        /// Get all unique persons and organizations that belong to at least one management file as a team member.
        /// </summary>
        /// <returns></returns>
        [HttpGet("team-members")]
        [HasPermission(Permissions.ManagementView)]
        [HasPermission(Permissions.ContactView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<ManagementFileTeamModel>), 200)]
        [SwaggerOperation(Tags = new[] { "managementfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetManagementTeamMembers()
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(ManagementFileController),
                nameof(GetManagementTeamMembers),
                User.GetUsername(),
                DateTime.Now);

            var team = _managementService.GetTeamMembers();

            return new JsonResult(_mapper.Map<IEnumerable<ManagementFileTeamModel>>(team));
        }

        /// <summary>
        /// Update the management file properties.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id:long}/properties")]
        [HasPermission(Permissions.ManagementEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ManagementFileModel), 200)]
        [SwaggerOperation(Tags = new[] { "managementfile" })]
        public IActionResult UpdateManagementFileProperties([FromBody] ManagementFileModel managementFileModel, [FromQuery] string[] userOverrideCodes)
        {
            var managementFileEntity = _mapper.Map<Dal.Entities.PimsManagementFile>(managementFileModel);
            var managementFile = _managementService.UpdateProperties(managementFileEntity, userOverrideCodes.Select(oc => UserOverrideCode.Parse(oc)));
            return new JsonResult(_mapper.Map<ManagementFileModel>(managementFile));
        }

        #endregion
    }
}
