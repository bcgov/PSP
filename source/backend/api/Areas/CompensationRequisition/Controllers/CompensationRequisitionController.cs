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

namespace Pims.Api.Areas.CompensationRequisition.Controllers
{
    /// <summary>
    /// CompensationController class, provides endpoints to handle compensation requests.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("compensation-requisitions")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class CompensationRequisitionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly ICompensationRequisitionService _compensationRequisitionService;

        public CompensationRequisitionController(IMapper mapper, ILogger<CompensationRequisitionController> logger, ICompensationRequisitionService compensationRequisitionService)
        {
            _mapper = mapper;
            _logger = logger;
            _compensationRequisitionService = compensationRequisitionService;
        }

        /// <summary>
        /// Get The Compensation Requisition by CompensationRequisitionId.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:long}")]
        [HasPermission(Permissions.CompensationRequisitionView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CompensationRequisitionModel), 200)]
        [SwaggerOperation(Tags = new[] { "compensation-requisition" })]
        public IActionResult GetCompensationRequisitionById([FromRoute] long id)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(CompensationRequisitionController),
                nameof(GetCompensationRequisitionById),
                User.GetUsername(),
                DateTime.Now);
            _logger.LogInformation("Dispatching to service: {Service}", _compensationRequisitionService.GetType());

            var compensationRequisition = _compensationRequisitionService.GetById(id);

            return new JsonResult(_mapper.Map<CompensationRequisitionModel>(compensationRequisition));
        }

        [HttpPut("{id:long}")]
        [HasPermission(Permissions.CompensationRequisitionEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CompensationRequisitionModel), 200)]
        [SwaggerOperation(Tags = new[] { "compensation-requisition" })]
        public IActionResult UpdateCompensationRequisition([FromRoute] long id, [FromBody] CompensationRequisitionModel compensationRequisition)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(CompensationRequisitionController),
                nameof(UpdateCompensationRequisition),
                User.GetUsername(),
                DateTime.Now);
            _logger.LogInformation("Dispatching to service: {Service}", _compensationRequisitionService.GetType());

            if (id != compensationRequisition.Id)
            {
                throw new BadRequestException("Invalid compensationRequisitionId.");
            }

            var compensationReqEntity = _mapper.Map<Dal.Entities.PimsCompensationRequisition>(compensationRequisition);
            var compensation = _compensationRequisitionService.Update(compensationReqEntity);

            return new JsonResult(_mapper.Map<CompensationRequisitionModel>(compensation));
        }

        /// <summary>
        /// Deletes the compensation with the matching id.
        /// </summary>
        /// <param name="id">Used to identify the compensation and delete it.</param>
        /// <returns></returns>
        [HttpDelete("{id:long}")]
        [Produces("application/json")]
        [HasPermission(Permissions.CompensationRequisitionDelete)]
        [ProducesResponseType(typeof(bool), 200)]
        [SwaggerOperation(Tags = new[] { "compensation-requisition" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult DeleteCompensation([FromRoute] long id)
        {
            var result = _compensationRequisitionService.DeleteCompensation(id);
            return new JsonResult(result);
        }
    }
}
