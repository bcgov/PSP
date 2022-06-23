using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IAccessRequestRepository interface, provides functions to interact with access requests within the datasource.
    /// </summary>
    public interface IAccessRequestRepository : IRepository<PimsAccessRequest>
    {
        Paged<PimsAccessRequest> Get(AccessRequestFilter filter);
        PimsAccessRequest Get();
        PimsAccessRequest Get(long id);
        PimsAccessRequest Add(PimsAccessRequest addRequest);
        PimsAccessRequest Update(PimsAccessRequest updateRequest);
        PimsAccessRequest Delete(PimsAccessRequest deleteRequest);
    }
}
