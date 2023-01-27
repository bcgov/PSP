using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Services
{
    public interface IActivityService
    {
        PimsActivityInstance GetById(long id);

        IList<PimsActivityInstance> GetAllByResearchFileId(long researchFileId);

        IList<PimsActivityTemplate> GetAllActivityTemplates();

        PimsActivityInstance Add(PimsActivityInstance instance);
    }
}
