using System;
using Pims.Api.Models.CodeTypes;

namespace Pims.Api.Services
{
    public class ManagementFileStatusSolver : IManagementFileStatusSolver
    {
        public bool IsAdminProtected(ManagementFileStatusTypes? managementStatus)
        {
            if (managementStatus == null)
            {
                return false;
            }

            var canEdit = managementStatus switch
            {
                ManagementFileStatusTypes.ACTIVE or ManagementFileStatusTypes.DRAFT or ManagementFileStatusTypes.THIRDRDPARTY or ManagementFileStatusTypes.HOLD or ManagementFileStatusTypes.CANCELLED => false,
                ManagementFileStatusTypes.ARCHIVED or ManagementFileStatusTypes.COMPLETE => true,
                _ => false,
            };
            return canEdit;
        }

        public bool CanEditDetails(ManagementFileStatusTypes? managementStatus)
        {
            if (managementStatus == null)
            {
                return false;
            }

            var canEdit = managementStatus switch
            {
                ManagementFileStatusTypes.ACTIVE or ManagementFileStatusTypes.DRAFT or ManagementFileStatusTypes.THIRDRDPARTY => true,
                ManagementFileStatusTypes.HOLD or ManagementFileStatusTypes.ARCHIVED or ManagementFileStatusTypes.CANCELLED or ManagementFileStatusTypes.COMPLETE => false,
                _ => false,
            };
            return canEdit;
        }

        public bool CanEditProperties(ManagementFileStatusTypes? managementStatus)
        {
            if (managementStatus == null)
            {
                return false;
            }

            var canEdit = managementStatus switch
            {
                ManagementFileStatusTypes.ACTIVE or ManagementFileStatusTypes.DRAFT or ManagementFileStatusTypes.THIRDRDPARTY => true,
                ManagementFileStatusTypes.ARCHIVED or ManagementFileStatusTypes.HOLD or ManagementFileStatusTypes.CANCELLED or ManagementFileStatusTypes.COMPLETE => false,
                _ => false,
            };
            return canEdit;
        }

        public bool CanEditDocuments(ManagementFileStatusTypes? managementStatus)
        {
            if (managementStatus == null)
            {
                return false;
            }

            var canEdit = managementStatus switch
            {
                ManagementFileStatusTypes.ACTIVE or ManagementFileStatusTypes.DRAFT or ManagementFileStatusTypes.HOLD or ManagementFileStatusTypes.CANCELLED or ManagementFileStatusTypes.COMPLETE or ManagementFileStatusTypes.THIRDRDPARTY => true,
                ManagementFileStatusTypes.ARCHIVED => false,
                _ => false,
            };

            return canEdit;
        }

        public bool CanEditNotes(ManagementFileStatusTypes? managementStatus)
        {
            if (managementStatus == null)
            {
                return false;
            }

            var canEditNotes = managementStatus switch
            {
                ManagementFileStatusTypes.ACTIVE or ManagementFileStatusTypes.DRAFT or ManagementFileStatusTypes.HOLD or ManagementFileStatusTypes.CANCELLED or ManagementFileStatusTypes.COMPLETE or ManagementFileStatusTypes.THIRDRDPARTY => true,
                ManagementFileStatusTypes.ARCHIVED => false,
                _ => false,
            };

            return canEditNotes;
        }

        public bool CanEditActivities(ManagementFileStatusTypes? managementStatus)
        {
            if (managementStatus == null)
            {
                return false;
            }

            var canEditActivities = managementStatus switch
            {
                ManagementFileStatusTypes.ACTIVE or ManagementFileStatusTypes.DRAFT or ManagementFileStatusTypes.THIRDRDPARTY => true,
                ManagementFileStatusTypes.HOLD or ManagementFileStatusTypes.ARCHIVED or ManagementFileStatusTypes.CANCELLED or ManagementFileStatusTypes.COMPLETE => false,
                _ => false,
            };
            return canEditActivities;
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
