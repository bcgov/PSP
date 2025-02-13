using System;
using Pims.Api.Models.CodeTypes;

namespace Pims.Api.Services
{
    public class LeaseStatusSolver : ILeaseStatusSolver
    {

        public bool CanEditDetails(LeaseStatusTypes? leaseStatus)
        {
            if (leaseStatus == null)
            {
                return false;
            }

            bool canEdit;
            switch (leaseStatus)
            {
                case LeaseStatusTypes.ACTIVE:
                case LeaseStatusTypes.DRAFT:
                case LeaseStatusTypes.INACTIVE:
                    canEdit = true;
                    break;
                case LeaseStatusTypes.TERMINATED:
                case LeaseStatusTypes.DUPLICATE:
                case LeaseStatusTypes.DISCARD:
                case LeaseStatusTypes.ARCHIVED:
                case LeaseStatusTypes.EXPIRED:
                    canEdit = false;
                    break;
                default:
                    canEdit = false;
                    break;
            }

            return canEdit;
        }

        public bool CanEditProperties(LeaseStatusTypes? leaseStatus)
        {
            if (leaseStatus == null)
            {
                return false;
            }

            bool canEdit;
            switch (leaseStatus)
            {
                case LeaseStatusTypes.ACTIVE:
                case LeaseStatusTypes.DRAFT:
                case LeaseStatusTypes.INACTIVE:
                    canEdit = true;
                    break;
                case LeaseStatusTypes.TERMINATED:
                case LeaseStatusTypes.DUPLICATE:
                case LeaseStatusTypes.DISCARD:
                case LeaseStatusTypes.ARCHIVED:
                case LeaseStatusTypes.EXPIRED:
                    canEdit = false;
                    break;
                default:
                    canEdit = false;
                    break;
            }

            return canEdit;
        }

        public bool CanEditOrDeleteCompensation(LeaseStatusTypes? leaseStatus, bool? isDraftCompensation, bool? isAdmin)
        {
            if (leaseStatus == null)
            {
                return false;
            }

            bool canEdit;
            switch (leaseStatus)
            {
                case LeaseStatusTypes.ACTIVE:
                case LeaseStatusTypes.DRAFT:
                case LeaseStatusTypes.INACTIVE:
                    canEdit = (isDraftCompensation.HasValue && isDraftCompensation.Value) || (isAdmin.HasValue && isAdmin.Value);
                    break;
                case LeaseStatusTypes.TERMINATED:
                case LeaseStatusTypes.DUPLICATE:
                case LeaseStatusTypes.DISCARD:
                case LeaseStatusTypes.ARCHIVED:
                case LeaseStatusTypes.EXPIRED:
                    canEdit = false;
                    break;
                default:
                    canEdit = false;
                    break;
            }

            return canEdit;
        }

        public bool CanEditOrDeleteConsultation(LeaseStatusTypes? leaseStatus)
        {
            if (leaseStatus == null)
            {
                return false;
            }

            bool canEdit;
            switch (leaseStatus)
            {
                case LeaseStatusTypes.ACTIVE:
                case LeaseStatusTypes.DRAFT:
                case LeaseStatusTypes.INACTIVE:
                    canEdit = true;
                    break;
                case LeaseStatusTypes.TERMINATED:
                case LeaseStatusTypes.DUPLICATE:
                case LeaseStatusTypes.DISCARD:
                case LeaseStatusTypes.ARCHIVED:
                case LeaseStatusTypes.EXPIRED:
                    canEdit = false;
                    break;
                default:
                    canEdit = false;
                    break;
            }

            return canEdit;
        }

        public bool CanEditChecklists(LeaseStatusTypes? leaseStatus)
        {
            if (leaseStatus == null)
            {
                return false;
            }

            bool canEdit;
            switch (leaseStatus)
            {
                case LeaseStatusTypes.ACTIVE:
                case LeaseStatusTypes.DRAFT:
                case LeaseStatusTypes.INACTIVE:
                    canEdit = true;
                    break;
                case LeaseStatusTypes.TERMINATED:
                case LeaseStatusTypes.DUPLICATE:
                case LeaseStatusTypes.DISCARD:
                case LeaseStatusTypes.ARCHIVED:
                case LeaseStatusTypes.EXPIRED:
                    canEdit = false;
                    break;
                default:
                    canEdit = false;
                    break;
            }

            return canEdit;
        }

