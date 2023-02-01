using System.Collections.Generic;
using System.Threading.Tasks;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Api.Services
{
    public interface IProjectService
    {
        IList<PimsProject> SearchProjects(string filter, int maxResult);

        Task<Paged<PimsProject>> GetPage(ProjectFilter filter);

        Task<PimsProject> Add(PimsProject project);

        Task<PimsProject> GetById(long projectId);
        
        IList<PimsProduct> GetProducts(int projectId);
    }
}
