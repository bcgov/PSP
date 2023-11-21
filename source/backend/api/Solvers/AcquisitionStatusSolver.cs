using Pims.Api.Constants;

namespace Pims.Api.Services
{
    public class AcquisitionStatusSolver : IAcquisitionStatusSolver
    {

        public bool CanEditDetails(AcqusitionStatusTypes? acquisitionStatus)
        {
            if (acquisitionStatus == null)
            {
                return false;
            }

            bool canEdit;
            switch (acquisitionStatus)
            {
                case AcqusitionStatusTypes.ACTIVE:
                case AcqusitionStatusTypes.DRAFT:
                    canEdit = true;
                    break;
                case AcqusitionStatusTypes.ARCHIV:
                case AcqusitionStatusTypes.CANCEL:
                case AcqusitionStatusTypes.CLOSED:
                case AcqusitionStatusTypes.COMPLT:
                case AcqusitionStatusTypes.HOLD:
                    canEdit = false;
                    break;
                default:
                    canEdit = false;
                    break;
            }

            return canEdit;
        }

        public bool CanEditTakes(AcqusitionStatusTypes? acquisitionStatus)
        {
            if (acquisitionStatus == null)
            {
                return false;
            }

            bool canEdit;
            switch (acquisitionStatus)
            {
                case AcqusitionStatusTypes.ACTIVE:
                case AcqusitionStatusTypes.DRAFT:
                    canEdit = true;
                    break;
                case AcqusitionStatusTypes.ARCHIV:
                case AcqusitionStatusTypes.CANCEL:
                case AcqusitionStatusTypes.CLOSED:
                case AcqusitionStatusTypes.COMPLT:
                case AcqusitionStatusTypes.HOLD:
                    canEdit = false;
                    break;
                default:
                    canEdit = false;
                    break;
            }

            return canEdit;
        }

        public bool CanEditOrDeleteCompensation(AcqusitionStatusTypes? acquisitionStatus, bool? isDraftCompensation)
        {
            if (acquisitionStatus == null)
            {
                return false;
            }

            bool canEdit;
            switch (acquisitionStatus)
            {
                case AcqusitionStatusTypes.ACTIVE:
                case AcqusitionStatusTypes.DRAFT:
                    canEdit = true;
                    break;
                case AcqusitionStatusTypes.ARCHIV:
                case AcqusitionStatusTypes.CANCEL:
                case AcqusitionStatusTypes.CLOSED:
                case AcqusitionStatusTypes.COMPLT:
                case AcqusitionStatusTypes.HOLD:
                    canEdit = isDraftCompensation ?? true;
                    break;
                default:
                    canEdit = false;
                    break;
            }

            return canEdit;
        }

        public bool CanEditOrDeleteAgreement(AcqusitionStatusTypes? acquisitionStatus, AgreementStatusTypes? agreementStatus)
        {
            if (acquisitionStatus == null)
            {
                return false;
            }

            bool canEdit;
            switch (acquisitionStatus)
            {
                case AcqusitionStatusTypes.ACTIVE:
                case AcqusitionStatusTypes.DRAFT:
                    canEdit = true;
                    break;
                case AcqusitionStatusTypes.ARCHIV:
                case AcqusitionStatusTypes.CANCEL:
                case AcqusitionStatusTypes.CLOSED:
                case AcqusitionStatusTypes.COMPLT:
                case AcqusitionStatusTypes.HOLD:
                    canEdit = agreementStatus != AgreementStatusTypes.FINAL;
                    break;
                default:
                    canEdit = false;
                    break;
            }

            return canEdit;
        }

        public bool CanEditChecklists(AcqusitionStatusTypes? acquisitionStatus)
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

        public bool CanEditStakeholders(AcqusitionStatusTypes? acquisitionStatus)
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
