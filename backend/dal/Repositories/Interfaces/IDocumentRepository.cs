using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IDocumentRepository interface, provides functions to interact with documents within the datasource.
    /// </summary>
    public interface IDocumentRepository : IRepository<PimsDocument>
    {
        int GetTotalRelationCount(long documentId);

        bool Delete(PimsDocument document);
    }
}
