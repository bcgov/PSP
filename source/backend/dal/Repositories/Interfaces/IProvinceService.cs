using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IProvinceRepository interface, provides a service layer to administer provinces within the datasource.
    /// </summary>
    public interface IProvinceRepository : IRepository<PimsProvinceState>
    {
        IEnumerable<PimsProvinceState> GetAll();
    }
}
