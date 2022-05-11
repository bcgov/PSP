using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Repositories
{
    public interface IResearchFileRepository : IRepository
    {
        PimsResearchFile GetById(long id);
        PimsResearchFile Add(PimsResearchFile researchFile);
        PimsResearchFile Update(PimsResearchFile researchFile);
        Paged<PimsResearchFile> GetPage(ResearchFilter filter);
        PimsResearchFile UpdateProperty(long researchFileId, PimsPropertyResearchFile researchFileProperty);
        long GetRowVersion(long id);
    }
}
