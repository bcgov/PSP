using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IAcquisitionFileDocumentRepository interface, provides functions to interact with document acquisition files within the datasource.
    /// </summary>
    public interface IAcquisitionFileDocumentRepository : IRepository<PimsAcquisitionFileDocument>
    {
        IList<PimsAcquisitionFileDocument> GetAllByAcquisitionFile(long fileId);

        IList<PimsAcquisitionFileDocument> GetAllByDocument(long documentId);

        PimsAcquisitionFileDocument AddAcquisition(PimsAcquisitionFileDocument acquisitionDocument);

        bool DeleteAcquisition(PimsAcquisitionFileDocument acquisitionDocument);
    }
}
