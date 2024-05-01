using Pims.Api.Models.CodeTypes;

namespace Pims.Api.Services
{
    public interface IDispositionStatusSolver
    {
        bool CanEditDetails(DispositionFileStatusTypes? dispositionStatus);
    }
}
