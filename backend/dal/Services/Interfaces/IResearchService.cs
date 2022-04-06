using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Services
{
    public interface IResearchService
    {
        Paged<PimsResearchFile> GetPage(ResearchFilter filter);
    }
}
