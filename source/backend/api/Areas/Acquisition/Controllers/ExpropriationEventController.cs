using System;
using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Concepts.ExpropriationEvent;
using Pims.Api.Services;
using Pims.Core.Api.Exceptions;
using Pims.Core.Api.Policies;
using Pims.Core.Extensions;
using Pims.Core.Json;
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
        private const string LogMessage = "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}";

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
        [HttpGet("{acquisitionFileId:long}/expropriation-events")]
        [HasPermission(Permissions.AcquisitionFileView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<ExpropriationEventModel>), 200)]
        [SwaggerOperation(Tags = new[] { "acquisitionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetAcquisitionFileExpropriationEvents([FromRoute] long acquisitionFileId)
        {
            _logger.LogInformation(
                LogMessage,
                nameof(ExpropriationEventController),
                nameof(GetAcquisitionFileExpropriationEvents),
                User.GetUsername(),
                DateTime.Now);

            var expropriationEvents = _expropriationEventService.GetExpropriationEvents(acquisitionFileId);
            return new JsonResult(_mapper.Map<List<ExpropriationEventModel>>(expropriationEvents));
        }

        /// <summary>
        /// Get an expropriation event by Id.
        /// </summary>
        /// <returns>The expropriation event details.</returns>
        [HttpGet("{acquisitionFileId:long}/expropriation-events/{eventId:long}")]
        [HasPermission(Permissions.AcquisitionFileView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ExpropriationEventModel), 200)]
        [SwaggerOperation(Tags = new[] { "acquisitionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetAcquisitionFileExpropriationEventById([FromRoute] long acquisitionFileId, [FromRoute] long eventId)
        {
            _logger.LogInformation(
                LogMessage,
                nameof(ExpropriationEventController),
                nameof(GetAcquisitionFileExpropriationEventById),
                User.GetUsername(),
                DateTime.Now);

            var expropriationEvent = _expropriationEventService.GetExpropriationEventById(eventId);
            return new JsonResult(_mapper.Map<ExpropriationEventModel>(expropriationEvent));
        }

        /// <summary>
        /// Create an entry in the expropriation event history.
        /// </summary>
        /// <returns>The expropriation event details.</returns>
        [HttpPost("{acquisitionFileId:long}/expropriation-events")]
        [HasPermission(Permissions.AcquisitionFileEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ExpropriationEventModel), 201)]
        [SwaggerOperation(Tags = new[] { "acquisitionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult AddAcquisitionFileExpropriationEvent([FromRoute] long acquisitionFileId, [FromBody] ExpropriationEventModel expropriationEvent)
        {
            _logger.LogInformation(
                LogMessage,
                nameof(ExpropriationEventController),
                nameof(AddAcquisitionFileExpropriationEvent),
                User.GetUsername(),
                DateTime.Now);

            if (acquisitionFileId != expropriationEvent.AcquisitionFileId)
            {
                throw new BadRequestException("Invalid AcquisitionFileId.");
            }

            var newExpropriationEvent = _expropriationEventService.AddExpropriationEvent(acquisitionFileId, _mapper.Map<Dal.Entities.PimsExpropOwnerHistory>(expropriationEvent));
            return new JsonResult(_mapper.Map<ExpropriationEventModel>(newExpropriationEvent));
        }

        /// <summary>
        /// Update the expropriation event by Id.
        /// </summary>
        /// <returns>The expropriation event details.</returns>
        [HttpPut("{acquisitionFileId:long}/expropriation-events/{eventId:long}")]
        [HasPermission(Permissions.AcquisitionFileEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ExpropriationEventModel), 201)]
        [SwaggerOperation(Tags = new[] { "acquisitionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateAcquisitionFileExpropriationEvent([FromRoute] long acquisitionFileId, [FromRoute] long eventId, [FromBody] ExpropriationEventModel expropriationEvent)
        {
            _logger.LogInformation(
                LogMessage,
                nameof(ExpropriationEventController),
                nameof(UpdateAcquisitionFileExpropriationEvent),
                User.GetUsername(),
                DateTime.Now);

            if (acquisitionFileId != expropriationEvent.AcquisitionFileId)
            {
                throw new BadRequestException("Invalid AcquisitionFileId.");
            }

            var updatedExpropriationEvent = _expropriationEventService.UpdateExpropriationEvent(acquisitionFileId, _mapper.Map<Dal.Entities.PimsExpropOwnerHistory>(expropriationEvent));
            return new JsonResult(_mapper.Map<ExpropriationEventModel>(updatedExpropriationEvent));
        }

        /// <summary>
        /// Delete an expropriation event by Id.
        /// </summary>
        /// <returns>True if the operation was successful; false otherwise.</returns>
        [HttpDelete("{acquisitionFileId:long}/expropriation-events/{eventId:long}")]
        [HasPermission(Permissions.AcquisitionFileEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ExpropriationEventModel), 200)]
        [SwaggerOperation(Tags = new[] { "acquisitionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult DeleteAcquisitionFileExpropriationEvent([FromRoute] long acquisitionFileId, [FromRoute] long eventId)
        {
            _logger.LogInformation(
                LogMessage,
                nameof(ExpropriationEventController),
                nameof(DeleteAcquisitionFileExpropriationEvent),
                User.GetUsername(),
                DateTime.Now);

            var result = _expropriationEventService.DeleteExpropriationEvent(acquisitionFileId, eventId);
            return new JsonResult(result);
        }

        #endregion
    }
}
