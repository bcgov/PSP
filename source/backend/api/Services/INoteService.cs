using System.Collections.Generic;
using Pims.Api.Constants;
using Pims.Api.Models.Concepts;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface INoteService
    {
        NoteModel GetById(long id);

        EntityNoteModel Add(NoteType type, EntityNoteModel model);

        NoteModel Update(NoteModel model);

        bool DeleteNote(NoteType type, long noteId, bool commitTransaction = true);

        IEnumerable<PimsNote> GetNotes(NoteType type, long entityId);
    }
}
