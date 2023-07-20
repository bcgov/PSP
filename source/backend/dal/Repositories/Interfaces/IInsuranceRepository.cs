using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IInsuranceService interface, provides functions to interact with contacts within the datasource.
    /// </summary>
    public interface IInsuranceRepository : IRepository<PimsInsurance>
    {
        IEnumerable<PimsInsurance> GetByLeaseId(long leaseId);

        IEnumerable<PimsInsurance> UpdateLeaseInsurance(long leaseId, IEnumerable<PimsInsurance> insurances);
    }
}
