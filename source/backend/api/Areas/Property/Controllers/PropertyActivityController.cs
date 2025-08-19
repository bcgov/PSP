using System.Collections.Generic;
using System.Linq;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models.Concepts.Property;
using Pims.Api.Services;
using Pims.Core.Api.Exceptions;
using Pims.Core.Api.Policies;
using Pims.Core.Json;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Property.Controllers
{
    /// <summary>
    /// PropertyActivityController class, provides endpoints for interacting with properties activities.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("properties")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class PropertyActivityController : ControllerBase
    {
        #region Variables
        private readonly IPropertyService _propertyService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a PropertyActivityController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="propertyService"></param>
        /// <param name="mapper"></param>
        ///
        public PropertyActivityController(IPropertyService propertyService, IMapper mapper)
        {
            _propertyService = propertyService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Get the property management activities for the property with 'propertyId'.
        /// </summary>
        /// <returns>Collection of Property management activities.</returns>
        [HttpGet("{propertyId}/management-activities")]
        [HasPermission(Permissions.ManagementView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<ManagementActivityModel>), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetPropertyActivities(long propertyId)
        {
            var activities = _propertyService.GetActivities(propertyId);
            return new JsonResult(_mapper.Map<List<ManagementActivityModel>>(activities));
        }

        /// <summary>
        /// Get the activity with the given identifiers.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{propertyId}/management-activities/{activityId}")]
        [HasPermission(Permissions.ManagementView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ManagementActivityModel), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetPropertyActivity(long propertyId, long activityId)
        {
            var managementActivity = _propertyService.GetActivity(activityId);

            if (managementActivity.PimsManagementActivityProperties.Any(x => x.PropertyId == propertyId))
            {
                return new JsonResult(_mapper.Map<ManagementActivityModel>(managementActivity));
            }

            throw new BadRequestException("Activity with the given id does not match the property id");
        }

        /// <summary>
        /// Create the specified property activity.
        /// </summary>
        /// <returns></returns>
        [HttpPost("{propertyId}/management-activities")]
        [HasPermission(Permissions.ManagementAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ManagementActivityModel), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult CreatePropertyActivity(long propertyId, [FromBody] ManagementActivityModel activityModel)
        {
            if (propertyId != activityModel.ActivityProperties[0].PropertyId)
            {
                throw new BadRequestException("Invalid property id.");
            }
            var activityEntity = _mapper.Map<PimsManagementActivity>(activityModel);
            var createdActivity = _propertyService.CreateActivity(activityEntity);

            return new JsonResult(_mapper.Map<ManagementActivityModel>(createdActivity));
        }

        /// <summary>
        /// Update the specified property activity.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{propertyId}/management-activities/{activityId}")]
        [HasPermission(Permissions.ManagementEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ManagementActivityModel), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdatePropertyActivity(long propertyId, long activityId, [FromBody] ManagementActivityModel activityModel)
        {
            var managementActivity = _mapper.Map<PimsManagementActivity>(activityModel);
            if (!managementActivity.PimsManagementActivityProperties.Any(x => x.PropertyId == propertyId && x.ManagementActivityId == activityId)
                || managementActivity.ManagementActivityId != activityId)
            {
                throw new BadRequestException("Invalid activity identifiers.");
            }

            var updatedProperty = _propertyService.UpdateActivity(managementActivity);

            return new JsonResult(_mapper.Map<ManagementActivityModel>(updatedProperty));
        }

        /// <summary>
        /// Delete a Property's management activity.
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="activityId"></param>
        /// <returns></returns>
        [HttpDelete("{propertyId}/management-activities/{activityId}")]
        [HasPermission(Permissions.ManagementDelete)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult DeletePropertyActivity([FromRoute] long propertyId, [FromRoute] long activityId)
        {
            var result = _propertyService.DeleteActivity(activityId);
            return new JsonResult(result);
        }
        #endregion
    }
}
