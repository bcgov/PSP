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

        long GetRowVersion(long id);
    }
}
