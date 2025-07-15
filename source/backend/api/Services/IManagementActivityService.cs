using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Api.Services
{
    public interface IManagementActivityService
    {
        Paged<PimsManagementActivity> GetPage(ManagementActivityFilter filter, bool? all = false);
    }
}
