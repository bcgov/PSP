using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface IActivityService
    {
        PimsActivityInstance GetById(long id);

        IList<PimsActivityInstance> GetAllByResearchFileId(long researchFileId);

        IList<PimsActivityTemplate> GetAllActivityTemplates();

        PimsActivityInstance Add(PimsActivityInstance instance);

        PimsActivityInstance Update(PimsActivityInstance model);

        void Delete(long activityId);
    }
}
