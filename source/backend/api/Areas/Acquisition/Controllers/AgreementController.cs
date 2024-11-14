using System;
using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Concepts.AcquisitionFile;
using Pims.Api.Services;
using Pims.Core.Api.Exceptions;
using Pims.Core.Api.Policies;
using Pims.Core.Extensions;
using Pims.Core.Json;
using Pims.Core.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Acquisition.Controllers
{
    /// <summary>
    /// AgreementController class, provides endpoints for interacting with acquisition files agreements.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("acquisitionfiles")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class AgreementController : ControllerBase
    {
        #region Variables
        private readonly IAcquisitionFileService _acquisitionService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a AgreementController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="acquisitionService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public AgreementController(IAcquisitionFileService acquisitionService, IMapper mapper, ILogger<AgreementController> logger)
        {
            _acquisitionService = acquisitionService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Get the acquisition file agreements.
        /// </summary>
        /// <returns>The agreements items.</returns>
        [HttpGet("{id:long}/agreements")]
        [HasPermission(Permissions.AgreementView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<AgreementModel>), 200)]
        [SwaggerOperation(Tags = new[] { "acquisitionfile" })]
        public IActionResult GetAcquisitionFileAgreements([FromRoute] long id)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(AgreementController),
                nameof(GetAcquisitionFileAgreements),
                User.GetUsername(),
                DateTime.Now);

            var agreements = _acquisitionService.GetAgreements(id);
            return new JsonResult(_mapper.Map<IEnumerable<AgreementModel>>(agreements));
        }

        /// <summary>
        /// Create the acquisition file agreement to the acquisition file.
        /// </summary>
        /// <returns>The agreements items.</returns>
        [HttpPost("{id:long}/agreements")]
        [HasPermission(Permissions.AgreementView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AgreementModel), 201)]
        [TypeFilter(typeof(NullJsonResultFilter))]
        [SwaggerOperation(Tags = new[] { "acquisitionfile" })]
        public IActionResult AddAcquisitionFileAgreement([FromRoute] long id, [FromBody] AgreementModel agreement)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(AgreementController),
                nameof(AddAcquisitionFileAgreement),
                User.GetUsername(),
                DateTime.Now);

            if (id != agreement.AcquisitionFileId)
            {
                throw new BadRequestException("Invalid AcquisitionFileId.");
            }

            var newAgreement = _acquisitionService.AddAgreement(id, _mapper.Map<Dal.Entities.PimsAgreement>(agreement));

            return new JsonResult(_mapper.Map<AgreementModel>(newAgreement));
        }

        /// <summary>
        /// Get the acquisition file agreement by Id.
        /// </summary>
        /// <returns>The agreements items.</returns>
        [HttpGet("{id:long}/agreements/{agreementId:long}")]
        [HasPermission(Permissions.AgreementView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AgreementModel), 200)]
        [SwaggerOperation(Tags = new[] { "acquisitionfile" })]
        public IActionResult GetAcquisitionFileAgreementById([FromRoute]long id, [FromRoute]long agreementId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(AgreementController),
                nameof(GetAcquisitionFileAgreementById),
                User.GetUsername(),
                DateTime.Now);

            var agreement = _acquisitionService.GetAgreementById(id, agreementId);

            return new JsonResult(_mapper.Map<AgreementModel>(agreement));
        }

        /// <summary>
        /// Update the acquisition file agreement by Id.
        /// </summary>
        /// <returns>The agreements item updated.</returns>
        [HttpPut("{id:long}/agreements/{agreementId:long}")]
        [HasPermission(Permissions.AgreementView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AgreementModel), 200)]
        [TypeFilter(typeof(NullJsonResultFilter))]
        [SwaggerOperation(Tags = new[] { "acquisitionfile" })]
        public IActionResult UpdateAcquisitionFileAgreement([FromRoute] long id, [FromRoute] long agreementId, [FromBody] AgreementModel agreement)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(AgreementController),
                nameof(UpdateAcquisitionFileAgreement),
                User.GetUsername(),
                DateTime.Now);

            if (id != agreement.AcquisitionFileId)
            {
                throw new BadRequestException("Invalid AcquisitionFileId.");
            }

            var updatedAgreement = _acquisitionService.UpdateAgreement(id, _mapper.Map<Dal.Entities.PimsAgreement>(agreement));

            return new JsonResult(_mapper.Map<AgreementModel>(updatedAgreement));
        }

        /// <summary>
        /// Delete the acquisition file agreement by Id.
        /// </summary>
        /// <returns>The agreements item updated.</returns>
        [HttpDelete("{id:long}/agreements/{agreementId:long}")]
        [HasPermission(Permissions.AgreementView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), 200)]
        [SwaggerOperation(Tags = new[] { "acquisitionfile" })]
        public IActionResult DeleteAcquisitionFileAgreement([FromRoute] long id, [FromRoute] long agreementId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(AgreementController),
                nameof(DeleteAcquisitionFileAgreement),
                User.GetUsername(),
                DateTime.Now);

            var result = _acquisitionService.DeleteAgreement(id, agreementId);

            return new JsonResult(result);
        }

        #endregion
    }
}
