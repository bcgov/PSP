using Pims.Api.Models.CodeTypes;

namespace Pims.Api.Services
{
    public interface IDispositionStatusSolver
    {
        bool CanEditDetails(DispositionFileStatusTypes? dispositionStatus);

        bool CanEditChecklists(DispositionFileStatusTypes? dispositionStatus);

        bool CanEditOfferSalesValues(DispositionFileStatusTypes? dispositionStatus);

        bool CanEditProperties(DispositionFileStatusTypes? dispositionStatus);

        DispositionFileStatusTypes? GetCurrentDispositionStatus(string pimsDispositionStatusType);
    }
}
