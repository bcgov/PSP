using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Areas.Lease.Models.Lease;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models;
using Pims.Api.Policies;
using Pims.Dal.Entities;
using Pims.Dal.Security;
using Pims.Dal.Services;
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
        private readonly IPimsService _pimsService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a SecurityDepositReturnController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="pimsService"></param>
        /// <param name="mapper"></param>
        ///
        public SecurityDepositReturnController(IPimsService pimsService, IMapper mapper)
        {
            _pimsService = pimsService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints
        /// <summary>
        /// Adds the specified return deposit to the lease.
        /// </summary>
        /// <returns></returns>
        [HttpPost("{leaseId:long}/deposit-returns")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<LeaseModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        public IActionResult AddDepositReturn(long leaseId, [FromBody] ParentConcurrencyGuardModel<SecurityDepositReturnModel> addRequest)
        {
            if (leaseId != addRequest.ParentId)
            {
                throw new BadRequestException($"Concurrency parent id mismatch.");
            }
            var depositEntity = _mapper.Map<PimsSecurityDepositReturn>(addRequest.Payload);
            depositEntity.LeaseId = leaseId;

            var updatedLease = _pimsService.SecurityDepositReturnService.AddLeaseDepositReturn(addRequest.ParentId, addRequest.ParentRowVersion, depositEntity);

            return new JsonResult(_mapper.Map<LeaseModel>(updatedLease));
        }

        /// <summary>
        /// Update the specified return deposit on the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{leaseId:long}/deposit-returns/{depositId:long}")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<LeaseModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        public IActionResult UpdateDepositReturn(long leaseId, long depositId, ParentConcurrencyGuardModel<SecurityDepositReturnModel> updateRequest)
        {
            if (leaseId != updateRequest.ParentId)
            {
                throw new BadRequestException($"Concurrency parent id mismatch.");
            }

            if (depositId != updateRequest.Payload.Id)
            {
                throw new BadRequestException($"Bad payload id.");
            }

            var depositEntity = _mapper.Map<PimsSecurityDepositReturn>(updateRequest.Payload);
            depositEntity.LeaseId = leaseId;
            var updatedLease = _pimsService.SecurityDepositReturnService.UpdateLeaseDepositReturn(updateRequest.ParentId, updateRequest.ParentRowVersion, depositEntity);

            return new JsonResult(_mapper.Map<LeaseModel>(updatedLease));
        }

        /// <summary>
        /// Delete the specified return deposit on the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{leaseId:long}/deposit-returns")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<LeaseModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        public IActionResult DeleteDepositReturn(long leaseId, [FromBody] ParentConcurrencyGuardModel<SecurityDepositReturnModel> deleteRequest)
        {
            if (leaseId != deleteRequest.ParentId)
            {
                throw new BadRequestException($"Concurrency parent id mismatch.");
            }
            var depositEntity = _mapper.Map<PimsSecurityDepositReturn>(deleteRequest.Payload);
            var updatedLease = _pimsService.SecurityDepositReturnService.DeleteLeaseDepositReturn(deleteRequest.ParentId, deleteRequest.ParentRowVersion, depositEntity);

            return new JsonResult(_mapper.Map<LeaseModel>(updatedLease));
        }
        #endregion
    }
}
