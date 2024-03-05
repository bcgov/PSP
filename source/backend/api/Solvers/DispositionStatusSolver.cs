using Pims.Api.Constants;

namespace Pims.Api.Services
{
    public class DispositionStatusSolver : IDispositionStatusSolver
    {

        public bool CanEditDetails(DispositionStatusTypes? dispositionStatus)
        {
            if (dispositionStatus == null)
            {
                return false;
            }

            bool canEdit;
            switch (dispositionStatus)
            {
                case DispositionStatusTypes.ACTIVE:
                case DispositionStatusTypes.DRAFT:
                    canEdit = true;
                    break;
                case DispositionStatusTypes.ARCHIVED:
                case DispositionStatusTypes.CANCELLED:
                case DispositionStatusTypes.CLOSED:
                case DispositionStatusTypes.COMPLETE:
                case DispositionStatusTypes.HOLD:
                    canEdit = false;
                    break;
                default:
                    canEdit = false;
                    break;
            }

            return canEdit;
        }

        public bool CanEditProperties(DispositionStatusTypes? dispositionStatus)
        {
            if (dispositionStatus == null)
            {
                return false;
            }

            bool canEdit;
            switch (dispositionStatus)
            {
                case DispositionStatusTypes.ACTIVE:
                case DispositionStatusTypes.DRAFT:
                    canEdit = true;
                    break;
                case DispositionStatusTypes.ARCHIVED:
                case DispositionStatusTypes.CANCELLED:
                case DispositionStatusTypes.CLOSED:
                case DispositionStatusTypes.COMPLETE:
                case DispositionStatusTypes.HOLD:
                    canEdit = false;
                    break;
                default:
                    canEdit = false;
                    break;
            }

            return canEdit;
        }

        public bool CanEditOrDeleteValuesOffersSales(DispositionStatusTypes? dispositionStatus)
        {
            if (dispositionStatus == null)
            {
                return false;
            }

            bool canEdit;
            switch (dispositionStatus)
            {
                case DispositionStatusTypes.ACTIVE:
                case DispositionStatusTypes.DRAFT:
                    canEdit = true;
                    break;
                case DispositionStatusTypes.ARCHIVED:
                case DispositionStatusTypes.CANCELLED:
                case DispositionStatusTypes.CLOSED:
                case DispositionStatusTypes.COMPLETE:
                case DispositionStatusTypes.HOLD:
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
