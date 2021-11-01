using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Services
{
    /// <summary>
    /// IOrganizationService interface, provides a service layer to administer organizations within the datasource.
    /// </summary>
    public interface IOrganizationService : IService<Organization>
    {
        IEnumerable<Organization> GetAll();
        Organization Get(long id);
        IEnumerable<Organization> GetChildren(long parentId);
        Paged<Organization> Get(int page, int quantity);
        Paged<Organization> Get(OrganizationFilter filter = null);
        Organization Add(Organization add);
        Organization Update(Organization update);
        void Delete(Organization delete);
    }
}
