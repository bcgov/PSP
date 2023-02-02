using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IResearchFileDocumentRepository interface, provides functions to interact with document research files within the datasource.
    /// </summary>
    public interface IResearchFileDocumentRepository : IRepository<PimsResearchFileDocument>
    {
        IList<PimsResearchFileDocument> GetAllByResearchFile(long fileId);

        IList<PimsResearchFileDocument> GetAllByDocument(long documentId);

        PimsResearchFileDocument AddResearch(PimsResearchFileDocument researchDocument);

        bool DeleteResearch(PimsResearchFileDocument researchDocument);
    }
}
