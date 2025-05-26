using Pims.Api.Models.CodeTypes;

namespace Pims.Api.Services
{
    public interface IManagementFileStatusSolver
    {
        bool IsAdminProtected(ManagementFileStatusTypes? managementStatus);

        bool CanEditDetails(ManagementFileStatusTypes? managementStatus);

        bool CanEditProperties(ManagementFileStatusTypes? managementStatus);

        bool CanEditDocuments(ManagementFileStatusTypes? managementStatus);

        bool CanEditNotes(ManagementFileStatusTypes? managementStatus);

        bool CanEditActivities(ManagementFileStatusTypes? managementStatus);

        ManagementFileStatusTypes? GetCurrentManagementStatus(string pimsManagementStatusType);
    }
}
