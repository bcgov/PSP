using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IDispositionFileDocumentRepository interface, provides functions to interact with document disposition files within the datasource.
    /// </summary>
    public interface IDispositionFileDocumentRepository : IRepository<PimsDispositionFileDocument>
    {
        IList<PimsDispositionFileDocument> GetAllByDispositionFile(long fileId);

        PimsDispositionFileDocument AddDispositionDocument(PimsDispositionFileDocument dispositionDocument);

        bool DeleteDispositionDocument(PimsDispositionFileDocument dispositionDocument);
    }
}
