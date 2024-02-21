using Pims.Api.Constants;

namespace Pims.Api.Services
{
    public interface IDispositionStatusSolver
    {
        bool CanEditDetails(DispositionStatusTypes? dispositionStatus);

        bool CanEditProperties(DispositionStatusTypes? dispositionStatus);

        bool CanEditOrDeleteValuesOffersSales(DispositionStatusTypes? dispositionStatus);
    }
}
