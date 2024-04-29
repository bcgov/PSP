using Pims.Api.Models.CodeTypes;

namespace Pims.Api.Services
{
    public class DispositionStatusSolver : IDispositionStatusSolver
    {
        public bool CanEditDetails(DispositionFileStatusTypes? dispositionStatus)
        {
            if (dispositionStatus == null)
            {
                return false;
            }

            bool canEdit;
            switch (dispositionStatus)
            {
                case DispositionFileStatusTypes.ACTIVE:
                case DispositionFileStatusTypes.DRAFT:
                case DispositionFileStatusTypes.HOLD:
                    canEdit = true;
                    break;
                case DispositionFileStatusTypes.ARCHIVED:
                case DispositionFileStatusTypes.CANCELLED:
                case DispositionFileStatusTypes.COMPLETE:
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
