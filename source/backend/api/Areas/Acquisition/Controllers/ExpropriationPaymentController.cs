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
using Pims.Core.Json;
using Pims.Dal.Entities;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Acquisition.Controllers
{
    /// <summary>
    /// Form8Controller class, provides endpoints for Acquisition File's form 8's.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("acquisitionfiles")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class ExpropriationPaymentController : ControllerBase
    {
        private readonly IAcquisitionFileService _acquisitionFileService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ExpropriationPaymentController(IAcquisitionFileService acquisitionService, IMapper mapper, ILogger<ExpropriationPaymentController> logger)
        {
            _acquisitionFileService = acquisitionService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get all Expropriation Payments from the Acquisition File.
        /// </summary>
        /// <param name="id">Acquisition File Id.</param>
        /// <returns>List of all Expropriation Payments created for the acquisition file.</returns>
        [HttpGet("{id:long}/expropriation-payment")]
        [HasPermission(Permissions.AcquisitionFileView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<ExpropriationPaymentModel>), 200)]
        [SwaggerOperation(Tags = new[] { "expropriation-payment" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetAcquisitionFileExpropriationPayments([FromRoute] long id)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(ExpropriationPaymentController),
                nameof(GetAcquisitionFileExpropriationPayments),
                User.GetUsername(),
                DateTime.Now);
            _logger.LogInformation($"Dispatching to service: {_acquisitionFileService.GetType()}");

            var pimsForm8s = _acquisitionFileService.GetAcquisitionExpropriationPayments(id);
            var form8s = _mapper.Map<List<ExpropriationPaymentModel>>(pimsForm8s);

            return new JsonResult(form8s);
        }

        /// <summary>
        /// Creates a new Form8 for the acquistion file.
        /// </summary>
        /// <param name="id">Acquisition File Id.</param>
        /// <param name="expropriationPayment">Form8 Data Model.</param>
        /// <returns></returns>
        [HttpPost("{id:long}/expropriation-payment")]
        [HasPermission(Permissions.AcquisitionFileEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ExpropriationPaymentModel), 201)]
        [SwaggerOperation(Tags = new[] { "expropriation-payment" })]
        public IActionResult AddExpropriationPayment([FromRoute]long id, [FromBody]ExpropriationPaymentModel expropriationPayment)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(ExpropriationPaymentController),
                nameof(AddExpropriationPayment),
                User.GetUsername(),
                DateTime.Now);
            _logger.LogInformation($"Dispatching to service: {_acquisitionFileService.GetType()}");

            var expPaymentEntity = _mapper.Map<PimsExpropriationPayment>(expropriationPayment);
            var newExpPaymentEntity = _acquisitionFileService.AddExpropriationPayment(id, expPaymentEntity);

            return new JsonResult(_mapper.Map<ExpropriationPaymentModel>(newExpPaymentEntity));
        }
    }
}
