using System;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IClaimService interface, provides a service layer to administer roles within the datasource.
    /// </summary>
    public interface IClaimService : IRepository<PimsClaim>
    {
        Paged<PimsClaim> Get(int page, int quantity, string name = null);

        PimsClaim Get(Guid key);

        PimsClaim GetByName(string name);

        int RemoveAll(Guid[] exclude);

        PimsClaim Add(PimsClaim add);

        PimsClaim Update(PimsClaim update);

        void Delete(PimsClaim delete);
    }
}
