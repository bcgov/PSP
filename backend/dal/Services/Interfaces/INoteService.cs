using Pims.Dal.Constants;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Services
{
    public interface INoteService
    {
        GenericNoteModel Add(NoteType type, GenericNoteModel noteModel);
    }
}
