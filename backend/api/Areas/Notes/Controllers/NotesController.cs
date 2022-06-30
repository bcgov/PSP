
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models.Concepts;
using Pims.Api.Policies;
using Pims.Dal.Constants;
using Pims.Dal.Entities.Models;
using Pims.Dal.Security;
using Pims.Dal.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Notes.Controllers
{
    /// <summary>
    /// NotesController class, provides endpoints for interacting with notes.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("notes")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class NotesController : ControllerBase
    {
        #region Variables
        private readonly IPimsService _pimsService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a NotesController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="pimsService"></param>
        /// <param name="mapper"></param>
        ///
        public NotesController(IPimsService pimsService, IMapper mapper)
        {
            _pimsService = pimsService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Add the specified note.
        /// </summary>
        /// <param name="type">The parent entity type.</param>
        /// <param name="noteModel">The note to add.</param>
        /// <returns></returns>
        [HttpPost("{type}")]
        [HasPermission(Permissions.NoteAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GenericNote), 200)]
        [SwaggerOperation(Tags = new[] { "note" })]
        public IActionResult AddNote(NoteType type, [FromBody] GenericNote noteModel)
        {
            var createdNote =_pimsService.NoteService.Add(type, noteModel);
            return new JsonResult(createdNote);
        }
        #endregion
    }
}
