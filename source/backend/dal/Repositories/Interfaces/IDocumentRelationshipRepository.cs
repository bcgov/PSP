using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IDocumentRelationshipRepository interface, provides a generic interface that defines document relationship repositories.
    /// </summary>
    public interface IDocumentRelationshipRepository<T> : IRepository<T>
        where T : PimsFileDocument
    {
        IList<T> GetAllByParentId(long parentId);

        T AddDocument(T document);

        bool DeleteDocument(T document);
    }
}
