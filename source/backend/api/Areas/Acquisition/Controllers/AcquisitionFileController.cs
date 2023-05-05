using System;
using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Areas.CompensationRequisition.Controllers;
using Pims.Api.Models.Concepts;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Core.Json;
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
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a AcquisitionFileController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="acquisitionService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        ///
        public AcquisitionFileController(IAcquisitionFileService acquisitionService, IMapper mapper, ILogger<AcquisitionFileController> logger)
        {
            _acquisitionService = acquisitionService;
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
        /// Adds the specified acquisition file.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HasPermission(Permissions.AcquisitionFileAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AcquisitionFileModel), 200)]
        [SwaggerOperation(Tags = new[] { "acquisitionfile" })]
        public IActionResult AddAcquisitionFile([FromBody] AcquisitionFileModel model)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(AcquisitionFileController),
                nameof(AddAcquisitionFile),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _acquisitionService.GetType());

            var acqFileEntity = _mapper.Map<Dal.Entities.PimsAcquisitionFile>(model);
            var acquisitionFile = _acquisitionService.Add(acqFileEntity);

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
        [SwaggerOperation(Tags = new[] { "acquisitionfile" })]
        public IActionResult UpdateAcquisitionFile(long id, [FromBody] AcquisitionFileModel model, bool userOverride = false)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(AcquisitionFileController),
                nameof(UpdateAcquisitionFile),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _acquisitionService.GetType());

            var acqFileEntity = _mapper.Map<Dal.Entities.PimsAcquisitionFile>(model);

            try
            {
                var acquisitionFile = _acquisitionService.Update(acqFileEntity, userOverride);
                return new JsonResult(_mapper.Map<AcquisitionFileModel>(acquisitionFile));
            }
            catch (BusinessRuleViolationException e)
            {
                return Conflict(e.Message);
            }
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
        public IActionResult UpdateAcquisitionFileProperties([FromBody] AcquisitionFileModel acquisitionFileModel)
        {
            var acquisitionFileEntity = _mapper.Map<Dal.Entities.PimsAcquisitionFile>(acquisitionFileModel);
            try
            {
                var acquisitionFile = _acquisitionService.UpdateProperties(acquisitionFileEntity);
                return new JsonResult(_mapper.Map<AcquisitionFileModel>(acquisitionFile));
            }
            catch (BusinessRuleViolationException e)
            {
                return Conflict(e.Message);
            }
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
        public IActionResult GetAcquisitionFileOwners([FromRoute] long id)
        {
            var owners = _acquisitionService.GetOwners(id);

            return new JsonResult(_mapper.Map<IEnumerable<AcquisitionFileOwnerModel>>(owners));
        }

        [HttpGet("{id:long}/project")]
        [HasPermission(Permissions.AcquisitionFileView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ProjectModel), 200)]
        [SwaggerOperation(Tags = new[] { "acquisitionfile" })]
        public IActionResult GetAcquisitionFileProject([FromRoute] long id)
        {
            var project = _acquisitionService.GetProject(id);

            return new JsonResult(_mapper.Map<ProjectModel>(project));
        }

        [HttpGet("{id:long}/product")]
        [HasPermission(Permissions.AcquisitionFileView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ProductModel), 200)]
        [SwaggerOperation(Tags = new[] { "acquisitionfile" })]
        public IActionResult GetAcquisitionFileProduct([FromRoute] long id)
        {
            var product = _acquisitionService.GetProduct(id);

            return new JsonResult(_mapper.Map<ProductModel>(product));
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
        public IActionResult AddCompensationRequisition([FromRoute]long id, [FromBody] CompensationRequisitionModel compensationRequisition)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(CompensationRequisitionController),
                nameof(AddCompensationRequisition),
                User.GetUsername(),
                DateTime.Now);
            _logger.LogInformation($"Dispatching to service: {_acquisitionService.GetType()}");

            var compensationReqEntity = _mapper.Map<Dal.Entities.PimsCompensationRequisition>(compensationRequisition);
            var newCompensationRequisition = _acquisitionService.AddCompensationRequisition(id, compensationReqEntity);

            return new JsonResult(_mapper.Map<CompensationRequisitionModel>(newCompensationRequisition));
        }

        #endregion
    }
}
