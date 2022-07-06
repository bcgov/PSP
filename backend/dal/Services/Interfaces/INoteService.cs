using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Models;

namespace Pims.Dal.Services
{
    public interface INoteService
    {
        void DeleteNote(NoteType type, int noteId);
        IEnumerable<PimsNote> GetNotes(NoteType type);
    }
}
