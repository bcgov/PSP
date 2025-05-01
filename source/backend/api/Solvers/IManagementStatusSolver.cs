using Pims.Api.Models.CodeTypes;

namespace Pims.Api.Services
{
    public interface IManagementStatusSolver
    {
        bool CanEditDetails(ManagementFileStatusTypes? managementStatus);

        bool CanEditProperties(ManagementFileStatusTypes? managementStatus);

        ManagementFileStatusTypes? GetCurrentManagementStatus(string pimsManagementStatusType);
    }
}
