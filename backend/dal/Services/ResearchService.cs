using Pims.Dal.Repositories;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Services
{
    public class ResearchService : IResearchService
    {
        readonly IResearchRepository _researchRepository;
        public ResearchService(IResearchRepository researchRepository)
        {
            _researchRepository = researchRepository;
        }
        public Paged<PimsResearchFile> GetPage(ResearchFilter filter)
        {
            return _researchRepository.GetPage(filter);
        }
    }
}
