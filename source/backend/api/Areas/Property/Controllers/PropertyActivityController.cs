using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Core.Api.Exceptions;
using Pims.Api.Models.Concepts.Property;
using Pims.Core.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Json;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
using Pims.Core.Security;
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
        private readonly ILookupRepository _lookupRepository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a PropertyActivityController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="propertyService"></param>
        /// /// <param name="lookupRepository"></param>
        /// <param name="mapper"></param>
        ///
        public PropertyActivityController(IPropertyService propertyService, ILookupRepository lookupRepository, IMapper mapper)
        {
            _propertyService = propertyService;
            _lookupRepository = lookupRepository;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Get the activity subtypes.
        /// </summary>
        /// <returns></returns>
        [HttpGet("management-activities/subtypes")]
        [HasPermission(Permissions.ManagementView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<PropertyActivitySubtypeModel>), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetPropertyActivitySubtypes()
        {
            var subTypes = _lookupRepository.GetAllPropMgmtActivitySubtypes();
            return new JsonResult(_mapper.Map<List<PropertyActivitySubtypeModel>>(subTypes));
        }

        /// <summary>
        /// Get the property management activities for the property with 'propertyId'.
        /// </summary>
        /// <returns>Collection of Property management activities.</returns>
        [HttpGet("{propertyId}/management-activities")]
        [HasPermission(Permissions.ManagementView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<PropertyActivityModel>), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetPropertyActivities(long propertyId)
        {
            var activities = _propertyService.GetActivities(propertyId);
            return new JsonResult(_mapper.Map<List<PropertyActivityModel>>(activities));
        }

        /// <summary>
        /// Get the activity with the given identifiers.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{propertyId}/management-activities/{activityId}")]
        [HasPermission(Permissions.ManagementView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PropertyActivityModel), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetPropertyActivity(long propertyId, long activityId)
        {
            var activity = _propertyService.GetActivity(propertyId, activityId);
            return new JsonResult(_mapper.Map<PropertyActivityModel>(activity));
        }

        /// <summary>
        /// Create the specified property activity.
        /// </summary>
        /// <returns></returns>
        [HttpPost("{propertyId}/management-activities")]
        [HasPermission(Permissions.ManagementAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PropertyActivityModel), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult CreatePropertyActivity(long propertyId, [FromBody] PropertyActivityModel activityModel)
        {
            if (propertyId != activityModel.ActivityProperties[0].PropertyId)
            {
                throw new BadRequestException("Invalid property id.");
            }
            var activityEntity = _mapper.Map<PimsPropertyActivity>(activityModel);
            var createdActivity = _propertyService.CreateActivity(activityEntity);

            return new JsonResult(_mapper.Map<PropertyActivityModel>(createdActivity));
        }

        /// <summary>
        /// Update the specified property activity.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{propertyId}/management-activities/{activityId}")]
        [HasPermission(Permissions.ManagementEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PropertyActivityModel), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdatePropertyActivity(long propertyId, long activityId, [FromBody] PropertyActivityModel activityModel)
        {
            var activityEntity = _mapper.Map<PimsPropertyActivity>(activityModel);
            var updatedProperty = _propertyService.UpdateActivity(propertyId, activityId, activityEntity);

            return new JsonResult(_mapper.Map<PropertyActivityModel>(updatedProperty));
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
