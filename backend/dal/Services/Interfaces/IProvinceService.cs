using Pims.Dal.Entities;
using System.Collections.Generic;

namespace Pims.Dal.Services
{
    /// <summary>
    /// IProvinceService interface, provides a service layer to administer provinces within the datasource.
    /// </summary>
    public interface IProvinceService : IService<PimsProvinceState>
    {
        IEnumerable<PimsProvinceState> Get();
    }
}
