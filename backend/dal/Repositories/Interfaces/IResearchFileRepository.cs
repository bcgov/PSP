using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Repositories
{
    public interface IResearchFileRepository : IRepository
    {
        PimsResearchFile Add(PimsResearchFile researchFile);
        Paged<PimsResearchFile> GetPage(ResearchFilter filter);
    }
}
