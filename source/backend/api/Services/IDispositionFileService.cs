using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;

namespace Pims.Api.Services
{
    public interface IDispositionFileService
    {
        PimsDispositionFile GetById(long id);

        PimsDispositionFile Add(PimsDispositionFile dispositionFile, IEnumerable<UserOverrideCode> userOverrides);

        LastUpdatedByModel GetLastUpdateInformation(long dispositionFileId);

        IEnumerable<PimsPropertyDispositionFile> GetProperties(long id);
    }
}
