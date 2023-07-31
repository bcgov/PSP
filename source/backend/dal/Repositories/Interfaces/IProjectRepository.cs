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

        Task<Paged<PimsProject>> GetPageAsync(ProjectFilter filter, IEnumerable<short> userRegions);

        IList<PimsProject> SearchProjects(string filter, HashSet<short> regions, int maxResults);

        PimsProject Add(PimsProject project);

        PimsProject Get(long id);

        PimsProject Update(PimsProject project);

        PimsProjectDocument AddProjectDocument(PimsProjectDocument projectDocument);

        IList<PimsProjectDocument> GetAllProjectDocuments(long projectId);

        IList<PimsProjectDocument> GetAllByDocument(long documentId);

        void DeleteProjectDocument(long projectDocumentId);
    }
}
