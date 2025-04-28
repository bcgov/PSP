using System;
using Pims.Api.Models.CodeTypes;

namespace Pims.Api.Services
{
    public class ManagementStatusSolver : IManagementStatusSolver
    {
        public bool CanEditDetails(ManagementFileStatusTypes? managementStatus)
        {
            if (managementStatus == null)
            {
                return false;
            }

            bool canEdit;
            switch (managementStatus)
            {
                case ManagementFileStatusTypes.ACTIVE:
                case ManagementFileStatusTypes.DRAFT:
                case ManagementFileStatusTypes.HOLD:
                case ManagementFileStatusTypes.THIRDRDPARTY:
                    canEdit = true;
                    break;
                case ManagementFileStatusTypes.ARCHIVED:
                case ManagementFileStatusTypes.CANCELLED:
                case ManagementFileStatusTypes.COMPLETE:
                    canEdit = false;
                    break;
                default:
                    canEdit = false;
                    break;
            }

            return canEdit;
        }

        public bool CanEditProperties(ManagementFileStatusTypes? managementStatus)
        {
            if (managementStatus == null)
            {
                return false;
            }

            bool canEdit;
            switch (managementStatus)
            {
                case ManagementFileStatusTypes.ACTIVE:
                case ManagementFileStatusTypes.DRAFT:
                case ManagementFileStatusTypes.HOLD:
                case ManagementFileStatusTypes.THIRDRDPARTY:
                    canEdit = true;
                    break;
                case ManagementFileStatusTypes.ARCHIVED:
                case ManagementFileStatusTypes.CANCELLED:
                case ManagementFileStatusTypes.COMPLETE:
                    canEdit = false;
                    break;
                default:
                    canEdit = false;
                    break;
            }

            return canEdit;
        }

        public ManagementFileStatusTypes? GetCurrentManagementStatus(string pimsManagementStatusType)
        {
            ManagementFileStatusTypes currentManagementStatus;
            if (Enum.TryParse(pimsManagementStatusType, out currentManagementStatus))
            {
                return currentManagementStatus;
            }

            return currentManagementStatus;
        }
    }
}
