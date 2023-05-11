using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models.Concepts;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Acquisition.Controllers
{
    /// <summary>
    /// ChecklistController class, provides endpoints for interacting with acquisition files checklists.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("acquisitionfiles")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class ChecklistController : ControllerBase
    {
        #region Variables
        private readonly IAcquisitionFileService _acquisitionService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a ChecklistController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="acquisitionService"></param>
        /// <param name="mapper"></param>
        ///
        public ChecklistController(IAcquisitionFileService acquisitionService, IMapper mapper)
        {
            _acquisitionService = acquisitionService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Get the acquisition file checklist.
        /// </summary>
        /// <returns>The checklist items.</returns>
        [HttpGet("{id:long}/checklist")]
        [HasPermission(Permissions.AcquisitionFileView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<AcquisitionFileChecklistItemModel>), 200)]
        [SwaggerOperation(Tags = new[] { "acquisitionfile" })]
        public IActionResult GetAcquisitionFileChecklist([FromRoute] long id)
        {
            var checklist = _acquisitionService.GetChecklistItems(id);
            return new JsonResult(_mapper.Map<IEnumerable<AcquisitionFileChecklistItemModel>>(checklist));
        }

        /// <summary>
        /// Update the acquisition file checklist.
        /// </summary>
        /// <returns>The updated checklist items.</returns>
        [HttpPut("{id:long}/checklist")]
        [HasPermission(Permissions.AcquisitionFileEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AcquisitionFileModel), 200)]
        [SwaggerOperation(Tags = new[] { "acquisitionfile" })]
        public IActionResult UpdateAcquisitionFileChecklist([FromBody] AcquisitionFileModel acquisitionFileModel)
        {
            var acquisitionFileEntity = _mapper.Map<Dal.Entities.PimsAcquisitionFile>(acquisitionFileModel);
            var acquisitionFile = _acquisitionService.UpdateChecklistItems(acquisitionFileEntity);
            return new JsonResult(_mapper.Map<AcquisitionFileModel>(acquisitionFile));
        }

        #endregion
    }
}
