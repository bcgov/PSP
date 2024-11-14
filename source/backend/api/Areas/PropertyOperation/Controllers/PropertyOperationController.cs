using System;
using System.Collections.Generic;
using System.Linq;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Models.Concepts.PropertyOperation;
using Pims.Core.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Extensions;
using Pims.Core.Json;
using Pims.Core.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.PropertyOperation.Controllers
{
    /// <summary>
    /// PropertyOperationController class, provides endpoints for interacting with property operations.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("property/operations")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class PropertyOperationController : ControllerBase
    {
        #region Variables
        private readonly IPropertyOperationService _propertyOperationService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a PropertyOperationController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="propertyOperationService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        ///
        public PropertyOperationController(IPropertyOperationService propertyOperationService, IMapper mapper, ILogger<PropertyOperationController> logger)
        {
            _propertyOperationService = propertyOperationService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Endpoints

        [HttpPost("")]
        [HasPermission(Permissions.PropertyEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PropertyOperationModel>), 201)]
        [SwaggerOperation(Tags = new[] { "propertyoperation" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult RunPropertyOperations([FromBody] IEnumerable<PropertyOperationModel> operations)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(PropertyOperationController),
                nameof(PropertyOperationModel),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _propertyOperationService.GetType());

            if (!operations.Any())
            {
                return BadRequest("No property operations were sent.");
            }

            var operationTypeCode = operations.FirstOrDefault().PropertyOperationTypeCode;
            var propertyOperations = _mapper.Map<IEnumerable<Dal.Entities.PimsPropertyOperation>>(operations);
            if(!Enum.TryParse(operationTypeCode?.Id, out PropertyOperationTypes propertyOperationTypes))
            {
                return BadRequest("Unsupported property operation type code.");
            }
            switch (propertyOperationTypes)
            {
                case PropertyOperationTypes.SUBDIVIDE:
                    var subdividedProperties = _propertyOperationService.SubdivideProperty(propertyOperations);
                    return new JsonResult(_mapper.Map<IEnumerable<PropertyOperationModel>>(subdividedProperties));
                case PropertyOperationTypes.CONSOLIDATE:
                    var consolidatedProperties = _propertyOperationService.ConsolidateProperty(propertyOperations);
                    return new JsonResult(_mapper.Map<IEnumerable<PropertyOperationModel>>(consolidatedProperties));
                default:
                    return BadRequest("Unsupported property operation type code.");
            }
        }

        #endregion
    }
}
