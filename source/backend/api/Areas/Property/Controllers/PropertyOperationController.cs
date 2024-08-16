
using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models.Concepts.PropertyOperation;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Json;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Property.Controllers
{
    /// <summary>
    /// PropertyOperationController class, provides endpoints for interacting with property operations.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("properties")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class PropertyOperationController : ControllerBase
    {
        #region Variables
        private readonly IPropertyOperationService _propertyOperationService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a PropertyOperationController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="propertyOperationService"></param>
        /// <param name="mapper"></param>
        public PropertyOperationController(IPropertyOperationService propertyOperationService, IMapper mapper)
        {
            _propertyOperationService = propertyOperationService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints
        /// <summary>
        /// Get the property operations related to a given property
        /// </summary>
        /// <returns></returns>
        [HttpGet("{propertyId}/propertyOperations")]
        [HasPermission(Permissions.PropertyView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IList<PropertyOperationModel>), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetPropertyPropertyOperations(long propertyId)
        {
            var propertyOperations = _propertyOperationService.GetOperationsForProperty(propertyId);
            return new JsonResult(_mapper.Map<IList<PropertyOperationModel>>(propertyOperations));
        }

        #endregion
    }
}
