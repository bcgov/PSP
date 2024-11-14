using System;
using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Concepts.Property;
using Pims.Core.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Core.Json;
using Pims.Core.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.HistoricalNumber.Controllers
{
    /// <summary>
    /// HistoricalNumberController class, provides endpoints for interacting with property historical numbers.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("properties")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class HistoricalNumberController : ControllerBase
    {
        #region Variables
        private readonly IPropertyService _propertyService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a HistoricalNumberController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="propertyService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        ///
        public HistoricalNumberController(IPropertyService propertyService, IMapper mapper, ILogger<HistoricalNumberController> logger)
        {
            _propertyService = propertyService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Gets a list of the historic numbers for a given property id.
        /// </summary>
        [HttpGet("{propertyId}/historicalNumbers")]
        [HasPermission(Permissions.PropertyView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<HistoricalFileNumberModel>), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetHistoricalNumbersForPropertyId(long propertyId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(HistoricalNumberController),
                nameof(GetHistoricalNumbersForPropertyId),
                User.GetUsername(),
                DateTime.Now);

            var historicalNumbers = _propertyService.GetHistoricalNumbersForPropertyId(propertyId);
            return new JsonResult(_mapper.Map<List<HistoricalFileNumberModel>>(historicalNumbers));
        }

        /// <summary>
        /// Updates the list of historic numbers for a given property id.
        /// </summary>
        [HttpPut("{propertyId}/historicalNumbers")]
        [HasPermission(Permissions.PropertyEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<HistoricalFileNumberModel>), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateHistoricalNumbers(long propertyId, IEnumerable<HistoricalFileNumberModel> historicalNumbers)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(HistoricalNumberController),
                nameof(UpdateHistoricalNumbers),
                User.GetUsername(),
                DateTime.Now);

            try
            {
                var historicalEntities = _mapper.Map<IEnumerable<Dal.Entities.PimsHistoricalFileNumber>>(historicalNumbers);
                var updatedEntities = _propertyService.UpdateHistoricalFileNumbers(propertyId, historicalEntities);

                return new JsonResult(_mapper.Map<IEnumerable<HistoricalFileNumberModel>>(updatedEntities));
            }
            catch (DuplicateEntityException e)
            {
                return Conflict(e.Message);
            }
        }
        #endregion
    }
}
