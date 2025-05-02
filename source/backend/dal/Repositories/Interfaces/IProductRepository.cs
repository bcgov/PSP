using System;
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

        IList<PimsProjectProduct> GetProjectProductsByProject(long projectId);

        IEnumerable<PimsProduct> GetProducts(IEnumerable<PimsProduct> incomingProducts);

        IEnumerable<PimsProduct> GetByProductBatch(IEnumerable<PimsProduct> incomingProducts, long projectId);

        PimsProduct GetProductAtTime(long productId, DateTime time);
    }
}
