using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Services
{
    public interface IResearchFileService
    {
        PimsResearchFile GetById(long id);
        PimsResearchFile Add(PimsResearchFile researchFile);
        PimsResearchFile Update(PimsResearchFile researchFile);
        Paged<PimsResearchFile> GetPage(ResearchFilter filter);
    }
}
