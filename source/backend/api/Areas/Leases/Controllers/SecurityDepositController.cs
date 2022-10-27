using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Areas.Lease.Models.Lease;
using Pims.Api.Areas.Leases.Models.Lease;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models;
using Pims.Api.Models.Concepts;
using Pims.Api.Policies;
using Pims.Api.Services;
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
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a SecurityDepositController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="securityDepositService"></param>
        /// <param name="mapper"></param>
        ///
        public SecurityDepositController(ISecurityDepositService securityDepositService, IMapper mapper)
        {
            _securityDepositService = securityDepositService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Adds the specified deposit to the lease.
        /// </summary>
        /// <returns></returns>
        [HttpPost("{leaseId:long}/deposits")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<LeaseModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        public IActionResult AddDeposit(long leaseId, [FromBody] ParentConcurrencyGuardModel<SecurityDepositModel> addRequest)
        {
            if (leaseId != addRequest.ParentId)
            {
                throw new BadRequestException($"Concurrency parent id mismatch.");
            }
            var depositEntity = _mapper.Map<PimsSecurityDeposit>(addRequest.Payload);
            depositEntity.LeaseId = leaseId;

            var updatedLease = _securityDepositService.AddLeaseDeposit(addRequest.ParentId, addRequest.ParentRowVersion, depositEntity);

            return new JsonResult(_mapper.Map<LeaseModel>(updatedLease));
        }

        /// <summary>
        /// Update the specified deposit on the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{leaseId:long}/deposits/{depositId:long}")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<LeaseModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        public IActionResult UpdateDeposit(long leaseId, long depositId, ParentConcurrencyGuardModel<SecurityDepositModel> updateRequest)
        {
            if (leaseId != updateRequest.ParentId)
            {
                throw new BadRequestException($"Concurrency parent id mismatch.");
            }

            if (depositId != updateRequest.Payload.Id)
            {
                throw new BadRequestException($"Bad payload id.");
            }

            var depositEntity = _mapper.Map<PimsSecurityDeposit>(updateRequest.Payload);
            depositEntity.LeaseId = leaseId;

            var updatedLease = _securityDepositService.UpdateLeaseDeposit(updateRequest.ParentId, updateRequest.ParentRowVersion, depositEntity);

            return new JsonResult(_mapper.Map<LeaseModel>(updatedLease));
        }

        /// <summary>
        /// Delete the specified deposit on the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{leaseId:long}/deposits")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<LeaseModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        public IActionResult DeleteDeposit(long leaseId, [FromBody] ParentConcurrencyGuardModel<SecurityDepositModel> deleteRequest)
        {
            if (leaseId != deleteRequest.ParentId)
            {
                throw new BadRequestException($"Concurrency parent id mismatch.");
            }
            var depositEntity = _mapper.Map<PimsSecurityDeposit>(deleteRequest.Payload);
            var updatedLease = _securityDepositService.DeleteLeaseDeposit(deleteRequest.ParentId, deleteRequest.ParentRowVersion, depositEntity);

            return new JsonResult(_mapper.Map<LeaseModel>(updatedLease));
        }

        /// <summary>
        /// update the deposit note on the given lease.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{leaseId:long}/deposits/note")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<LeaseModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        public IActionResult DeleteDeposit(long leaseId, [FromBody] ParentConcurrencyGuardModel<DepositNoteModel> depositNoteModel)
        {
            var updatedLease = _securityDepositService.UpdateLeaseDepositNote(depositNoteModel.ParentId, depositNoteModel.ParentRowVersion, depositNoteModel.Payload.Note);

            return new JsonResult(_mapper.Map<LeaseModel>(updatedLease));
        }
        #endregion
    }
}
