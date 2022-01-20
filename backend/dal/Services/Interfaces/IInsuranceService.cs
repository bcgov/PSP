using Pims.Dal.Entities;

namespace Pims.Dal.Services
{
    /// <summary>
    /// IInsuranceService interface, provides functions to interact with contacts within the datasource.
    /// </summary>
    public interface IInsuranceService : IRepository<PimsInsurance>
    {
        PimsInsurance Add(PimsInsurance insurance, bool commit = true);
        PimsInsurance Update(PimsInsurance insurance, bool commit = true);
        PimsInsurance Delete(PimsInsurance insurance, bool commit = true);
    }
}
