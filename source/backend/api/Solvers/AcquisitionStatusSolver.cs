using Pims.Api.Models.CodeTypes;

namespace Pims.Api.Services
{
    public class AcquisitionStatusSolver : IAcquisitionStatusSolver
    {

        public bool CanEditDetails(AcquisitionStatusTypes? acquisitionStatus)
        {
            if (acquisitionStatus == null)
            {
                return false;
            }

            bool canEdit;
            switch (acquisitionStatus)
            {
                case AcquisitionStatusTypes.ACTIVE:
                case AcquisitionStatusTypes.DRAFT:
                case AcquisitionStatusTypes.HOLD:
                    canEdit = true;
                    break;
                case AcquisitionStatusTypes.ARCHIV:
                case AcquisitionStatusTypes.CANCEL:
                case AcquisitionStatusTypes.CLOSED:
                case AcquisitionStatusTypes.COMPLT:
                    canEdit = false;
                    break;
                default:
                    canEdit = false;
                    break;
            }

            return canEdit;
        }

        public bool CanEditProperties(AcquisitionStatusTypes? acquisitionStatus)
        {
            if (acquisitionStatus == null)
            {
                return false;
            }

            bool canEdit;
            switch (acquisitionStatus)
            {
                case AcquisitionStatusTypes.ACTIVE:
                case AcquisitionStatusTypes.DRAFT:
                case AcquisitionStatusTypes.HOLD:
                    canEdit = true;
                    break;
                case AcquisitionStatusTypes.ARCHIV:
                case AcquisitionStatusTypes.CANCEL:
                case AcquisitionStatusTypes.CLOSED:
                case AcquisitionStatusTypes.COMPLT:
                    canEdit = false;
                    break;
                default:
                    canEdit = false;
                    break;
            }

            return canEdit;
        }

        public bool CanEditTakes(AcquisitionStatusTypes? acquisitionStatus)
        {
            if (acquisitionStatus == null)
            {
                return false;
            }

            var canEdit = acquisitionStatus switch
            {
                AcquisitionStatusTypes.ACTIVE or AcquisitionStatusTypes.DRAFT => true,
                AcquisitionStatusTypes.ARCHIV or AcquisitionStatusTypes.CANCEL or AcquisitionStatusTypes.CLOSED or AcquisitionStatusTypes.COMPLT or AcquisitionStatusTypes.HOLD => false,
                _ => false,
            };
            return canEdit;
        }

        public bool CanEditOrDeleteCompensation(AcquisitionStatusTypes? acquisitionStatus, bool? isDraftCompensation, bool? isAdmin)
        {
            if (acquisitionStatus == null)
            {
                return false;
            }

            bool canEdit;
            switch (acquisitionStatus)
            {
                case AcquisitionStatusTypes.ACTIVE:
                case AcquisitionStatusTypes.DRAFT:
                case AcquisitionStatusTypes.HOLD:
                    canEdit = (isDraftCompensation.HasValue && isDraftCompensation.Value) || (isAdmin.HasValue && isAdmin.Value);
                    break;
                case AcquisitionStatusTypes.ARCHIV:
                case AcquisitionStatusTypes.CANCEL:
                case AcquisitionStatusTypes.CLOSED:
                case AcquisitionStatusTypes.COMPLT:
                    canEdit = false;
                    break;
                default:
                    canEdit = false;
                    break;
            }

            return canEdit;
        }

        public bool CanEditOrDeleteAgreement(AcquisitionStatusTypes? acquisitionStatus)
        {
            bool canEdit;
            switch (acquisitionStatus)
            {
                case AcquisitionStatusTypes.ACTIVE:
                case AcquisitionStatusTypes.DRAFT:
                case AcquisitionStatusTypes.HOLD:
                    canEdit = true;
                    break;
                case AcquisitionStatusTypes.ARCHIV:
                case AcquisitionStatusTypes.CANCEL:
                case AcquisitionStatusTypes.CLOSED:
                case AcquisitionStatusTypes.COMPLT:
                    canEdit = false;
                    break;
                default:
                    canEdit = false;
                    break;
            }

            return canEdit;
        }

        public bool CanEditChecklists(AcquisitionStatusTypes? acquisitionStatus)
        {
            if (acquisitionStatus == null)
            {
                return false;
            }

            bool canEdit;
            switch (acquisitionStatus)
            {
                case AcquisitionStatusTypes.ACTIVE:
                case AcquisitionStatusTypes.DRAFT:
                case AcquisitionStatusTypes.HOLD:
                    canEdit = true;
                    break;
                case AcquisitionStatusTypes.ARCHIV:
                case AcquisitionStatusTypes.CANCEL:
                case AcquisitionStatusTypes.CLOSED:
                case AcquisitionStatusTypes.COMPLT:
                    canEdit = false;
                    break;
                default:
                    canEdit = false;
                    break;
            }

            return canEdit;
        }

        public bool CanEditStakeholders(AcquisitionStatusTypes? acquisitionStatus)
        {
            if (acquisitionStatus == null)
            {
                return false;
            }

            bool canEdit;
            switch (acquisitionStatus)
            {
                case AcquisitionStatusTypes.ACTIVE:
                case AcquisitionStatusTypes.DRAFT:
                case AcquisitionStatusTypes.HOLD:
                    canEdit = true;
                    break;
                case AcquisitionStatusTypes.ARCHIV:
                case AcquisitionStatusTypes.CANCEL:
                case AcquisitionStatusTypes.CLOSED:
                case AcquisitionStatusTypes.COMPLT:
                    canEdit = false;
                    break;
                default:
                    canEdit = false;
                    break;
            }

            return canEdit;
        }

        public bool CanEditExpropriation(AcquisitionStatusTypes? acquisitionStatus)
        {
            if (acquisitionStatus == null)
            {
                return false;
            }

            bool canEdit;
            switch (acquisitionStatus)
            {
                case AcquisitionStatusTypes.ACTIVE:
                case AcquisitionStatusTypes.DRAFT:
                case AcquisitionStatusTypes.HOLD:
                    canEdit = true;
                    break;
                case AcquisitionStatusTypes.ARCHIV:
                case AcquisitionStatusTypes.CANCEL:
                case AcquisitionStatusTypes.CLOSED:
                case AcquisitionStatusTypes.COMPLT:
                    canEdit = false;
                    break;
                default:
                    canEdit = false;
                    break;
            }

            return canEdit;
        }
    }
}
