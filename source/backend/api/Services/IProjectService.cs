using System.Collections.Generic;
using System.Threading.Tasks;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;

namespace Pims.Api.Services
{
    public interface IProjectService
    {
        IList<PimsProject> SearchProjects(string filter, int maxResult);

        IList<PimsProject> GetAll();

        Task<Paged<PimsProject>> GetPage(ProjectFilter filter);

        PimsProject GetById(long projectId);

        IList<PimsProduct> GetProducts(long projectId);

        List<PimsAcquisitionFile> GetProductFiles(long productId);

        PimsProject Add(PimsProject project, IEnumerable<UserOverrideCode> userOverrides);

        PimsProject Update(PimsProject project, IEnumerable<UserOverrideCode> userOverrides);
    }
}
