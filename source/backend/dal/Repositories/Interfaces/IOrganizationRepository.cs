using System;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Interface that provides a service layer to administer organizations within the datasource.
    /// </summary>
    public interface IOrganizationRepository : IRepository<PimsOrganization>
    {
        long GetRowVersion(long id);

        PimsOrganization GetById(long id);

        PimsOrganization Add(PimsOrganization organization, bool userOverride);

        PimsOrganization Update(PimsOrganization organization);

        PimsOrganization GetOrganizationAtTime(long organizationId, DateTime time);
    }
}
