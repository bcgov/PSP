using System;
using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Areas.Leases.Models.Lease;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models.Concepts;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Extensions;
using Pims.Core.Json;
using Pims.Dal.Entities;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Lease.Controllers
{
    /// <summary>
    /// SecurityDepositController class, provides endpoints for interacting with lease security deposits.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("leases")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class SecurityDepositController : ControllerBase
    {
        #region Variables
        private readonly ISecurityDepositService _securityDepositService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a SecurityDepositController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="securityDepositService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        ///
        public SecurityDepositController(ISecurityDepositService securityDepositService, IMapper mapper, ILogger<SecurityDepositController> logger)
        {
            _securityDepositService = securityDepositService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Gets a list of deposits from the passed lease id.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{leaseId:long}/deposits")]
        [HasPermission(Permissions.LeaseView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<SecurityDepositModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetDeposits(long leaseId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(SecurityDepositController),
                nameof(GetDeposits),
                User.GetUsername(),
                DateTime.Now);

            var updatedLease = _securityDepositService.GetLeaseDeposits(leaseId);

            return new JsonResult(_mapper.Map<IEnumerable<SecurityDepositModel>>(updatedLease));
        }

        /// <summary>
        /// Adds the specified deposit to the lease.
        /// </summary>
        /// <returns></returns>
        [HttpPost("{leaseId:long}/deposits")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(SecurityDepositModel), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult AddDeposit(long leaseId, [FromBody] Pims.Api.Models.Concepts.SecurityDepositModel addRequest)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(SecurityDepositController),
                nameof(AddDeposit),
                User.GetUsername(),
                DateTime.Now);

            if (leaseId != addRequest.LeaseId)
            {
                throw new BadRequestException($"Invalid Security Deposit Lease Id");
            }

            var depositEntity = _mapper.Map<PimsSecurityDeposit>(addRequest);
            depositEntity.LeaseId = leaseId;

            var updatedLease = _securityDepositService.AddLeaseDeposit(addRequest.LeaseId, depositEntity);

            return new JsonResult(_mapper.Map<SecurityDepositModel>(updatedLease));
        }

        /// <summary>
        /// Update the specified deposit on the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{leaseId:long}/deposits/{depositId:long}")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(SecurityDepositModel), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateDeposit(long leaseId, long depositId, Pims.Api.Models.Concepts.SecurityDepositModel updateRequest)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(SecurityDepositController),
                nameof(UpdateDeposit),
                User.GetUsername(),
                DateTime.Now);

            if (leaseId != updateRequest.LeaseId)
            {
                throw new BadRequestException($"Invalid Security Deposit Lease Id");
            }

            if (depositId != updateRequest.Id)
            {
                throw new BadRequestException($"Invalid Security Deposit Id");
            }
            var depositEntity = _mapper.Map<PimsSecurityDeposit>(updateRequest);
            depositEntity.LeaseId = leaseId;

            var updatedLease = _securityDepositService.UpdateLeaseDeposit(leaseId, depositEntity);

            return new JsonResult(_mapper.Map<SecurityDepositModel>(updatedLease));
        }

        /// <summary>
        /// Delete the specified deposit on the passed lease.
        /// </summary>
        [HttpDelete("{leaseId:long}/deposits/{depositId:long}")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public bool DeleteDeposit(long leaseId, long depositId, [FromBody] Pims.Api.Models.Concepts.SecurityDepositModel deleteRequest)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(SecurityDepositController),
                nameof(DeleteDeposit),
                User.GetUsername(),
                DateTime.Now);

            if (leaseId != deleteRequest.LeaseId)
            {
                throw new BadRequestException($"Invalid Security Deposit Lease Id");
            }

            if (depositId != deleteRequest.Id)
            {
                throw new BadRequestException($"Invalid Security Deposit Id");
            }
            var depositEntity = _mapper.Map<PimsSecurityDeposit>(deleteRequest);
            return _securityDepositService.DeleteLeaseDeposit(depositEntity);
        }

        /// <summary>
        /// update the deposit note on the given lease.
        /// </summary>
        [HttpPut("{leaseId:long}/deposits/note")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public void UpdateDepositNote(long leaseId, [FromBody] DepositNoteModel depositNoteModel)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(SecurityDepositController),
                nameof(UpdateDepositNote),
                User.GetUsername(),
                DateTime.Now);

            _securityDepositService.UpdateLeaseDepositNote(leaseId, depositNoteModel.Note);
        }
        #endregion
    }
}
