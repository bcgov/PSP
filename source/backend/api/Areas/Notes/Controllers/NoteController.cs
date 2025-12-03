using System;
using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Constants;
using Pims.Api.Models.Concepts.Note;
using Pims.Api.Services;
using Pims.Core.Api.Exceptions;
using Pims.Core.Api.Policies;
using Pims.Core.Extensions;
using Pims.Core.Json;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Notes.Controllers
{
    /// <summary>
    /// NoteController class, provides endpoints for interacting with notes.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("notes")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class NoteController : ControllerBase
    {
        #region Variables
        private readonly INoteService _noteService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a NoteController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="noteService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        ///
        public NoteController(INoteService noteService, IMapper mapper, ILogger<NoteController> logger)
        {
            _noteService = noteService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Add the specified note.
        /// </summary>
        /// <param name="type">The parent entity type.</param>
        /// <param name="model">The note to add.</param>
        /// <returns></returns>
        [HttpPost("{type}")]
        [HasPermission(Permissions.NoteAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(EntityNoteModel), 200)]
        [SwaggerOperation(Tags = new[] { "note" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult AddNote(NoteType type, [FromBody] EntityNoteModel model)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(NoteController),
                nameof(AddNote),
                User.GetUsername(),
                DateTime.Now);

            switch (type)
            {
                case NoteType.Acquisition_File:
                    var acquisitionFileNoteEntity = _mapper.Map<PimsAcquisitionFileNote>(model);
                    var acquisitionFileNote = _noteService.AddAcquisitionFileNote(acquisitionFileNoteEntity);
                    return new JsonResult(_mapper.Map<EntityNoteModel>(acquisitionFileNote));
                case NoteType.Disposition_File:
                    var dispositionFileNoteEntity = _mapper.Map<PimsDispositionFileNote>(model);
                    var dispositionFileNote = _noteService.AddDispositionFileNote(dispositionFileNoteEntity);
                    return new JsonResult(_mapper.Map<EntityNoteModel>(dispositionFileNote));
                case NoteType.Lease_File:
                    var leaseNoteEntity = _mapper.Map<PimsLeaseNote>(model);
                    var leaseNote = _noteService.AddLeaseNote(leaseNoteEntity);
                    return new JsonResult(_mapper.Map<EntityNoteModel>(leaseNote));
                case NoteType.Project:
                    var projectNoteEntity = _mapper.Map<PimsProjectNote>(model);
                    var projectNote = _noteService.AddProjectNote(projectNoteEntity);
                    return new JsonResult(_mapper.Map<EntityNoteModel>(projectNote));
                case NoteType.Research_File:
                    var researchFileNoteEntity = _mapper.Map<PimsResearchFileNote>(model);
                    var researchFileNote = _noteService.AddResearchFileNote(researchFileNoteEntity);
                    return new JsonResult(_mapper.Map<EntityNoteModel>(researchFileNote));
                case NoteType.Management_File:
                    var managementFileNoteEntity = _mapper.Map<PimsManagementFileNote>(model);
                    var managementFileNote = _noteService.AddManagementFileNote(managementFileNoteEntity);
                    return new JsonResult(_mapper.Map<EntityNoteModel>(managementFileNote));
                case NoteType.Property:
                    var propertyNoteEntity = _mapper.Map<PimsPropertyNote>(model);
                    var propertyNote = _noteService.AddPropertyNote(propertyNoteEntity);
                    return new JsonResult(_mapper.Map<EntityNoteModel>(propertyNote));
                default:
                    throw new BadRequestException("Relationship type not valid.");
            }
        }

        /// <summary>
        /// Gets a collection of notes for the specified type and owner id.
        /// </summary>
        /// <param name="type">Used to identify note type.</param>
        /// <param name="entityId">Used to identify note's parent entity.</param>
        /// <returns></returns>
        [HttpGet("{type}/{entityId:long}")]
        [Produces("application/json")]
        [HasPermission(Permissions.NoteView)]
        [ProducesResponseType(typeof(IEnumerable<NoteModel>), 200)]
        [SwaggerOperation(Tags = new[] { "note" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetNotes(NoteType type, long entityId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(NoteController),
                nameof(GetNotes),
                User.GetUsername(),
                DateTime.Now);

            var pimsNotes = _noteService.GetNotes(type, entityId);
            var mappedNotes = _mapper.Map<List<NoteModel>>(pimsNotes);
            return new JsonResult(mappedNotes);
        }

        /// <summary>
        /// Retrieves the note with the specified id.
        /// </summary>
        /// <param name="noteId">Used to identify the note.</param>
        /// <returns></returns>
        [HttpGet("{noteId:long}")]
        [Produces("application/json")]
        [HasPermission(Permissions.NoteView)]
        [ProducesResponseType(typeof(NoteModel), 200)]
        [SwaggerOperation(Tags = new[] { "note" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetNoteById(long noteId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(NoteController),
                nameof(GetNoteById),
                User.GetUsername(),
                DateTime.Now);

            var pimsNote = _noteService.GetById(noteId);
            return new JsonResult(_mapper.Map<NoteModel>(pimsNote));
        }

        /// <summary>
        /// Updates the note with the specified id.
        /// </summary>
        /// <param name="noteId">Used to identify the note.</param>
        /// <param name="noteModel">The updated note values.</param>
        /// <returns></returns>
        [HttpPut("{noteId:long}")]
        [Produces("application/json")]
        [HasPermission(Permissions.NoteEdit)]
        [ProducesResponseType(typeof(NoteModel), 200)]
        [SwaggerOperation(Tags = new[] { "note" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateNote(long noteId, [FromBody] NoteModel noteModel)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(NoteController),
                nameof(UpdateNote),
                User.GetUsername(),
                DateTime.Now);

            if (noteId != noteModel.Id)
            {
                return BadRequest("Model and path id do not match.");
            }
            var noteEntity = _mapper.Map<PimsNote>(noteModel);
            var updatedNote = _noteService.Update(noteEntity);
            return new JsonResult(_mapper.Map<NoteModel>(updatedNote));
        }

        /// <summary>
        /// Deletes the note for the specified type.
        /// </summary>
        /// <param name="type">Used to identify note type.</param>
        /// <param name="noteId">Used to identify the note and delete it.</param>
        /// <returns></returns>
        [HttpDelete("{noteId:long}/{type}")]
        [Produces("application/json")]
        [HasPermission(Permissions.NoteDelete)]
        [ProducesResponseType(typeof(bool), 200)]
        [SwaggerOperation(Tags = new[] { "note" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult DeleteNote(NoteType type, long noteId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(NoteController),
                nameof(DeleteNote),
                User.GetUsername(),
                DateTime.Now);

            return new JsonResult(_noteService.DeleteNote(type, noteId));
        }
        #endregion
    }
}
