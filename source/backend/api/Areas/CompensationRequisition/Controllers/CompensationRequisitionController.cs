using System;
using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Models.Concepts.AcquisitionFile;
using Pims.Api.Models.Concepts.CompensationRequisition;
using Pims.Api.Models.Concepts.Lease;
using Pims.Api.Services;
using Pims.Core.Api.Exceptions;
using Pims.Core.Api.Policies;
using Pims.Core.Extensions;
using Pims.Core.Json;
using Pims.Core.Security;
using Pims.Dal.Entities;
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
        [TypeFilter(typeof(NullJsonResultFilter))]
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

        [HttpPost("{fileType}")]
        [HasPermission(Permissions.CompensationRequisitionAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CompensationRequisitionModel), 201)]
        [SwaggerOperation(Tags = new[] { "compensation-requisition" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult AddFileCompensationRequisition([FromRoute] FileTypes fileType, [FromBody] CompensationRequisitionModel compensationRequisition)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(CompensationRequisitionController),
                nameof(AddFileCompensationRequisition),
                User.GetUsername(),
                DateTime.Now);
            _logger.LogInformation($"Dispatching to service: {_compensationRequisitionService.GetType()}");

            var compensationReqEntity = _mapper.Map<PimsCompensationRequisition>(compensationRequisition);
            var newCompensationRequisition = _compensationRequisitionService.AddCompensationRequisition(fileType, compensationReqEntity);

            return new JsonResult(_mapper.Map<CompensationRequisitionModel>(newCompensationRequisition));
        }

        [HttpPut("{fileType}/{id:long}")]
        [HasPermission(Permissions.CompensationRequisitionEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CompensationRequisitionModel), 200)]
        [SwaggerOperation(Tags = new[] { "compensation-requisition" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateCompensationRequisition([FromRoute] FileTypes fileType, [FromRoute] long id, [FromBody] CompensationRequisitionModel compensationRequisition)
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

            var compensationReqEntity = _mapper.Map<PimsCompensationRequisition>(compensationRequisition);
            var compensation = _compensationRequisitionService.Update(fileType, compensationReqEntity);

            return new JsonResult(_mapper.Map<CompensationRequisitionModel>(compensation));
        }

        [HttpGet("acquisition/{id:long}/properties")]
        [HasPermission(Permissions.CompensationRequisitionView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<AcquisitionFilePropertyModel>), 200)]
        [SwaggerOperation(Tags = new[] { "compensation-requisition" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetAcquisitionCompensationRequisitionProperties([FromRoute] long id)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(CompensationRequisitionController),
                nameof(GetAcquisitionCompensationRequisitionProperties),
                User.GetUsername(),
                DateTime.Now);
            _logger.LogInformation("Dispatching to service: {Service}", _compensationRequisitionService.GetType());

            var compensationReqProperties = _compensationRequisitionService.GetAcquisitionProperties(id);

            return new JsonResult(_mapper.Map<IEnumerable<AcquisitionFilePropertyModel>>(compensationReqProperties));
        }

        [HttpGet("lease/{id:long}/properties")]
        [HasPermission(Permissions.CompensationRequisitionView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PropertyLeaseModel>), 200)]
        [SwaggerOperation(Tags = new[] { "compensation-requisition" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetLeaseCompensationRequisitionProperties([FromRoute] long id)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(CompensationRequisitionController),
                nameof(GetLeaseCompensationRequisitionProperties),
                User.GetUsername(),
                DateTime.Now);
            _logger.LogInformation("Dispatching to service: {Service}", _compensationRequisitionService.GetType());

            var compensationReqProperties = _compensationRequisitionService.GetLeaseProperties(id);

            return new JsonResult(_mapper.Map<IEnumerable<PropertyLeaseModel>>(compensationReqProperties));
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
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(CompensationRequisitionController),
                nameof(DeleteCompensation),
                User.GetUsername(),
                DateTime.Now);
            _logger.LogInformation($"Dispatching to service: {_compensationRequisitionService.GetType()}");

            var result = _compensationRequisitionService.DeleteCompensation(id);
            return new JsonResult(result);
        }

        [HttpGet("{fileType}/{fileId:long}")]
        [HasPermission(Permissions.CompensationRequisitionView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<CompensationRequisitionModel>), 200)]
        [SwaggerOperation(Tags = new[] { "compensation-requisition" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetFileCompensations([FromRoute] FileTypes fileType, [FromRoute] long fileId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(CompensationRequisitionController),
                nameof(GetFileCompensations),
                User.GetUsername(),
                DateTime.Now);
            _logger.LogInformation($"Dispatching to service: {_compensationRequisitionService.GetType()}");

            var pimsCompensations = _compensationRequisitionService.GetFileCompensationRequisitions(fileType, fileId);
            var compensations = _mapper.Map<List<CompensationRequisitionModel>>(pimsCompensations);

            return new JsonResult(compensations);
        }

        [HttpGet("{id:long}/financials")]
        [HasPermission(Permissions.CompensationRequisitionView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<CompensationFinancialModel>), 200)]
        [SwaggerOperation(Tags = new[] { "compensation-requisition" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetCompensationRequisitionFinancials([FromRoute] long id)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(CompensationRequisitionController),
                nameof(GetCompensationRequisitionFinancials),
                User.GetUsername(),
                DateTime.Now);
            _logger.LogInformation("Dispatching to service: {Service}", _compensationRequisitionService.GetType());

            var compReqFinancials = _compensationRequisitionService.GetCompensationRequisitionFinancials(id);

            return new JsonResult(_mapper.Map<IEnumerable<CompensationFinancialModel>>(compReqFinancials));
        }

        [HttpGet("{id:long}/acquisition-payees")]
        [HasPermission(Permissions.CompensationRequisitionView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<CompReqAcqPayeeModel>), 200)]
        [SwaggerOperation(Tags = new[] { "compensation-requisition" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetCompensationRequisitionAcquisitionPayees([FromRoute] long id)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(CompensationRequisitionController),
                nameof(GetCompensationRequisitionAcquisitionPayees),
                User.GetUsername(),
                DateTime.Now);
            _logger.LogInformation("Dispatching to service: {Service}", _compensationRequisitionService.GetType());

            var compReqPayees = _compensationRequisitionService.GetCompensationRequisitionAcquisitionPayees(id);

            return new JsonResult(_mapper.Map<IEnumerable<CompReqAcqPayeeModel>>(compReqPayees));
        }

        [HttpGet("{id:long}/lease-payees")]
        [HasPermission(Permissions.CompensationRequisitionView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<CompReqLeasePayeeModel>), 200)]
        [SwaggerOperation(Tags = new[] { "compensation-requisition" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetCompensationRequisitionLeasePayees([FromRoute] long id)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(CompensationRequisitionController),
                nameof(GetCompensationRequisitionLeasePayees),
                User.GetUsername(),
                DateTime.Now);
            _logger.LogInformation("Dispatching to service: {Service}", _compensationRequisitionService.GetType());

            var compReqPayees = _compensationRequisitionService.GetCompensationRequisitionLeasePayees(id);

            return new JsonResult(_mapper.Map<IEnumerable<CompReqLeasePayeeModel>>(compReqPayees));
        }

        [HttpGet("{id:long}/historical")]
        [HasPermission(Permissions.CompensationRequisitionView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<CompensationRequisitionModel>), 200)]
        [SwaggerOperation(Tags = new[] { "compensation-requisition" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetCompensationRequisionAtTime([FromRoute] long id, [FromQuery] DateTime time)
        {
            _logger.LogInformation(
               "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
               nameof(CompensationRequisitionController),
               nameof(GetCompensationRequisionAtTime),
               User.GetUsername(),
               DateTime.Now);

            var histCompReq = _compensationRequisitionService.GetCompensationRequisitionAtTime(id, time);

            return new JsonResult(_mapper.Map<CompensationRequisitionModel>(histCompReq));
        }

        [HttpGet("acquisition/{id:long}/properties/historical")]
        [HasPermission(Permissions.CompensationRequisitionView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<AcquisitionFilePropertyModel>), 200)]
        [SwaggerOperation(Tags = new[] { "compensation-requisition" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetAcquisitionCompensationRequisitionPropertiesAtTime([FromRoute] long id, [FromQuery] DateTime time)
        {
            _logger.LogInformation(
               "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
               nameof(CompensationRequisitionController),
               nameof(GetAcquisitionCompensationRequisitionPropertiesAtTime),
               User.GetUsername(),
               DateTime.Now);

            var acqCompReqProperties = _compensationRequisitionService.GetCompensationRequisitionAcqPropertiesAtTime(id, time);

            return new JsonResult(_mapper.Map<IEnumerable<AcquisitionFilePropertyModel>>(acqCompReqProperties));
        }

        [HttpGet("lease/{id:long}/properties/historical")]
        [HasPermission(Permissions.CompensationRequisitionView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PropertyLeaseModel>), 200)]
        [SwaggerOperation(Tags = new[] { "compensation-requisition" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetLeaseCompensationRequisitionPropertiesAtTime([FromRoute] long id, [FromQuery] DateTime time)
        {
            _logger.LogInformation(
               "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
               nameof(CompensationRequisitionController),
               nameof(GetLeaseCompensationRequisitionPropertiesAtTime),
               User.GetUsername(),
               DateTime.Now);

            var leaseCompReqProperties = _compensationRequisitionService.GetCompensationRequisitionLeasePropertiesAtTime(id, time);

            return new JsonResult(_mapper.Map<IEnumerable<PropertyLeaseModel>>(leaseCompReqProperties));
        }

        [HttpGet("{id:long}/acquisition-payees/historical")]
        [HasPermission(Permissions.CompensationRequisitionView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<CompReqAcqPayeeModel>), 200)]
        [SwaggerOperation(Tags = new[] { "compensation-requisition" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetCompensationRequisitionAcquisitionPayees([FromRoute] long id, [FromQuery] DateTime time)
        {
            _logger.LogInformation(
             "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
             nameof(CompensationRequisitionController),
             nameof(GetCompensationRequisitionAcquisitionPayees),
             User.GetUsername(),
             DateTime.Now);

            var acquisitionPayees = _compensationRequisitionService.GetCompensationRequisitionAcquisitionPayeesAtTime(id, time);

            return new JsonResult(_mapper.Map<IEnumerable<CompReqAcqPayeeModel>>(acquisitionPayees));
        }

        [HttpGet("{id:long}/lease-payees/historical")]
        [HasPermission(Permissions.CompensationRequisitionView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<CompReqLeasePayeeModel>), 200)]
        [SwaggerOperation(Tags = new[] { "compensation-requisition" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetCompensationRequisitionLeasePayees([FromRoute] long id, [FromQuery] DateTime time)
        {
            _logger.LogInformation(
              "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
              nameof(CompensationRequisitionController),
              nameof(GetCompensationRequisitionLeasePayees),
              User.GetUsername(),
              DateTime.Now);

            var leasePayees = _compensationRequisitionService.GetCompensationRequisitionLeasePayeesAtTime(id, time);

            return new JsonResult(_mapper.Map<IEnumerable<CompReqLeasePayeeModel>>(leasePayees));
        }
    }
}
