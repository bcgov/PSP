using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Interface that provides a service layer to administer system variables within the datasource.
    /// </summary>
    public interface ISystemConstantRepository : IRepository<PimsStaticVariable>
    {
        IEnumerable<PimsStaticVariable> GetAll();
    }
}
