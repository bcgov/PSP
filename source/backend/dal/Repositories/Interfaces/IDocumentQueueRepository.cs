using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IDocumentQueueRepository interface, provides functions to interact with queued documents within the datasource.
    /// </summary>
    public interface IDocumentQueueRepository : IRepository<PimsDocument>
    {

        PimsDocumentQueue TryGetById(long documentQueueId);

        IEnumerable<PimsDocumentQueue> GetAllByFilter(DocumentQueueFilter filter);

        PimsDocumentQueue Update(PimsDocumentQueue queuedDocument, bool removeDocument = false);

        bool Delete(PimsDocumentQueue queuedDocument);

        int DocumentQueueCount(PimsDocumentQueueStatusType pimsDocumentQueueStatusType);
    }
}
