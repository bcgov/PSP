using Pims.Dal.Entities.Models;
using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Services
{
    /// <summary>
    /// IContactService interface, provides functions to interact with contacts within the datasource.
    /// </summary>
    public interface IContactService : IService<PimsContactMgrVw>
    {
        int Count();
        IEnumerable<PimsContactMgrVw> Get(ContactFilter filter);
        Paged<PimsContactMgrVw> GetPage(ContactFilter filter);
    }
}
