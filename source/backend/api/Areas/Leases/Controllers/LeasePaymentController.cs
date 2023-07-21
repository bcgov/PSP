using System;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILeasePaymentService _leasePaymentService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a LeasePaymentController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="leasePaymentService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        ///
        public LeasePaymentController(ILeasePaymentService leasePaymentService, IMapper mapper, ILogger<LeasePaymentController> logger)
        {
            _leasePaymentService = leasePaymentService;
            _mapper = mapper;
            _logger = logger;
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
        [ProducesResponseType(typeof(PaymentModel), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult AddPayment(long leaseId, [FromBody] PaymentModel paymentModel)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(LeaseTermController),
                nameof(AddPayment),
                User.GetUsername(),
                DateTime.Now);

            var paymentEntity = _mapper.Map<PimsLeasePayment>(paymentModel);
            var updatedLease = _leasePaymentService.AddPayment(leaseId, paymentEntity);

            return new JsonResult(_mapper.Map<PaymentModel>(updatedLease));
        }

        /// <summary>
        /// Update the specified payment on the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{leaseId:long}/payment/{paymentId:long}")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PaymentModel), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdatePayment(long leaseId, long paymentId, [FromBody] PaymentModel paymentModel)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(LeaseTermController),
                nameof(UpdatePayment),
                User.GetUsername(),
                DateTime.Now);

            var paymentEntity = _mapper.Map<PimsLeasePayment>(paymentModel);
            var updatedLease = _leasePaymentService.UpdatePayment(leaseId, paymentId, paymentEntity);

            return new JsonResult(_mapper.Map<PaymentModel>(updatedLease));
        }

        /// <summary>
        /// Delete the specified payment on the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{leaseId:long}/payment")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult DeletePayment(long leaseId, PaymentModel paymentModel)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(LeaseTermController),
                nameof(DeletePayment),
                User.GetUsername(),
                DateTime.Now);

            var paymentEntity = _mapper.Map<PimsLeasePayment>(paymentModel);
            var deleted = _leasePaymentService.DeletePayment(leaseId, paymentEntity);

            return new JsonResult(deleted);
        }
        #endregion
    }
}
