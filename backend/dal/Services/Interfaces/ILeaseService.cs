using Pims.Dal.Entities.Models;
using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Services
{
    /// <summary>
    /// ILeaseService interface, provides functions to interact with leases within the datasource.
    /// </summary>
    public interface ILeaseService : IService<Lease>
    {
        int Count();
        IEnumerable<Lease> Get(LeaseFilter filter);
        Paged<Lease> GetPage(LeaseFilter filter);
    }
}
