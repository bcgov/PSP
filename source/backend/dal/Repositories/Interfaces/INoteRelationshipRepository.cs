using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// INoteRelationshipRepository interface, provides a generic interface that defines note relationship repositories.
    /// </summary>
    public interface INoteRelationshipRepository<T> : IRepository<T>
        where T : PimsNoteRelationship
    {
        IList<PimsNote> GetAllByParentId(long parentId);

        T AddNoteRelationship(T noteRelationship);

        bool DeleteNoteRelationship(long noteId);
    }
}
