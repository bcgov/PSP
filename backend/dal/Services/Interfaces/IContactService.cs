using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Services.Interfaces
{
    public interface IContactService
    {
        PimsContactMgrVw GetById(string id);
        Paged<PimsContactMgrVw> GetPage(ContactFilter filter);
    }
}
