using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IDocumentActivityRepository interface, provides functions to interact with document activities within the datasource.
    /// </summary>
    public interface IDocumentActivityRepository : IRepository<PimsActivityInstanceDocument>
    {
        IList<PimsActivityInstanceDocument> GetAllByDocument(long documentId);

        IList<PimsActivityInstanceDocument> GetAllByActivity(long activityId);

        IList<PimsActivityInstanceDocument> GetAllByResearchFile(long fileId);

        IList<PimsActivityInstanceDocument> GetAllByAcquisitionFile(long fileId);

        PimsActivityInstanceDocument Add(PimsActivityInstanceDocument activityDocument);

        bool Delete(PimsActivityInstanceDocument activityDocument);
    }
}
