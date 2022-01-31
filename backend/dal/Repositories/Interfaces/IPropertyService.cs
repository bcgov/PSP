using Pims.Dal.Entities.Models;
using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IPropertyService interface, provides functions to interact with properties within the datasource.
    /// </summary>
    public interface IPropertyService : IRepository<PimsProperty>
    {
        int Count();
        IEnumerable<PimsProperty> Get(PropertyFilter filter);
        Paged<PimsProperty> GetPage(PropertyFilter filter);
        PimsProperty Get(int id);
        PimsProperty GetForPID(string pid);
    }
}
