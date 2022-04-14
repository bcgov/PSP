using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Services
{
    public interface IResearchFileService
    {
        PimsResearchFile Add(PimsResearchFile researchFile);
        Paged<PimsResearchFile> GetPage(ResearchFilter filter);
    }
}
