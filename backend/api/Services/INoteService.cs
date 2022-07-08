using Pims.Api.Constants;
using Pims.Api.Models.Concepts;
using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface INoteService
    {
        EntityNoteModel Add(NoteType type, EntityNoteModel model);

        void DeleteNote(NoteType type, int noteId);
        IEnumerable<PimsNote> GetNotes(NoteType type);
    }
}
