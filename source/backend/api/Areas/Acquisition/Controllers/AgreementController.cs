using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models.Concepts.AcquisitionFile;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Json;
using Pims.Dal.Security;
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
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a AgreementController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="acquisitionService"></param>
        /// <param name="mapper"></param>
        ///
        public AgreementController(IAcquisitionFileService acquisitionService, IMapper mapper)
        {
            _acquisitionService = acquisitionService;
            _mapper = mapper;
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
            var agreements = _acquisitionService.GetAgreements(id);
            return new JsonResult(_mapper.Map<IEnumerable<AgreementModel>>(agreements));
        }

        /// <summary>
        /// Update the acquisition file agreements.
        /// </summary>
        /// <returns>The updated agreements items.</returns>
        [HttpPost("{id:long}/agreements")]
        [HasPermission(Permissions.AgreementView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AgreementModel), 200)]
        [SwaggerOperation(Tags = new[] { "acquisitionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateAcquisitionFileAgreements([FromRoute] long id, [FromBody] List<AgreementModel> agreements)
        {
            var agreementEntities = _mapper.Map<List<Dal.Entities.PimsAgreement>>(agreements);
            var acquisitionFile = _acquisitionService.UpdateAgreements(id, agreementEntities);
            return new JsonResult(_mapper.Map<AcquisitionFileModel>(acquisitionFile));
        }

        #endregion
    }
}
