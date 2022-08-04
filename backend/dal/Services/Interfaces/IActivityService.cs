using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using System.Collections.Generic;

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
