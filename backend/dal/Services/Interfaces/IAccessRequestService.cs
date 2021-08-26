using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Services
{
    /// <summary>
    /// IAccessRequestService interface, provides functions to interact with access requests within the datasource.
    /// </summary>
    public interface IAccessRequestService : IService<AccessRequest>
    {
        Paged<AccessRequest> Get(AccessRequestFilter filter);
        AccessRequest Get();
        AccessRequest Get(long id);
        AccessRequest Add(AccessRequest request);
        AccessRequest Update(AccessRequest request);
        AccessRequest Delete(AccessRequest accessRequest);
    }
}
