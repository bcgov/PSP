using System;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IRoleService interface, provides a service layer to administer roles within the datasource.
    /// </summary>
    public interface IRoleService : IRepository<PimsRole>
    {
        Paged<PimsRole> Get(int page, int quantity, string name = null);

        PimsRole Get(Guid key);

        PimsRole GetByName(string name);

        PimsRole GetByKeycloakId(Guid key);

        PimsRole Add(PimsRole add);

        void AddWithoutSave(PimsRole add);

        PimsRole Update(PimsRole update);

        PimsRole UpdateWithoutSave(PimsRole update);

        void Delete(PimsRole delete);

        int RemoveAll(Guid[] exclude);
    }
}
