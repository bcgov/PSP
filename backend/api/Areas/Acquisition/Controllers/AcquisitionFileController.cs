
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models.Concepts;
using Pims.Api.Policies;
using Pims.Dal.Security;
using Pims.Api.Services;
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
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a AcquisitionFileController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="acquisitionService"></param>
        /// <param name="mapper"></param>
        ///
        public AcquisitionFileController(IAcquisitionFileService acquisitionService, IMapper mapper)
        {
            _acquisitionService = acquisitionService;
            _mapper = mapper;
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
        [SwaggerOperation(Tags = new[] {"acquisitionfile" })]
        public IActionResult GetAcquisitionFile(long id)
        {
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
        [SwaggerOperation(Tags = new[] {"acquisitionfile" })]
        public IActionResult AddAcquisitionFile([FromBody] AcquisitionFileModel model)
        {
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
        [SwaggerOperation(Tags = new[] {"acquisitionfile" })]
        public IActionResult UpdateResearchFile(long id, [FromBody] AcquisitionFileModel model)
        {
            // TODO: Implementation pending
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
