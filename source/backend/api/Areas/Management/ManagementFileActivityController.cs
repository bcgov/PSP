using System;
using System.Collections.Generic;
using System.Linq;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Concepts.Property;
using Pims.Api.Services;
using Pims.Core.Api.Exceptions;
using Pims.Core.Api.Policies;
using Pims.Core.Extensions;
using Pims.Core.Json;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Management.Controllers
{
    /// <summary>
    /// ManagementFileActivityController class, provides endpoints for interacting with management file activities.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("managementfiles")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class ManagementFileActivityController : ControllerBase
    {
        #region Variables
        private readonly IPropertyService _propertyService;
        private readonly IManagementFileService _managementFileService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        #endregion

        /// <summary>
        /// Creates a new instance of a ManagementFileActivityController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="propertyService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        /// <param name="managementFileService"></param>
        public ManagementFileActivityController(IPropertyService propertyService, IMapper mapper, ILogger<ManagementFileActivityController> logger, IManagementFileService managementFileService)
        {
            _propertyService = propertyService;
            _managementFileService = managementFileService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Create the specified management file activity.
        /// </summary>
        /// <returns></returns>
        [HttpPost("{managementFileId:long}/management-activities")]
        [HasPermission(Permissions.ManagementAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ManagementActivityModel), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult CreateManagementActivity(long managementFileId, [FromBody] ManagementActivityModel activityModel)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(ManagementActivityController),
                nameof(CreateManagementActivity),
                User.GetUsername(),
                DateTime.Now);

            // Management file activities need to populate the managementFileId field.
            if (!activityModel.ManagementFileId.HasValue || activityModel.ManagementFileId.Value != managementFileId)
            {
                throw new BadRequestException("Invalid management file id.");
            }
            var activityEntity = _mapper.Map<PimsManagementActivity>(activityModel);
            var createdActivity = _propertyService.CreateActivity(activityEntity);

            return new JsonResult(_mapper.Map<ManagementActivityModel>(createdActivity));
        }

        /// <summary>
        /// Get the specified management file activity.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{managementFileId:long}/management-activities/{managementActivityId:long}")]
        [HasPermission(Permissions.ManagementView, Permissions.ActivityView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ManagementActivityModel), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetManagementActivity(long managementFileId, long managementActivityId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(ManagementActivityController),
                nameof(GetManagementActivity),
                User.GetUsername(),
                DateTime.Now);

            var activity = _propertyService.GetActivity(managementActivityId);

            if (activity.ManagementFileId != managementFileId)
            {
                throw new BadRequestException("Activity with the given id does not match the management file id");
            }

            return new JsonResult(_mapper.Map<ManagementActivityModel>(activity));
        }

        /// <summary>
        /// Get the specified management file's activities.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{managementFileId:long}/management-activities")]
        [HasPermission(Permissions.ManagementView, Permissions.ActivityView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ManagementActivityModel), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetManagementActivities(long managementFileId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(ManagementActivityController),
                nameof(GetManagementActivities),
                User.GetUsername(),
                DateTime.Now);

            var activities = _propertyService.GetFileActivities(managementFileId);

            return new JsonResult(_mapper.Map<IEnumerable<ManagementActivityModel>>(activities));
        }

        /// <summary>
        /// Update the specified management file activity.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{managementFileId:long}/management-activities/{activityId}")]
        [HasPermission(Permissions.ManagementEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ManagementActivityModel), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateManagementActivity(long managementFileId, long activityId, [FromBody] ManagementActivityModel activityModel)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(ManagementActivityController),
                nameof(UpdateManagementActivity),
                User.GetUsername(),
                DateTime.Now);

            var managementActivity = _mapper.Map<PimsManagementActivity>(activityModel);
            if (!managementActivity.ManagementFileId.HasValue || managementActivity.ManagementFileId != managementFileId || managementActivity.Internal_Id != activityId)
            {
                throw new BadRequestException("Invalid activity identifiers.");
            }

            var updatedProperty = _propertyService.UpdateActivity(managementActivity);

            return new JsonResult(_mapper.Map<ManagementActivityModel>(updatedProperty));
        }

        /// <summary>
        /// Delete the specified management file activity.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{managementFileId:long}/management-activities/{managementActivityId:long}")]
        [HasPermission(Permissions.ManagementEdit, Permissions.ActivityEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ManagementActivityModel), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult DeleteManagementActivity(long managementFileId, long managementActivityId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(ManagementActivityController),
                nameof(DeleteManagementActivity),
                User.GetUsername(),
                DateTime.Now);

            var deleted = _propertyService.DeleteFileActivity(managementFileId, managementActivityId);

            return new JsonResult(deleted);
        }

        /// <summary>
        /// Get all of the activities related to an property on the specified management file.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{managementFileId:long}/properties/management-activities")]
        [HasPermission(Permissions.ManagementView, Permissions.ActivityView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ManagementActivityModel), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetManagementPropertyActivities(long managementFileId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(ManagementActivityController),
                nameof(GetManagementPropertyActivities),
                User.GetUsername(),
                DateTime.Now);

            var propertyIds = _managementFileService.GetProperties(managementFileId).Select(mp => mp.PropertyId);
            var activities = _propertyService.GetActivitiesByPropertyIds(propertyIds);

            return new JsonResult(_mapper.Map<IEnumerable<ManagementActivityModel>>(activities));
        }
    }
}
