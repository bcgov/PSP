using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Api.Services
{
    public interface IDispositionFileService
    { 
        PimsDispositionFile GetById(long id);

        LastUpdatedByModel GetLastUpdateInformation(long dispositionFileId);

        IEnumerable<PimsPropertyDispositionFile> GetProperties(long id);
    }
}
