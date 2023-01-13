using System.Threading.Tasks;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Api.Services
{
    public interface IProjectService
    {
        Task<Paged<PimsProject>> GetPage(ProjectFilter filter);
    }
}
