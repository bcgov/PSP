using System;
using System.Collections.Generic;
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
    /// ManagementActivityController class, provides endpoints for interacting with management file activities.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("managementfiles")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class ManagementActivityController : ControllerBase
    {
        #region Variables
        private readonly IPropertyService _propertyService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        #endregion

        /// <summary>
        /// Creates a new instance of a ManagementActivityController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="propertyService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public ManagementActivityController(IPropertyService propertyService, IMapper mapper, ILogger<ManagementActivityController> logger)
        {
            _propertyService = propertyService;
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
        [ProducesResponseType(typeof(PropertyActivityModel), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult CreateManagementActivity(long managementFileId, [FromBody] PropertyActivityModel activityModel)
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
            var activityEntity = _mapper.Map<PimsPropertyActivity>(activityModel);
            var createdActivity = _propertyService.CreateActivity(activityEntity);

            return new JsonResult(_mapper.Map<PropertyActivityModel>(createdActivity));
        }

        /// <summary>
        /// Update the specified management file activity.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{managementFileId:long}/management-activities/{activityId}")]
        [HasPermission(Permissions.ManagementEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PropertyActivityModel), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateManagementActivity(long managementFileId, long activityId, [FromBody] PropertyActivityModel activityModel)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(ManagementActivityController),
                nameof(UpdateManagementActivity),
                User.GetUsername(),
                DateTime.Now);

            var propertyActivity = _mapper.Map<PimsPropertyActivity>(activityModel);
            if (!propertyActivity.ManagementFileId.HasValue || propertyActivity.ManagementFileId != managementFileId || propertyActivity.Internal_Id != activityId)
            {
                throw new BadRequestException("Invalid activity identifiers.");
            }

            var updatedProperty = _propertyService.UpdateActivity(propertyActivity);

            return new JsonResult(_mapper.Map<PropertyActivityModel>(updatedProperty));
        }

        /// <summary>
        /// Get the specified management file activity.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{managementFileId:long}/management-activities/{propertyActivityId:long}")]
        [HasPermission(Permissions.ManagementView, Permissions.ActivityView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PropertyActivityModel), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetManagementActivity(long managementFileId, long propertyActivityId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(ManagementActivityController),
                nameof(GetManagementActivity),
                User.GetUsername(),
                DateTime.Now);

            var activity = _propertyService.GetActivity(propertyActivityId);

            if (activity.ManagementFileId != managementFileId)
            {
                throw new BadRequestException("Activity with the given id does not match the management file id");
            }

            return new JsonResult(_mapper.Map<PropertyActivityModel>(activity));
        }

        /// <summary>
        /// Get the specified management file's activities.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{managementFileId:long}/management-activities")]
        [HasPermission(Permissions.ManagementView, Permissions.ActivityView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PropertyActivityModel), 200)]
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

            return new JsonResult(_mapper.Map<IEnumerable<PropertyActivityModel>>(activities));
        }

        /// <summary>
        /// Delete the specified management file activity.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{managementFileId:long}/management-activities/{propertyActivityId:long}")]
        [HasPermission(Permissions.ManagementEdit, Permissions.ActivityEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PropertyActivityModel), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult DeleteManagementActivity(long managementFileId, long propertyActivityId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(ManagementActivityController),
                nameof(DeleteManagementActivity),
                User.GetUsername(),
                DateTime.Now);

            var deleted = _propertyService.DeleteFileActivity(managementFileId, propertyActivityId);

            return new JsonResult(deleted);
        }
    }
}
