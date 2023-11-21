using System;
using Pims.Api.Constants;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public class AcquisitionStatusSolver : IAcquisitionStatusSolver
    {
        private readonly AcqusitionStatusTypes? statusCode;

        public AcquisitionStatusSolver(PimsAcquisitionFile aquisitionFile)
        {
            if (aquisitionFile != null && !string.IsNullOrEmpty(aquisitionFile.AcquisitionFileStatusTypeCode))
            {
                Enum.Parse<AcqusitionStatusTypes>(aquisitionFile.AcquisitionFileStatusTypeCode);
            }
            else
            {
                statusCode = null;
            }
        }

        public bool CanEditDetails()
        {
            if (statusCode == null)
            {
                return false;
            }

            bool canEdit;
            switch (statusCode)
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

        public bool CanEditTakes()
        {
            if (statusCode == null)
            {
                return false;
            }

            bool canEdit;
            switch (statusCode)
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

        public bool CanEditOrDeleteCompensation(bool? isDraftCompensation)
        {
            if (statusCode == null)
            {
                return false;
            }

            bool canEdit;
            switch (statusCode)
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

        public bool CanEditOrDeleteAgreement(string agreementStatusCode)
        {
            if (statusCode == null)
            {
                return false;
            }

            var agreementStatusCodeEnum = Enum.Parse<AgreementStatusTypes>(agreementStatusCode);

            bool canEdit;
            switch (statusCode)
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
                    canEdit = agreementStatusCodeEnum != AgreementStatusTypes.FINAL;
                    break;
                default:
                    canEdit = false;
                    break;
            }

            return canEdit;
        }

        public bool CanEditChecklists()
        {
            if (statusCode == null)
            {
                return false;
            }

            bool canEdit;
            switch (statusCode)
            {
                default:
                    canEdit = true;
                    break;
            }

            return canEdit;
        }

        public bool CanEditStakeholders()
        {
            if (statusCode == null)
            {
                return false;
            }

            bool canEdit;
            switch (statusCode)
            {
                default:
                    canEdit = true;
                    break;
            }

            return canEdit;
        }
    }
}
