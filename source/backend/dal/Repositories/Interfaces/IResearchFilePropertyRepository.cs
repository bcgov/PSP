using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IResearchFilePropertyRepository : IRepository
    {
        List<PimsPropertyResearchFile> GetAllByResearchFileId(long researchFileId);

        int GetResearchFilePropertyRelatedCount(long propertyId);

        PimsPropertyResearchFile Add(PimsPropertyResearchFile propertyResearchFile);

        PimsPropertyResearchFile Update(PimsPropertyResearchFile propertyResearchFile);

        void Delete(PimsPropertyResearchFile propertyResearchFile);
    }
}
