using Pims.Dal.Constants;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Services
{
    public interface INoteService
    {
        GenericNote Add(NoteType type, GenericNote noteModel);
    }
}
