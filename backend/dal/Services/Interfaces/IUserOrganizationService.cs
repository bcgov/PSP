using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IOrganizationService interface, provides a service layer to administer organizations within the datasource.
    /// </summary>
    public interface IUserOrganizationService : IRepository<PimsOrganization>
    {
        IEnumerable<PimsOrganization> GetAll();
        PimsOrganization Get(long id);
        IEnumerable<PimsOrganization> GetChildren(long parentId);
        Paged<PimsOrganization> Get(int page, int quantity);
        Paged<PimsOrganization> Get(OrganizationFilter filter = null);
        PimsOrganization Add(PimsOrganization add);
        PimsOrganization Update(PimsOrganization update);
        void Delete(PimsOrganization delete);
    }
}
