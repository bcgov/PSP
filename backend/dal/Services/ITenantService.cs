using Pims.Dal.Entities;

namespace Pims.Dal.Services
{
    /// <summary>
    /// ITenantService interface, provides functions to interact with tenants within the datasource.
    /// </summary>
    public interface ITenantService : IService
    {
        Tenant GetTenant(string code);

        Tenant UpdateTenant(Tenant tenant);
    }
}
