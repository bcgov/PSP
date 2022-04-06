namespace Pims.Dal.Repositories
{
    using Pims.Dal.Entities;
    using Pims.Dal.Entities.Models;

    /// <summary>
    /// IResearchRepository interface, provides functions to interact with research within the datasource.
    /// </summary>
    public interface IResearchRepository : IRepository<PimsResearchFile>
    {
        Paged<PimsResearchFile> GetPage(ResearchFilter filter);
    }
}
