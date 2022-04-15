using Pims.Dal.Entities.Models;
using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IContactRepository interface, provides functions to interact with contacts within the datasource.
    /// </summary>
    public interface IContactRepository : IRepository<PimsContactMgrVw>
    {
        int Count();
        IEnumerable<PimsContactMgrVw> Get(ContactFilter filter);
        PimsContactMgrVw Get(string id);
        Paged<PimsContactMgrVw> GetPage(ContactFilter filter);
    }
}
