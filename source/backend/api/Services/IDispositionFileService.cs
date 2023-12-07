using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Api.Services
{
    public interface IDispositionFileService
    {
        Paged<PimsDispositionFile> GetPage(DispositionFilter filter);

        PimsDispositionFile GetById(long id);

        LastUpdatedByModel GetLastUpdateInformation(long dispositionFileId);

        IEnumerable<PimsPropertyDispositionFile> GetProperties(long id);

        IEnumerable<PimsDispositionFileTeam> GetTeamMembers();
    }
}
