using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IInsuranceService interface, provides functions to interact with contacts within the datasource.
    /// </summary>
    public interface IInsuranceRepository : IRepository<PimsInsurance>
    {
        PimsInsurance Add(PimsInsurance insurance, bool commit = true);
        PimsInsurance Update(PimsInsurance insurance, bool commit = true);
        PimsInsurance Delete(PimsInsurance insurance, bool commit = true);
    }
}
