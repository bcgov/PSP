using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IActivityRepository : IRepository<PimsActivityInstance>
    {
        PimsActivityInstance GetById(long id);

        IList<PimsActivityInstance> GetAllByResearchFileId(long researchFileId);

        PimsActivityInstance Add(PimsActivityInstance instance);
    }
}
