using System;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IClaimRepository interface, provides a service layer to administer roles within the datasource.
    /// </summary>
    public interface IClaimRepository : IRepository<PimsClaim>
    {
        Paged<PimsClaim> GetPage(int page, int quantity, string name = null);

        PimsClaim GetByKey(Guid key);

        PimsClaim GetByName(string name);

        int RemoveAll(Guid[] exclude);

        PimsClaim Add(PimsClaim add);

        PimsClaim Update(PimsClaim update);

        void Delete(PimsClaim delete);
    }
}
