using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Repository that provides functionality to manage Products.
    /// </summary>
    public interface IProductRepository : IRepository<PimsProduct>
    {
        IList<PimsProduct> GetByProject(long projectId);

        IEnumerable<PimsProduct> GetByProductBatch(IEnumerable<PimsProduct> incomingProducts, long projectId);
    }
}
