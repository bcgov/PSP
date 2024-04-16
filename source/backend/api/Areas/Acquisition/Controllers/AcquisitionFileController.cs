using System;
using System.Collections.Generic;
using System.Linq;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Models;
using Pims.Api.Models.Concepts.AcquisitionFile;
using Pims.Api.Models.Concepts.CompensationRequisition;
using Pims.Api.Models.Concepts.ExpropriationPayment;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Extensions;
using Pims.Core.Json;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Acquisition.Controllers
{
    /// <summary>
    /// AcquisitionFileController class, provides endpoints for interacting with acquisition files.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("acquisitionfiles")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class AcquisitionFileController : ControllerBase
    {
        #region Variables
        private readonly IAcquisitionFileService _acquisitionService;
        private readonly ICompReqFinancialService _compReqFinancialService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a AcquisitionFileController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="acquisitionService"></param>
        /// <param name="compReqFinancialService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        ///
        public AcquisitionFileController(IAcquisitionFileService acquisitionService, ICompReqFinancialService compReqFinancialService, IMapper mapper, ILogger<AcquisitionFileController> logger)
        {
            _acquisitionService = acquisitionService;
            _compReqFinancialService = compReqFinancialService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Gets the specified acquisition file.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:long}")]
        [HasPermission(Permissions.AcquisitionFileView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AcquisitionFileModel), 200)]
        [SwaggerOperation(Tags = new[] { "acquisitionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetAcquisitionFile(long id)
        {
            // RECOMMENDED - Add valuable metadata to logs
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(AcquisitionFileController),
                nameof(GetAcquisitionFile),
                User.GetUsername(),
                DateTime.Now);

            // RECOMMENDED - Log communications between components
            _logger.LogInformation("Dispatching to service: {Service}", _acquisitionService.GetType());

            var acqFile = _acquisitionService.GetById(id);
            return new JsonResult(_mapper.Map<AcquisitionFileModel>(acqFile));
        }

        /// <summary>
        /// Gets the specified acquisition file last updated-by information.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:long}/updateInfo")]
        [HasPermission(Permissions.AcquisitionFileView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Dal.Entities.Models.LastUpdatedByModel), 200)]
        [SwaggerOperation(Tags = new[] { "acquisitionfile" })]
        public IActionResult GetLastUpdatedBy(long id)
        {
            var lastUpdated = _acquisitionService.GetLastUpdateInformation(id);
            return new JsonResult(lastUpdated);
        }

        /// <summary>
        /// Adds the specified acquisition file.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HasPermission(Permissions.AcquisitionFileAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AcquisitionFileModel), 200)]
        [SwaggerOperation(Tags = new[] { "acquisitionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult AddAcquisitionFile([FromBody] AcquisitionFileModel model, [FromQuery] string[] userOverrideCodes)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(AcquisitionFileController),
                nameof(AddAcquisitionFile),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _acquisitionService.GetType());

            var acqFileEntity = _mapper.Map<Dal.Entities.PimsAcquisitionFile>(model);
            var acquisitionFile = _acquisitionService.Add(acqFileEntity, userOverrideCodes.Select(oc => UserOverrideCode.Parse(oc)));

            return new JsonResult(_mapper.Map<AcquisitionFileModel>(acquisitionFile));
        }

        /// <summary>
        /// Updates the acquisition file.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id:long}")]
        [HasPermission(Permissions.AcquisitionFileEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AcquisitionFileModel), 200)]
        [ProducesResponseType(typeof(ErrorResponseModel), 409)]
        [SwaggerOperation(Tags = new[] { "acquisitionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateAcquisitionFile(long id, [FromBody] AcquisitionFileModel model, [FromQuery] string[] userOverrideCodes)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(AcquisitionFileController),
                nameof(UpdateAcquisitionFile),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _acquisitionService.GetType());

            var acqFileEntity = _mapper.Map<Dal.Entities.PimsAcquisitionFile>(model);
            var acquisitionFile = _acquisitionService.Update(acqFileEntity, userOverrideCodes.Select(oc => UserOverrideCode.Parse(oc)));
            return new JsonResult(_mapper.Map<AcquisitionFileModel>(acquisitionFile));
        }

        /// <summary>
        /// Update the acquisition file properties.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id:long}/properties")]
        [HasPermission(Permissions.AcquisitionFileEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AcquisitionFileModel), 200)]
        [SwaggerOperation(Tags = new[] { "acquisitionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateAcquisitionFileProperties([FromBody] AcquisitionFileModel acquisitionFileModel, [FromQuery] string[] userOverrideCodes)
        {
            var acquisitionFileEntity = _mapper.Map<Dal.Entities.PimsAcquisitionFile>(acquisitionFileModel);
            var acquisitionFile = _acquisitionService.UpdateProperties(acquisitionFileEntity, userOverrideCodes.Select(oc => UserOverrideCode.Parse(oc)));
            return new JsonResult(_mapper.Map<AcquisitionFileModel>(acquisitionFile));
        }

        /// <summary>
        /// Get the acquisition file properties.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:long}/properties")]
        [HasPermission(Permissions.AcquisitionFileView, Permissions.PropertyView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<AcquisitionFilePropertyModel>), 200)]
        [SwaggerOperation(Tags = new[] { "acquisitionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetAcquisitionFileProperties(long id)
        {
            var acquisitionFileProperties = _acquisitionService.GetProperties(id);

            return new JsonResult(_mapper.Map<IEnumerable<AcquisitionFilePropertyModel>>(acquisitionFileProperties));
        }

        /// <summary>
        /// Get the acquisition file Owners.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:long}/owners")]
        [HasPermission(Permissions.AcquisitionFileView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<AcquisitionFileOwnerModel>), 200)]
        [SwaggerOperation(Tags = new[] { "acquisitionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetAcquisitionFileOwners([FromRoute] long id)
        {
            var owners = _acquisitionService.GetOwners(id);

            return new JsonResult(_mapper.Map<IEnumerable<AcquisitionFileOwnerModel>>(owners));
        }

        /// <summary>
        /// Get all unique persons that belong to at least one acquisition file as a team member.
        /// </summary>
        /// <returns></returns>
        [HttpGet("team-members")]
        [HasPermission(Permissions.AcquisitionFileView)]
        [HasPermission(Permissions.ContactView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<AcquisitionFileTeamModel>), 200)]
        [SwaggerOperation(Tags = new[] { "acquisitionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetAcquisitionTeamMembers()
        {
            var team = _acquisitionService.GetTeamMembers();

            return new JsonResult(_mapper.Map<IEnumerable<AcquisitionFileTeamModel>>(team));
        }

        /// <summary>
        /// Get all the compensations corresponding to the passed file id.
        /// </summary>
        /// <param name="id">The file to retrieve compensations for.</param>
        /// <returns></returns>
        [HttpGet("{id:long}/compensation-requisitions")]
        [HasPermission(Permissions.CompensationRequisitionView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<CompensationRequisitionModel>), 200)]
        [SwaggerOperation(Tags = new[] { "compensation-requisition" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetFileCompensations(long id)
        {
            var pimsCompensations = _acquisitionService.GetAcquisitionCompensations(id);
            var compensations = _mapper.Map<List<CompensationRequisitionModel>>(pimsCompensations);
            return new JsonResult(compensations);
        }

        /// <summary>
        /// Gets all the compensation requisition financials for an acq file.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:long}/comp-req-h120s")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<CompensationFinancialModel>), 200)]
        [SwaggerOperation(Tags = new[] { "comp-req-h120s" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetFileCompReqH120(long id, bool? finalOnly)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(AcquisitionFileController),
                nameof(GetFileCompReqH120),
                User.GetUsername(),
                DateTime.Now);
            _logger.LogInformation("Dispatching to service: {Service}", _compReqFinancialService.GetType());

            var h120s = _compReqFinancialService.GetAllByAcquisitionFileId(id, finalOnly);

            return new JsonResult(_mapper.Map<IEnumerable<CompensationFinancialModel>>(h120s));
        }

        /// <summary>
        /// Add a Compensation Requisition to an Acquisition File.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="compensationRequisition"></param>
        /// <returns></returns>
        [HttpPost("{id:long}/compensation-requisitions")]
        [HasPermission(Permissions.CompensationRequisitionAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CompensationRequisitionModel), 201)]
        [SwaggerOperation(Tags = new[] { "compensation-requisition" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult AddCompensationRequisition([FromRoute] long id, [FromBody] CompensationRequisitionModel compensationRequisition)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(AcquisitionFileController),
                nameof(AddCompensationRequisition),
                User.GetUsername(),
                DateTime.Now);
            _logger.LogInformation($"Dispatching to service: {_acquisitionService.GetType()}");

            var compensationReqEntity = _mapper.Map<Dal.Entities.PimsCompensationRequisition>(compensationRequisition);
            var newCompensationRequisition = _acquisitionService.AddCompensationRequisition(id, compensationReqEntity);

            return new JsonResult(_mapper.Map<CompensationRequisitionModel>(newCompensationRequisition));
        }

        /// <summary>
        /// Get all Expropriation Payments from the Acquisition File.
        /// </summary>
        /// <param name="id">Acquisition File Id.</param>
        /// <returns>List of all Expropriation Payments created for the acquisition file.</returns>
        [HttpGet("{id:long}/expropriation-payments")]
        [HasPermission(Permissions.AcquisitionFileView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<ExpropriationPaymentModel>), 200)]
        [SwaggerOperation(Tags = new[] { "expropriation-payments" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetAcquisitionFileExpropriationPayments([FromRoute] long id)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(AcquisitionFileController),
                nameof(GetAcquisitionFileExpropriationPayments),
                User.GetUsername(),
                DateTime.Now);
            _logger.LogInformation($"Dispatching to service: {_acquisitionService.GetType()}");

            var pimsForm8s = _acquisitionService.GetAcquisitionExpropriationPayments(id);
            var form8s = _mapper.Map<List<ExpropriationPaymentModel>>(pimsForm8s);

            return new JsonResult(form8s);
        }

        /// <summary>
        /// Creates a new Form8 for the acquisition file.
        /// </summary>
        /// <param name="id">Acquisition File Id.</param>
        /// <param name="expropriationPayment">Form8 Data Model.</param>
        /// <returns></returns>
        [HttpPost("{id:long}/expropriation-payments")]
        [HasPermission(Permissions.AcquisitionFileEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ExpropriationPaymentModel), 201)]
        [SwaggerOperation(Tags = new[] { "expropriation-payments" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult AddExpropriationPayment([FromRoute] long id, [FromBody] ExpropriationPaymentModel expropriationPayment)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(AcquisitionFileController),
                nameof(AddExpropriationPayment),
                User.GetUsername(),
                DateTime.Now);
            _logger.LogInformation($"Dispatching to service: {_acquisitionService.GetType()}");

            var expPaymentEntity = _mapper.Map<PimsExpropriationPayment>(expropriationPayment);
            var newExpPaymentEntity = _acquisitionService.AddExpropriationPayment(id, expPaymentEntity);

            return new JsonResult(_mapper.Map<ExpropriationPaymentModel>(newExpPaymentEntity));
        }

        #endregion
    }
}
