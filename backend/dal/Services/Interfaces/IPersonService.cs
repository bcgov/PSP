using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Services
{
    /// <summary>
    /// Interface that provides a service layer to administer persons within the datasource.
    /// </summary>
    public interface IPersonService : IService<PimsPerson>
    {
        IEnumerable<PimsPerson> GetAll();
        PimsPerson Get(long id);
        PimsPerson Add(PimsPerson person, bool userOverride);
    }
}
