using Pims.Dal.Entities;
using System.Collections.Generic;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IProvinceService interface, provides a service layer to administer provinces within the datasource.
    /// </summary>
    public interface IProvinceService : IRepository<PimsProvinceState>
    {
        IEnumerable<PimsProvinceState> Get();
    }
}
