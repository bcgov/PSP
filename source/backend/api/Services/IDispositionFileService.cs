using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Api.Services
{
    public interface IDispositionFileService
    {
        PimsDispositionFile GetById(long id);

        PimsDispositionFile Add(PimsDispositionFile dispositionFile);

        LastUpdatedByModel GetLastUpdateInformation(long dispositionFileId);

        IEnumerable<PimsPropertyDispositionFile> GetProperties(long id);
    }
}
