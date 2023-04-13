using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface IOrganizationService
    {
        PimsOrganization GetOrganization(long id);

        PimsOrganization AddOrganization(PimsOrganization organization, bool userOverride);

        PimsOrganization UpdateOrganization(PimsOrganization organization, long? rowVersion);
    }
}
