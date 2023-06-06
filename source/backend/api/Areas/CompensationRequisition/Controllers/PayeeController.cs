using System;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Concepts;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Extensions;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.CompensationRequisition.Controllers
{
    /// <summary>
    /// PayeeController class, provides endpoints to handle compensation requests.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("compensation-requisitions/payees")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class PayeeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly ICompensationRequisitionService _compensationRequisitionService;

        public PayeeController(IMapper mapper, ILogger<CompensationRequisitionController> logger, ICompensationRequisitionService compensationRequisitionService)
        {
            _mapper = mapper;
            _logger = logger;
            _compensationRequisitionService = compensationRequisitionService;
        }

        /// <summary>
        /// Get The Compensation Requisition Payee by payee id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:long}")]
        [HasPermission(Permissions.CompensationRequisitionView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CompensationPayeeModel), 200)]
        [SwaggerOperation(Tags = new[] { "compensation-requisition", "payee" })]
        public IActionResult GetPayeeById([FromRoute] long id)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(PayeeController),
                nameof(GetPayeeById),
                User.GetUsername(),
                DateTime.Now);
            _logger.LogInformation("Dispatching to service: {Service}", _compensationRequisitionService.GetType());

            var pimsAcquisitionPayee = _compensationRequisitionService.GetPayeeById(id);

            return new JsonResult(_mapper.Map<CompensationPayeeModel>(pimsAcquisitionPayee));
        }
    }
}
