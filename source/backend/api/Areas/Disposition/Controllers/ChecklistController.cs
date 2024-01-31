using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models.Concepts.DispositionFile;
using Pims.Api.Models.Concepts.File;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Json;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Disposition.Controllers
{
    /// <summary>
    /// ChecklistController class, provides endpoints for interacting with disposition files checklists.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("dispositionfiles")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class ChecklistController : ControllerBase
    {
        #region Variables
        private readonly IDispositionFileService _dispositionService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a ChecklistController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="dispositionService"></param>
        /// <param name="mapper"></param>
        ///
        public ChecklistController(IDispositionFileService dispositionService, IMapper mapper)
        {
            _dispositionService = dispositionService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Get the disposition file checklist.
        /// </summary>
        /// <returns>The checklist items.</returns>
        [HttpGet("{id:long}/checklist")]
        [HasPermission(Permissions.DispositionView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<FileChecklistItemModel>), 200)]
        [SwaggerOperation(Tags = new[] { "dispositionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetDispositionFileChecklist([FromRoute] long id)
        {
            var checklist = _dispositionService.GetChecklistItems(id);
            return new JsonResult(_mapper.Map<IEnumerable<FileChecklistItemModel>>(checklist));
        }

        /// <summary>
        /// Update the disposition file checklist.
        /// </summary>
        /// <returns>The updated checklist items.</returns>
        [HttpPut("{id:long}/checklist")]
        [HasPermission(Permissions.DispositionEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DispositionFileModel), 200)]
        [SwaggerOperation(Tags = new[] { "dispositionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateDispositionFileChecklist(long id, [FromBody] IList<FileChecklistItemModel> checklistItems)
        {

            foreach (var item in checklistItems)
            {
                if (item.FileId != id)
                {
                    throw new BadRequestException("All checklist items file id must match the disposition file id");
                }
            }

            var checklistItemEntities = _mapper.Map<IList<Dal.Entities.PimsDispositionChecklistItem>>(checklistItems);
            var dispositionFile = _dispositionService.UpdateChecklistItems(checklistItemEntities);
            return new JsonResult(_mapper.Map<DispositionFileModel>(dispositionFile));
        }

        #endregion
    }
}