        public bool CanEditStakeholders(LeaseStatusTypes? leaseStatus)
        {
            if (leaseStatus == null)
            {
                return false;
            }

            bool canEdit;
            switch (leaseStatus)
            {
                case LeaseStatusTypes.ACTIVE:
                case LeaseStatusTypes.DRAFT:
                case LeaseStatusTypes.INACTIVE:
                    canEdit = true;
                    break;
                case LeaseStatusTypes.TERMINATED:
                case LeaseStatusTypes.DUPLICATE:
                case LeaseStatusTypes.DISCARD:
                case LeaseStatusTypes.ARCHIVED:
                case LeaseStatusTypes.EXPIRED:
                    canEdit = false;
                    break;
                default:
                    canEdit = false;
                    break;
            }

            return canEdit;
        }

        public bool CanEditImprovements(LeaseStatusTypes? leaseStatus)
        {
            if (leaseStatus == null)
            {
                return false;
            }

            bool canEdit;
            switch (leaseStatus)
            {
                case LeaseStatusTypes.ACTIVE:
                case LeaseStatusTypes.DRAFT:
                case LeaseStatusTypes.INACTIVE:
                    canEdit = true;
                    break;
                case LeaseStatusTypes.TERMINATED:
                case LeaseStatusTypes.DUPLICATE:
                case LeaseStatusTypes.DISCARD:
                case LeaseStatusTypes.ARCHIVED:
                case LeaseStatusTypes.EXPIRED:
                    canEdit = false;
                    break;
                default:
                    canEdit = false;
                    break;
            }

            return canEdit;
        }

        public bool CanEditInsurance(LeaseStatusTypes? leaseStatus)
        {
            if (leaseStatus == null)
            {
                return false;
            }

            bool canEdit;
            switch (leaseStatus)
            {
                case LeaseStatusTypes.ACTIVE:
                case LeaseStatusTypes.DRAFT:
                case LeaseStatusTypes.INACTIVE:
                    canEdit = true;
                    break;
                case LeaseStatusTypes.TERMINATED:
                case LeaseStatusTypes.DUPLICATE:
                case LeaseStatusTypes.DISCARD:
                case LeaseStatusTypes.ARCHIVED:
                case LeaseStatusTypes.EXPIRED:
                    canEdit = false;
                    break;
                default:
                    canEdit = false;
                    break;
            }

            return canEdit;
        }

        public bool CanEditDeposits(LeaseStatusTypes? leaseStatus)
        {
            if (leaseStatus == null)
            {
                return false;
            }

            bool canEdit;
            switch (leaseStatus)
            {
                case LeaseStatusTypes.ACTIVE:
                case LeaseStatusTypes.DRAFT:
                case LeaseStatusTypes.INACTIVE:
                    canEdit = true;
                    break;
                case LeaseStatusTypes.TERMINATED:
                case LeaseStatusTypes.DUPLICATE:
                case LeaseStatusTypes.DISCARD:
                case LeaseStatusTypes.ARCHIVED:
                case LeaseStatusTypes.EXPIRED:
                    canEdit = false;
                    break;
                default:
                    canEdit = false;
                    break;
            }

            return canEdit;
        }

        public bool CanEditPayments(LeaseStatusTypes? leaseStatus)
        {
            if (leaseStatus == null)
            {
                return false;
            }

            bool canEdit;
            switch (leaseStatus)
            {
                case LeaseStatusTypes.ACTIVE:
                case LeaseStatusTypes.DRAFT:
                case LeaseStatusTypes.INACTIVE:
                    canEdit = true;
                    break;
                case LeaseStatusTypes.TERMINATED:
                case LeaseStatusTypes.DUPLICATE:
                case LeaseStatusTypes.DISCARD:
                case LeaseStatusTypes.ARCHIVED:
                case LeaseStatusTypes.EXPIRED:
                    canEdit = false;
                    break;
                default:
                    canEdit = false;
                    break;
            }

            return canEdit;
        }

        public LeaseStatusTypes? GetCurrentLeaseStatus(string pimsLeaseStatusType)
        {
            LeaseStatusTypes currentLeaseStatus;
            if (Enum.TryParse(pimsLeaseStatusType, out currentLeaseStatus))
            {
                return currentLeaseStatus;
            }

            return currentLeaseStatus;
        }
    }
}
