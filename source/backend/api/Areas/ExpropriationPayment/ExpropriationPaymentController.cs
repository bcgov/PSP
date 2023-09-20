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
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.ExpropriationPayment
{
    /// <summary>
    /// ExpropriationPaymentController class, provides endpoints to handle expropriation payments.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("expropriation-payments")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class ExpropriationPaymentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IExpropriationPaymentService _expropriationPaymentService;

        public ExpropriationPaymentController(IMapper mapper, ILogger<ExpropriationPaymentController> logger, IExpropriationPaymentService expropriationPaymentService)
        {
            _mapper = mapper;
            _logger = logger;
            _expropriationPaymentService = expropriationPaymentService;
        }

        /// <summary>
        /// Get The Expropriation Payment (Form8) by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:long}")]
        [HasPermission(Permissions.AcquisitionFileView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ExpropriationPaymentModel), 200)]
        [SwaggerOperation(Tags = new[] { "expropriation-payments" })]
        public IActionResult GetExpropriationPaymentById([FromRoute] long id)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(ExpropriationPaymentController),
                nameof(GetExpropriationPaymentById),
                User.GetUsername(),
                DateTime.Now);
            _logger.LogInformation("Dispatching to service: {Service}", _expropriationPaymentService.GetType());

            var expropriationPayment = _expropriationPaymentService.GetById(id);

            return new JsonResult(_mapper.Map<ExpropriationPaymentModel>(expropriationPayment));
        }

        /// <summary>
        /// Update the Expropriation Payment instance.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="expropriationPayment"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException">Validate id matching.</exception>
        [HttpPut("{id:long}")]
        [HasPermission(Permissions.AcquisitionFileEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ExpropriationPaymentModel), 200)]
        [SwaggerOperation(Tags = new[] { "expropriation-payments" })]
        public IActionResult UpdateExpropriationPayment([FromRoute] long id, [FromBody] ExpropriationPaymentModel expropriationPayment)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(ExpropriationPaymentController),
                nameof(UpdateExpropriationPayment),
                User.GetUsername(),
                DateTime.Now);
            _logger.LogInformation("Dispatching to service: {Service}", _expropriationPaymentService.GetType());

            if (id != expropriationPayment.Id)
            {
                throw new BadRequestException("Invalid expropriationPaymentId.");
            }

            var expPaymentEntity = _mapper.Map<Dal.Entities.PimsExpropriationPayment>(expropriationPayment);
            var compensation = _expropriationPaymentService.Update(expPaymentEntity);

            return new JsonResult(_mapper.Map<ExpropriationPaymentModel>(compensation));
        }

        /// <summary>
        /// Deletes the Expropriation Payment with the matching id.
        /// </summary>
        /// <param name="id">Used to identify the entity to delete.</param>
        /// <returns></returns>
        [HttpDelete("{id:long}")]
        [Produces("application/json")]
        [HasPermission(Permissions.AcquisitionFileEdit)]
        [ProducesResponseType(typeof(bool), 200)]
        [SwaggerOperation(Tags = new[] { "expropriation-payments" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult DeleteExpropriationPayment([FromRoute] long id)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(ExpropriationPaymentController),
                nameof(DeleteExpropriationPayment),
                User.GetUsername(),
                DateTime.Now);
            _logger.LogInformation($"Dispatching to service: {_expropriationPaymentService.GetType()}");

            var result = _expropriationPaymentService.Delete(id);
            return new JsonResult(result);
        }
    }
}
