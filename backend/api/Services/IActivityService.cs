using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface IActivityService
    {
        PimsActivityInstance GetById(long id);

        IList<PimsActivityInstance> GetAllByResearchFileId(long researchFileId);

        IList<PimsActivityInstance> GetAllByAcquisitionFileId(long acquisitionFileId);

        IList<PimsActivityTemplate> GetAllActivityTemplates();

        PimsActivityInstance Add(PimsActivityInstance instance);

        PimsActivityInstance AddResearchActivity(PimsActivityInstance instance, long researchFileId);

        PimsActivityInstance AddAcquisitionActivity(PimsActivityInstance instance, long acquisitionFileId);

        PimsActivityInstance Update(PimsActivityInstance model);

        void Delete(long activityId);
    }
}
