using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Repositories
{
    public interface IDispositionFileRepository : IRepository
    {
        Paged<PimsDispositionFile> GetPageDeep(DispositionFilter filter);

        PimsDispositionFile GetById(long id);

        LastUpdatedByModel GetLastUpdateBy(long id);

        List<PimsDispositionFileTeam> GetTeamMembers();

        long GetRowVersion(long id);
    }
}
