using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// ITenantRepository interface, provides functions to interact with tenants within the datasource.
    /// </summary>
    public interface ITenantRepository : IRepository<PimsTenant>
    {
        PimsTenant GetTenant(string code);

        PimsTenant UpdateTenant(PimsTenant tenant);
    }
}
