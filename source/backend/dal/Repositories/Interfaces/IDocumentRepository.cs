using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IDocumentRepository interface, provides functions to interact with documents within the datasource.
    /// </summary>
    public interface IDocumentRepository : IRepository<PimsDocument>
    {
        PimsDocument Add(PimsDocument document);

        PimsDocument TryGet(long documentId);

        PimsDocument Update(PimsDocument document, bool commitTransaction = true);

        bool Delete(PimsDocument document);

        int DocumentRelationshipCount(long documentId);

        PimsDocument TryGetDocumentRelationships(long documentId);
    }
}
