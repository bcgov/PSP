using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Services
{
    /// <summary>
    /// Interface that provides a service layer to administer system variables within the datasource.
    /// </summary>
    public interface ISystemConstantService : IService<PimsStaticVariable>
    {
        IEnumerable<PimsStaticVariable> GetAll();
    }
}
