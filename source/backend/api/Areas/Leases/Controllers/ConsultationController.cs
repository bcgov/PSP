
using System;
using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models.Concepts.Lease;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Extensions;
using Pims.Core.Json;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Lease.Controllers
{
    /// <summary>
    /// ConsultationController class, provides endpoints for interacting with lease files consultation.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("leases")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class ConsultationController : ControllerBase
    {
        #region Variables
        private readonly ILeaseService _leaseService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a ConsultationController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="leaseService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public ConsultationController(ILeaseService leaseService, IMapper mapper, ILogger<ConsultationController> logger)
        {
            _leaseService = leaseService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Get the lease file consultation.
        /// </summary>
        /// <returns>The consultation items.</returns>
        [HttpGet("{id:long}/consultations")]
        [HasPermission(Permissions.LeaseView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<ConsultationLeaseModel>), 200)]
        [SwaggerOperation(Tags = new[] { "leasefile" })]
        public IActionResult GetLeaseConsultations([FromRoute] long id)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(ConsultationController),
                nameof(GetLeaseConsultations),
                User.GetUsername(),
                DateTime.Now);


            var consultation = _leaseService.GetConsultations(id);
            return new JsonResult(_mapper.Map<IEnumerable<ConsultationLeaseModel>>(consultation));
        }

        /// <summary>
        /// Create the lease file consultation to the lease file.
        /// </summary>
        /// <returns>The consultation items.</returns>
        [HttpPost("{leaseId:long}/consultations")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ConsultationLeaseModel), 201)]
        [TypeFilter(typeof(NullJsonResultFilter))]
        [SwaggerOperation(Tags = new[] { "leasefile" })]
        public IActionResult AddLeaseConsultation([FromRoute] long leaseId, [FromBody] ConsultationLeaseModel consultation)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(ConsultationController),
                nameof(AddLeaseConsultation),
                User.GetUsername(),
                DateTime.Now);

            if (leaseId != consultation.LeaseId)
            {
                throw new BadRequestException("Invalid LeaseId.");
            }

            var newConsultation = _leaseService.AddConsultation(_mapper.Map<Dal.Entities.PimsLeaseConsultation>(consultation));

            return new JsonResult(_mapper.Map<ConsultationLeaseModel>(newConsultation));
        }

        /// <summary>
        /// Get the lease file consultation by Id.
        /// </summary>
        /// <returns>The consultation items.</returns>
        [HttpGet("{leaseId:long}/consultations/{consultationId:long}")]
        [HasPermission(Permissions.LeaseView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ConsultationLeaseModel), 200)]
        [SwaggerOperation(Tags = new[] { "leasefile" })]
        public IActionResult GetLeaseConsultationById([FromRoute] long leaseId, [FromRoute] long consultationId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(ConsultationController),
                nameof(GetLeaseConsultationById),
                User.GetUsername(),
                DateTime.Now);

            var consultation = _leaseService.GetConsultationById(consultationId);

            if (consultation.LeaseId != leaseId)
            {
                throw new BadRequestException("Invalid lease id for the given consultation.");
            }

            return new JsonResult(_mapper.Map<ConsultationLeaseModel>(consultation));
        }

        /// <summary>
        /// Update the lease file consultation by Id.
        /// </summary>
        /// <returns>The consultation item updated.</returns>
        [HttpPut("{leaseId:long}/consultations/{consultationId:long}")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ConsultationLeaseModel), 200)]
        [TypeFilter(typeof(NullJsonResultFilter))]
        [SwaggerOperation(Tags = new[] { "leasefile" })]
        public IActionResult UpdateLeaseConsultation([FromRoute] long leaseId, [FromRoute] long consultationId, [FromBody] ConsultationLeaseModel consultation)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(ConsultationController),
                nameof(UpdateLeaseConsultation),
                User.GetUsername(),
                DateTime.Now);

            if (leaseId != consultation.LeaseId)
            {
                throw new BadRequestException("Invalid LeaseId.");
            }
            if (consultationId != consultation.Id)
            {
                throw new BadRequestException("Invalid consultationId.");
            }

            var updatedConsultation = _leaseService.UpdateConsultation(_mapper.Map<Dal.Entities.PimsLeaseConsultation>(consultation));

            return new JsonResult(_mapper.Map<ConsultationLeaseModel>(updatedConsultation));
        }

        /// <summary>
        /// Delete the lease file consultation by Id.
        /// </summary>
        /// <returns>The consultation item updated.</returns>
        [HttpDelete("{leaseId:long}/consultations/{consultationId:long}")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), 200)]
        [SwaggerOperation(Tags = new[] { "leasefile" })]
        public IActionResult DeleteLeaseConsultation([FromRoute] long leaseId, [FromRoute] long consultationId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(ConsultationController),
                nameof(DeleteLeaseConsultation),
                User.GetUsername(),
                DateTime.Now);

            var existingConsultation = _leaseService.GetConsultationById(consultationId);
            if (existingConsultation.LeaseId != leaseId)
            {
                throw new BadRequestException("Invalid lease id for the given consultation.");
            }

            var result = _leaseService.DeleteConsultation(consultationId);

            return new JsonResult(result);
        }

        #endregion
    }
}
