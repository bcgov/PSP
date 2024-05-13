using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models.Concepts.Property;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Json;
using Pims.Dal.Security;
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
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a HistoricalNumberController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="propertyService"></param>
        /// <param name="mapper"></param>
        ///
        public HistoricalNumberController(IPropertyService propertyService, IMapper mapper)
        {
            _propertyService = propertyService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets a list of the historic numbers for a given property id.
        /// </summary>
        [HttpGet("{propertyId}/historicalNumbers")]
        [HasPermission(Permissions.PropertyView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(HistoricalFileNumberModel), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetHistoricalNumbersForPropertyId(long propertyId)
        {
            var historicalNumbers = _propertyService.GetHistoricalNumbersForPropertyId(propertyId);
            return new JsonResult(_mapper.Map<List<HistoricalFileNumberModel>>(historicalNumbers));
        }
        #endregion
    }
}
