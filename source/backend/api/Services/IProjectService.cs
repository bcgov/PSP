using System.Collections.Generic;
using System.Threading.Tasks;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Api.Services
{
    public interface IProjectService
    {
        IList<PimsProject> SearchProjects(string filter, int maxResult);

        IList<PimsProject> GetAll();

        Task<Paged<PimsProject>> GetPage(ProjectFilter filter);

        PimsProject Add(PimsProject project);

        PimsProject GetById(long projectId);

        IList<PimsProduct> GetProducts(long projectId);

        List<PimsAcquisitionFile> GetProductFiles(long productId);

        PimsProject Update(PimsProject project);
    }
}
