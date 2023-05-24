using System;
using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Concepts;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Takes.Controllers
{
    /// <summary>
    /// TakeController class, provides endpoints for interacting with takes.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("takes")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class TakeController : ControllerBase
    {
        #region Variables
        private readonly ITakeService _takeService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a TakeController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="takeService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        ///
        public TakeController(ITakeService takeService, IMapper mapper, ILogger<TakeController> logger)
        {
            _takeService = takeService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Gets a list of takes that belong to the associated property id.
        /// </summary>
        /// <returns></returns>
        [HttpGet("acquisition/{fileId:long}")]
        [HasPermission(Permissions.AcquisitionFileView, Permissions.PropertyView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TakeModel), 200)]
        [SwaggerOperation(Tags = new[] { "take" })]
        public IActionResult GetTakesByAcquisitionFileId(long fileId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(TakeController),
                nameof(GetTakesByAcquisitionFileId),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _takeService.GetType());

            var takes = _takeService.GetByFileId(fileId);
            return new JsonResult(_mapper.Map<IEnumerable<TakeModel>>(takes));
        }

        /// <summary>
        /// Get all takes for a property in the Acquisition File.
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="acquisitionFilePropertyId"></param>
        /// <returns></returns>
        [HttpGet("acquisition/{fileId:long}/property/{acquisitionFilePropertyId:long}")]
        [HasPermission(Permissions.AcquisitionFileView, Permissions.PropertyView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TakeModel), 200)]
        [SwaggerOperation(Tags = new[] { "take" })]
        public IActionResult GetTakesByPropertyId([FromRoute] long fileId, [FromRoute] long acquisitionFilePropertyId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(TakeController),
                nameof(GetTakesByAcquisitionFileId),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _takeService.GetType());

            var takes = _takeService.GetByPropertyId(fileId, acquisitionFilePropertyId);
            return new JsonResult(_mapper.Map<IEnumerable<TakeModel>>(takes));
        }

        /// <summary>
        /// Update the list of takes associated to a property within an acquisition file.
        /// </summary>
        /// <returns></returns>
        [HttpPut("acquisition/property/{acquisitionFilePropertyId:long}")]
        [HasPermission(Permissions.AcquisitionFileEdit, Permissions.PropertyEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TakeModel), 200)]
        [SwaggerOperation(Tags = new[] { "take" })]
        public IActionResult UpdateAcquisitionPropertyTakes(long acquisitionFilePropertyId, [FromBody] IEnumerable<TakeModel> takes)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(TakeController),
                nameof(UpdateAcquisitionPropertyTakes),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _takeService.GetType());

            var updatedTakes = _takeService.UpdateAcquisitionPropertyTakes(acquisitionFilePropertyId, _mapper.Map<IEnumerable<PimsTake>>(takes));
            return new JsonResult(_mapper.Map<IEnumerable<TakeModel>>(updatedTakes));
        }

        /// <summary>
        /// Gets a count of takes that that match a property.
        /// </summary>
        /// <returns></returns>
        [HttpGet("property/{propertyId:long}/count")]
        [HasPermission(Permissions.AcquisitionFileView, Permissions.PropertyView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TakeModel), 200)]
        [SwaggerOperation(Tags = new[] { "take" })]
        public IActionResult GetTakesCountByPropertyId([FromRoute] long propertyId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(TakeController),
                nameof(GetTakesCountByPropertyId),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _takeService.GetType());

            var count = _takeService.GetCountByPropertyId(propertyId);
            return new JsonResult(count);
        }

        #endregion
    }
}
