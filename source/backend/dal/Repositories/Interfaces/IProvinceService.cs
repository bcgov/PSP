using System.Collections.Generic;
using Pims.Dal.Entities;

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
