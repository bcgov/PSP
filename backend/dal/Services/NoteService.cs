using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Models;

namespace Pims.Dal.Services
{
    public class NoteService : INoteService
    {
        #region Variables
        private readonly IPimsRepository _pimsService;
        #endregion


        #region Constructors

        /// <summary>
        /// Creates a new instance of a NoteService class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="pimsService"></param>
        ///
        public NoteService(IPimsRepository pimsService)
        {
            _pimsService = pimsService;
        }
        #endregion

        /// <summary>
        /// Find and delete the note
        /// </summary>
        /// <param name="type">Note type to determine the type of note to delete.</param>
        /// <param name="noteId">Note id to identify the note to delete</param>
        public void DeleteNote(NoteType type, int noteId)
        {
            switch (type)
            {
                case NoteType.Activity:
                    _pimsService.Note.DeleteActivityNotes(noteId);
                    break;
                case NoteType.File:
                    // Write code to delete the note from FileNotes table
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Get notes by note type.
        /// </summary>
        /// <param name="type">Note type to determine the type of notes to return.</param>
        /// <returns></returns>
        public IEnumerable<PimsNote> GetNotes(NoteType type)
        {
            switch (type)
            {
                case NoteType.Activity:
                    return _pimsService.Note.GetActivityNotes();
                case NoteType.File:
                default:
                    return new List<PimsNote>();
            }
        }
    }
}
