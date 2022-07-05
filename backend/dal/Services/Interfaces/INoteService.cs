using Pims.Dal.Constants;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Services
{
    public interface INoteService
    {
        EntityNoteModel Add(NoteType type, EntityNoteModel model);
    }
}
