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
                    canEdit = true;
                    break;
                case AcquisitionStatusTypes.ARCHIV:
                case AcquisitionStatusTypes.CANCEL:
                case AcquisitionStatusTypes.CLOSED:
                case AcquisitionStatusTypes.COMPLT:
                case AcquisitionStatusTypes.HOLD:
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

            bool canEdit;
            switch (acquisitionStatus)
            {
                case AcquisitionStatusTypes.ACTIVE:
                case AcquisitionStatusTypes.DRAFT:
                    canEdit = true;
                    break;
                case AcquisitionStatusTypes.ARCHIV:
                case AcquisitionStatusTypes.CANCEL:
                case AcquisitionStatusTypes.CLOSED:
                case AcquisitionStatusTypes.COMPLT:
                case AcquisitionStatusTypes.HOLD:
                    canEdit = false;
                    break;
                default:
                    canEdit = false;
                    break;
            }

            return canEdit;
        }

        public bool CanEditOrDeleteCompensation(AcquisitionStatusTypes? acquisitionStatus, bool? isDraftCompensation)
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
                    canEdit = true;
                    break;
                case AcquisitionStatusTypes.ARCHIV:
                case AcquisitionStatusTypes.CANCEL:
                case AcquisitionStatusTypes.CLOSED:
                case AcquisitionStatusTypes.COMPLT:
                case AcquisitionStatusTypes.HOLD:
                    canEdit = isDraftCompensation ?? true;
                    break;
                default:
                    canEdit = false;
                    break;
            }

            return canEdit;
        }

        public bool CanEditOrDeleteAgreement(AcquisitionStatusTypes? acquisitionStatus, AgreementStatusTypes? agreementStatus)
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
                    canEdit = true;
                    break;
                case AcquisitionStatusTypes.ARCHIV:
                case AcquisitionStatusTypes.CANCEL:
                case AcquisitionStatusTypes.CLOSED:
                case AcquisitionStatusTypes.COMPLT:
                case AcquisitionStatusTypes.HOLD:
                    canEdit = agreementStatus != AgreementStatusTypes.FINAL;
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
                default:
                    canEdit = true;
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
                default:
                    canEdit = true;
                    break;
            }

            return canEdit;
        }
    }
}
