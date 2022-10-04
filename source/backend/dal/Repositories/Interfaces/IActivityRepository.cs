using System;
using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IActivityRepository : IRepository<PimsActivityInstance>
    {
        PimsActivityInstance GetById(long id);

        long GetRowVersion(long activityId);

        IList<PimsActivityInstance> GetAllByResearchFileId(long researchFileId);

        IList<PimsActivityInstance> GetAllByAcquisitionFileId(long acquisitionFileId);

        PimsActivityInstance Add(PimsActivityInstance instance);

        Tuple<PimsActivityInstance, PimsActivityInstanceNote> Update(PimsActivityInstance instance);

        PimsActivityInstance UpdateActivityResearchProperties(PimsActivityInstance instance);

        PimsActivityInstance UpdateActivityAcquisitionProperties(PimsActivityInstance instance);

        bool Delete(long activityId);
    }
}
