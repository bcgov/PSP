using System.Collections.Generic;
using System.Threading.Tasks;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IProject Repository.
    /// </summary>
    public interface IProjectRepository : IRepository<PimsProject>
    {
        int Count();

        Task<Paged<PimsProject>> GetPageAsync(ProjectFilter filter);

        IList<PimsProject> SearchProjects(string filter, int maxResult);

        PimsProject Add(PimsProject project);

        PimsProject Get(long id);
		PimsProject Update(PimsProject project);

		long GetRowVersion(long id);
    }
}
