using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IProjectRepository : IRepository<PimsProject>
    {
        IList<PimsProject> SearchProjects(string filter, int maxResult);
    }
}
