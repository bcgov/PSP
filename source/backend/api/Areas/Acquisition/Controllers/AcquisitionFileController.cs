using System;
using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Concepts;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
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
        #endregion
    }
}
