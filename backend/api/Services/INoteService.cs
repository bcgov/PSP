using Pims.Api.Constants;
using Pims.Api.Models.Concepts;

namespace Pims.Api.Services
{
    public interface INoteService
    {
        EntityNoteModel Add(NoteType type, EntityNoteModel model);
    }
}
