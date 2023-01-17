using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IContactRepository interface, provides functions to interact with contacts within the datasource.
    /// </summary>
    public interface IContactRepository : IRepository<PimsContactMgrVw>
    {
        int Count();

        IEnumerable<PimsContactMgrVw> GetAll(ContactFilter filter);

        PimsContactMgrVw GetById(string id);

        Paged<PimsContactMgrVw> GetPage(ContactFilter filter);
    }
}
