using System;
using System.Collections.Generic;
using System.Linq;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Core.Api.Exceptions;
using Pims.Api.Models.Concepts.Take;
using Pims.Core.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Extensions;
using Pims.Core.Json;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Core.Security;
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
        [ProducesResponseType(typeof(IEnumerable<TakeModel>), 200)]
        [SwaggerOperation(Tags = new[] { "take" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
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
        [ProducesResponseType(typeof(IEnumerable<TakeModel>), 200)]
        [SwaggerOperation(Tags = new[] { "take" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
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
        /// Add the passed take to the acquisition property with the given id.
        /// </summary>
        /// <returns></returns>
        [HttpPost("acquisition/property/{acquisitionFilePropertyId:long}/takes")]
        [HasPermission(Permissions.AcquisitionFileEdit, Permissions.PropertyEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TakeModel), 201)]
        [SwaggerOperation(Tags = new[] { "take" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult AddAcquisitionPropertyTake(long acquisitionFilePropertyId, [FromBody] TakeModel take)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(TakeController),
                nameof(AddAcquisitionPropertyTake),
                User.GetUsername(),
                DateTime.Now);

            if (acquisitionFilePropertyId != take.PropertyAcquisitionFileId)
            {
                throw new BadRequestException("Invalid acquisition file property id.");
            }

            _logger.LogInformation("Dispatching to service: {Service}", _takeService.GetType());

            var addedTake = _takeService.AddAcquisitionPropertyTake(acquisitionFilePropertyId, _mapper.Map<PimsTake>(take));
            return new JsonResult(_mapper.Map<TakeModel>(addedTake));
        }

        /// <summary>
        /// Update a take with the given take and acquisition file property id with the passed take.
        /// </summary>
        /// <returns></returns>
        [HttpPut("acquisition/property/{acquisitionFilePropertyId:long}/takes/{takeId:long}")]
        [HasPermission(Permissions.AcquisitionFileEdit, Permissions.PropertyEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TakeModel), 200)]
        [SwaggerOperation(Tags = new[] { "take" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateAcquisitionPropertyTake(long acquisitionFilePropertyId, long takeId, [FromBody] TakeModel take)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(TakeController),
                nameof(UpdateAcquisitionPropertyTake),
                User.GetUsername(),
                DateTime.Now);

            if (acquisitionFilePropertyId != take.PropertyAcquisitionFileId)
            {
                throw new BadRequestException("Invalid acquisition file property id.");
            }
            else if (takeId != take.Id)
            {
                throw new BadRequestException("Invalid take id.");
            }

            _logger.LogInformation("Dispatching to service: {Service}", _takeService.GetType());

            var updatedTake = _takeService.UpdateAcquisitionPropertyTake(acquisitionFilePropertyId, _mapper.Map<PimsTake>(take));
            return new JsonResult(_mapper.Map<TakeModel>(updatedTake));
        }

        /// <summary>
        /// Delete a take with the given take id and acquisition file property id.
        /// </summary>
        [HttpDelete("acquisition/property/{acquisitionFilePropertyId:long}/takes/{takeId:long}")]
        [HasPermission(Permissions.AcquisitionFileEdit, Permissions.PropertyEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), 200)]
        [SwaggerOperation(Tags = new[] { "take" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public void DeleteAcquisitionPropertyTake(long acquisitionFilePropertyId, long takeId, [FromQuery] string[] userOverrideCodes)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(TakeController),
                nameof(DeleteAcquisitionPropertyTake),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _takeService.GetType());
            var existingTake = _takeService.GetById(takeId);
            if (existingTake.PropertyAcquisitionFileId != acquisitionFilePropertyId)
            {
                throw new BadRequestException("Invalid acquisition file property id.");
            }
            var deleted = _takeService.DeleteAcquisitionPropertyTake(takeId, userOverrideCodes.Select(oc => UserOverrideCode.Parse(oc)));
            if (!deleted)
            {
                throw new InvalidOperationException($"Failed to delete take {takeId}.");
            }
        }

        /// <summary>
        /// Gets a count of takes that that match a property.
        /// </summary>
        /// <returns></returns>
        [HttpGet("property/{propertyId:long}/count")]
        [HasPermission(Permissions.AcquisitionFileView, Permissions.PropertyView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(int), 200)]
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

        /// <summary>
        /// GGet a take by id.
        /// </summary>
        /// <returns></returns>
        [HttpGet("acquisition/property/{acquisitionFilePropertyId:long}/takes/{takeId:long}")]
        [HasPermission(Permissions.AcquisitionFileView, Permissions.PropertyView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(int), 200)]
        [SwaggerOperation(Tags = new[] { "take" })]
        public IActionResult GetTakeByPropertyFileId(long acquisitionFilePropertyId, long takeId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(TakeController),
                nameof(GetTakesCountByPropertyId),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _takeService.GetType());

            var take = _takeService.GetById(takeId);
            if(take.PropertyAcquisitionFileId != acquisitionFilePropertyId)
            {
                throw new BadRequestException("Invalid acquisition file property id.");
            }
            return new JsonResult(_mapper.Map<TakeModel>(take));
        }

        #endregion
    }
}
