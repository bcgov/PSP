using System;
using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Concepts.Property;
using Pims.Api.Services;
using Pims.Core.Api.Policies;
using Pims.Core.Extensions;
using Pims.Core.Json;
using Pims.Core.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Property.Controllers
{
    /// <summary>
    /// PropertyTenureCleanupController class, provides endpoints for interacting with property historical numbers.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("properties")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class PropertyTenureCleanupController : ControllerBase
    {
        #region Variables
        private readonly IPropertyService _propertyService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a PropertyTenureCleanupController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="propertyService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        ///
        public PropertyTenureCleanupController(
            IPropertyService propertyService,
            IMapper mapper,
            ILogger<PropertyTenureCleanupController> logger
        )
        {
            _propertyService = propertyService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Gets a list tenure cleanups for a given property id.
        /// </summary>
        [HttpGet("{propertyId}/tenureCleanups")]
        [HasPermission(Permissions.PropertyView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PropertyTenureCleanupModel>), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetHistoricalNumbersForPropertyId(long propertyId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(PropertyTenureCleanupController),
                nameof(GetHistoricalNumbersForPropertyId),
                User.GetUsername(),
                DateTime.Now
            );

            var propertyTenureCleanups = _propertyService.GetTenureCleanupsForPropertyId(propertyId);
            return new JsonResult(_mapper.Map<List<PropertyTenureCleanupModel>>(propertyTenureCleanups));
        }
        #endregion
    }
}
