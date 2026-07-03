using System;
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
        Task<Paged<PimsProject>> GetPageAsync(ProjectFilter filter, UserContextModel userContext);

        IList<PimsProject> SearchProjects(string filter, int maxResults, UserContextModel userContext);

        PimsProject Add(PimsProject project);

        PimsProject TryGet(long id);

        IEnumerable<PimsProject> GetAllByName(string name);

        PimsProject Update(PimsProject project);

        PimsProject GetProjectAtTime(long projectId, DateTime time);

        IEnumerable<PimsProjectPerson> GetTeamMembers(UserContextModel userContext);
    }
}
