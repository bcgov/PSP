using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using System;

namespace Pims.Dal.Services
{
    /// <summary>
    /// IRoleService interface, provides a service layer to administer roles within the datasource.
    /// </summary>
    public interface IRoleService : IService<Role>
    {
        Paged<Role> Get(int page, int quantity, string name = null);
        Role Get(Guid key);
        Role GetByName(string name);
        Role GetByKeycloakId(Guid key);
        Role Add(Role add);
        void AddWithoutSave(Role add);
        Role Update(Role update);
        Role UpdateWithoutSave(Role update);
        void Delete(Role delete);
        int RemoveAll(Guid[] exclude);
    }
}
