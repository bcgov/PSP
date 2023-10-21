using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models.Concepts;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Json;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
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
        [HasPermission(Permissions.PropertyView)]
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
        /// Get the activity with the given identifiers.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{propertyId}/management-activities/{activityId}")]
        [HasPermission(Permissions.PropertyView)]
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
        [HasPermission(Permissions.PropertyEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PropertyActivityModel), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult CreateContact(long propertyId, [FromBody] PropertyActivityModel activityModel)
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
        [HasPermission(Permissions.PropertyEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PropertyActivityModel), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateContact(long propertyId, long activityId, [FromBody] PropertyActivityModel activityModel)
        {
            if (propertyId != activityModel.ActivityProperties[0].PropertyId || activityId != activityModel.Id)
            {
                throw new BadRequestException("Invalid activity identifiers.");
            }
            var activityEntitiy = _mapper.Map<PimsPropertyActivity>(activityModel);
            var updatedProperty = _propertyService.UpdateActivity(activityEntitiy);

            return new JsonResult(_mapper.Map<PropertyActivityModel>(updatedProperty));
        }

        /// <summary>
        /// Deletes the property activity with the matching id.
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="activityId">Used to identify the entity to delete.</param>
        /// <returns></returns>
        [HttpDelete("{propertyId}/management-activities/{activityId}")]
        [HasPermission(Permissions.PropertyEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult DeleteContact([FromRoute] long propertyId, [FromRoute] long activityId)
        {
            var result = _propertyService.DeleteActivity(activityId);
            return new JsonResult(result);
        }
        #endregion
    }
}
