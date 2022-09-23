using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IPropertyRepository interface, provides functions to interact with properties within the datasource.
    /// </summary>
    public interface IPropertyRepository : IRepository<PimsProperty>
    {
        int Count();

        IEnumerable<PimsProperty> Get(PropertyFilter filter);

        Paged<PimsProperty> GetPage(PropertyFilter filter);

        PimsProperty Get(long id);

        PimsProperty GetByPid(string pid);

        PimsProperty GetByPid(int pid);

        PimsProperty GetAssociations(long id);

        PimsProperty Update(PimsProperty property);

        void Delete(PimsProperty property);
    }
}
