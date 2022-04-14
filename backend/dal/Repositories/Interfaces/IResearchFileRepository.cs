using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IResearchFileRepository : IRepository
    {
        PimsResearchFile Add(PimsResearchFile researchFile);
    }
}
