using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using System.Collections.Generic;

namespace Pims.Dal.Repositories
{
    public interface IActivityRepository : IRepository<PimsActivityInstance>
    {
        PimsActivityInstance GetById(long id);
        IList<PimsActivityInstance> GetAllByResearchFileId(long researchFileId);
        PimsActivityInstance Add(PimsActivityInstance instance);

    }
}
