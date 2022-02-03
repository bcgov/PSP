using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Policies;
using Pims.Dal.Entities;
using Pims.Dal.Security;
using Pims.Dal.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

namespace Pims.Api.Areas.Lease.Controllers
{
    /// <summary>
    /// LeasePaymentController class, provides endpoints for interacting with lease payment improvements.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("leases")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class LeasePaymentController : ControllerBase
    {
        #region Variables
        private readonly IPimsService _pimsService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a LeasePaymentController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="pimsService"></param>
        /// <param name="mapper"></param>
        ///
        public LeasePaymentController(IPimsService pimsService, IMapper mapper)
        {
            _pimsService = pimsService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints
        /// <summary>
        /// Update the specified payment on the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpPost("{leaseId:long}/payment")]
        [HasPermission(Permissions.LeaseAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Models.Lease.LeaseModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        public IActionResult AddPayment(long leaseId, [FromBody] Models.Lease.PaymentModel paymentModel)
        {
            var paymentEntity = _mapper.Map<PimsLeasePayment>(paymentModel);
            var updatedLease = _pimsService.LeasePaymentService.AddPayment(leaseId, paymentModel.LeaseRowVersion, paymentEntity);

            return new JsonResult(_mapper.Map<Models.Lease.LeaseModel>(updatedLease));
        }

        /// <summary>
        /// Update the specified payment on the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{leaseId:long}/payment/{paymentId:long}")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Models.Lease.LeaseModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        public IActionResult UpdatePayment(long leaseId, long paymentId, [FromBody] Models.Lease.PaymentModel paymentModel)
        {
            var paymentEntity = _mapper.Map<PimsLeasePayment>(paymentModel);
            var updatedLease = _pimsService.LeasePaymentService.UpdatePayment(leaseId, paymentId, paymentModel.LeaseRowVersion, paymentEntity);

            return new JsonResult(_mapper.Map<Models.Lease.LeaseModel>(updatedLease));
        }

        /// <summary>
        /// Delete the specified payment on the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{leaseId:long}/payment")]
        [HasPermission(Permissions.LeaseDelete)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Models.Lease.LeaseModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        public IActionResult DeletePayment(long leaseId, Models.Lease.PaymentModel paymentModel)
        {
            var paymentEntity = _mapper.Map<PimsLeasePayment>(paymentModel);
            var updatedLease = _pimsService.LeasePaymentService.DeletePayment(leaseId, paymentModel.LeaseRowVersion, paymentEntity);

            return new JsonResult(_mapper.Map<Models.Lease.LeaseModel>(updatedLease));
        }
        #endregion
    }
}
