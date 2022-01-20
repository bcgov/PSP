using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Interface that provides a service layer to administer organizations within the datasource.
    /// </summary>
    public interface IOrganizationService : IRepository<PimsOrganization>
    {
        IEnumerable<PimsOrganization> GetAll();
        PimsOrganization Get(long id);
        PimsOrganization Add(PimsOrganization organization, bool userOverride);
    }
}
