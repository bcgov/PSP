
namespace Pims.Dal.Services
{
    public interface IPimsService
    {
        #region Leases
        ILeaseTermService LeaseTermService { get; }
        ILeaseService LeaseService { get; }
        #region Insurance
        IInsuranceService Insurance { get; }
        #endregion

        #region Autocomplete
        IAutocompleteService Autocomplete { get; }
        #endregion

        #endregion
    }
}
