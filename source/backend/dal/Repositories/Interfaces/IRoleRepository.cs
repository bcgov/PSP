using System;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IRoleRepository interface, provides a service layer to administer roles within the datasource.
    /// </summary>
    public interface IRoleRepository : IRepository<PimsRole>
    {
        Paged<PimsRole> GetPage(int page, int quantity, string name = null);

        PimsRole GetByKey(Guid key);
    }
}
