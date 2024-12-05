using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models.Concepts.Property;
using Pims.Core.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Json;
using Pims.Core.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Property.Controllers
{
    /// <summary>
    /// PropertyManagementController class, provides endpoints for interacting with property management information.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("properties")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class PropertyManagementController : ControllerBase
    {
        #region Variables
        private readonly IPropertyService _propertyService;
        private readonly IMapper _mapper;
        #endregion

        /// <summary>
        /// Creates a new instance of a PropertyManagementController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="propertyService"></param>
        /// <param name="mapper"></param>
        public PropertyManagementController(IPropertyService propertyService, IMapper mapper)
        {
            _propertyService = propertyService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get the property management information for the property with 'propertyId'.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{propertyId}/management")]
        [HasPermission(Permissions.ManagementView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PropertyManagementModel), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetPropertyManagement(long propertyId)
        {
            return new JsonResult(_propertyService.GetPropertyManagement(propertyId));
        }

        /// <summary>
        /// Update the specified property management information.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{propertyId}/management")]
        [HasPermission(Permissions.ManagementEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PropertyManagementModel), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdatePropertyManagement([FromBody] PropertyManagementModel propertyManagementModel)
        {
            var propertyEntity = _mapper.Map<Pims.Dal.Entities.PimsProperty>(propertyManagementModel);
            return new JsonResult(_propertyService.UpdatePropertyManagement(propertyEntity));
        }
    }
}
