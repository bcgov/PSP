using System;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    /// SecurityDepositReturnController class, provides endpoints for interacting with lease security deposit returns.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("leases")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class SecurityDepositReturnController : ControllerBase
    {
        #region Variables
        private readonly ISecurityDepositReturnService _securityDepositReturnService;
        private readonly IMapper _mapper;
        private readonly ILogger<SecurityDepositReturnController> _logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a SecurityDepositReturnController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="securityDepositReturnService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        ///
        public SecurityDepositReturnController(ISecurityDepositReturnService securityDepositReturnService, IMapper mapper, ILogger<SecurityDepositReturnController> logger)
        {
            _securityDepositReturnService = securityDepositReturnService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Adds the specified return deposit to the lease.
        /// </summary>
        /// <returns></returns>
        [HttpPost("{leaseId:long}/deposits/{depositId:long}/returns")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(SecurityDepositReturnModel), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult AddDepositReturn(long leaseId, long depositId, [FromBody] Pims.Api.Models.Concepts.SecurityDepositReturnModel securityDepositReturnModel)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(SecurityDepositReturnController),
                nameof(AddDepositReturn),
                User.GetUsername(),
                DateTime.Now);

            if (depositId != securityDepositReturnModel.ParentDepositId)
            {
                throw new BadRequestException($"Bad payload id.");
            }

            var depositEntity = _mapper.Map<PimsSecurityDepositReturn>(securityDepositReturnModel);

            var depositReturn = _securityDepositReturnService.AddLeaseDepositReturn(leaseId, depositEntity);

            return new JsonResult(_mapper.Map<SecurityDepositReturnModel>(depositReturn));
        }

        /// <summary>
        /// Update the specified return deposit on the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{leaseId:long}/deposits/{depositId:long}/returns/{depositReturnId:long}")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(SecurityDepositReturnModel), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateDepositReturn(long leaseId, long depositId, long depositReturnId, Pims.Api.Models.Concepts.SecurityDepositReturnModel updateRequest)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(SecurityDepositReturnController),
                nameof(UpdateDepositReturn),
                User.GetUsername(),
                DateTime.Now);

            if (depositId != updateRequest.ParentDepositId || depositReturnId != updateRequest.Id)
            {
                throw new BadRequestException($"Bad payload id.");
            }

            var depositEntity = _mapper.Map<PimsSecurityDepositReturn>(updateRequest);
            var updatedDepositReturn = _securityDepositReturnService.UpdateLeaseDepositReturn(leaseId, depositEntity);

            return new JsonResult(_mapper.Map<SecurityDepositReturnModel>(updatedDepositReturn));
        }

        /// <summary>
        /// Delete the specified return deposit on the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{leaseId:long}/deposits/{depositId:long}/returns")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult DeleteDepositReturn(long leaseId, long depositId, [FromBody] Pims.Api.Models.Concepts.SecurityDepositReturnModel deleteRequest)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(SecurityDepositReturnController),
                nameof(DeleteDepositReturn),
                User.GetUsername(),
                DateTime.Now);

            if (depositId != deleteRequest.ParentDepositId)
            {
                throw new BadRequestException($"Bad payload id.");
            }

            var depositEntity = _mapper.Map<PimsSecurityDepositReturn>(deleteRequest);
            var updatedLease = _securityDepositReturnService.DeleteLeaseDepositReturn(leaseId, depositEntity);

            return new JsonResult(updatedLease);
        }
        #endregion
    }
}
