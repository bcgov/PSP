using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// ITenantService interface, provides functions to interact with tenants within the datasource.
    /// </summary>
    public interface ITenantService : IRepository<PimsTenant>
    {
        PimsTenant GetTenant(string code);

        PimsTenant UpdateTenant(PimsTenant tenant);
    }
}
