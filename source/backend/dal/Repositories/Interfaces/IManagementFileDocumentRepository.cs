using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IManagementFileDocumentRepository interface, provides functions to interact with document management files within the datasource.
    /// </summary>
    public interface IManagementFileDocumentRepository : IRepository<PimsManagementFileDocument>
    {
        IList<PimsManagementFileDocument> GetAllByManagementFile(long fileId);

        PimsManagementFileDocument AddManagementFileDocument(PimsManagementFileDocument managementDocument);

        bool DeleteManagementFileDocument(PimsManagementFileDocument managementDocument);
    }
}
