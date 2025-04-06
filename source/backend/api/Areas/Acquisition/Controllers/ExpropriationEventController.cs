using System;
using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Concepts.ExpropriationEvent;
using Pims.Api.Services;
using Pims.Core.Api.Policies;
using Pims.Core.Extensions;
using Pims.Core.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Acquisition.Controllers
{
    /// <summary>
    /// ExpropriationEventController class, provides endpoints for interacting with actions involving a property owner associated with an expropriation.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("acquisitionfiles")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class ExpropriationEventController : ControllerBase
    {
        #region Variables
        private readonly IExpropriationEventService _expropriationEventService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        #endregion

        /// <summary>
        /// Creates a new instance of a ExpropriationEventController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="expropriationEventService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public ExpropriationEventController(IExpropriationEventService expropriationEventService, IMapper mapper, ILogger<ExpropriationEventController> logger)
        {
            _expropriationEventService = expropriationEventService;
            _mapper = mapper;
            _logger = logger;
        }

        #region Endpoints

        /// <summary>
        /// Get the acquisition file expropriation events (history).
        /// </summary>
        /// <returns>The expropriation events.</returns>
        [HttpGet("{id:long}/expropriation-events")]
        [HasPermission(Permissions.AcquisitionFileView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<ExpropriationEventModel>), 200)]
        [SwaggerOperation(Tags = new[] { "acquisitionfile" })]
        public IActionResult GetAcquisitionFileExpropriationEvents([FromRoute] long id)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(ExpropriationEventController),
                nameof(GetAcquisitionFileExpropriationEvents),
                User.GetUsername(),
                DateTime.Now);

            var expropriationEvents = _expropriationEventService.GetExpropriationEvents(id);
            return new JsonResult(_mapper.Map<List<ExpropriationEventModel>>(expropriationEvents));
        }

        #endregion
    }
}
