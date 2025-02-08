using System;
using Pims.Api.Models.CodeTypes;

namespace Pims.Api.Services
{
    public class ResearchStatusSolver : IResearchStatusSolver
    {
        public bool CanEditDetails(ResearchFileStatusTypes? researchStatus)
        {
            if (researchStatus == null)
            {
                return false;
            }

            bool canEdit;
            switch (researchStatus)
            {
                case ResearchFileStatusTypes.ACTIVE:
                    canEdit = true;
                    break;
                case ResearchFileStatusTypes.ARCHIVED:
                case ResearchFileStatusTypes.CLOSED:
                case ResearchFileStatusTypes.INACTIVE:
                    canEdit = false;
                    break;
                default:
                    canEdit = false;
                    break;
            }

            return canEdit;
        }

        public bool CanEditProperties(ResearchFileStatusTypes? researchStatus)
        {
            if (researchStatus == null)
            {
                return false;
            }

            bool canEdit;
            switch (researchStatus)
            {
                case ResearchFileStatusTypes.ACTIVE:
                    canEdit = true;
                    break;
                case ResearchFileStatusTypes.ARCHIVED:
                case ResearchFileStatusTypes.CLOSED:
                case ResearchFileStatusTypes.INACTIVE:
                    canEdit = false;
                    break;
                default:
                    canEdit = false;
                    break;
            }

            return canEdit;
        }

        public ResearchFileStatusTypes? GetCurrentResearchStatus(string pimsResearchStatusType)
        {
            ResearchFileStatusTypes currentResearchStatus;
            if (Enum.TryParse(pimsResearchStatusType, out currentResearchStatus))
            {
                return currentResearchStatus;
            }

            return currentResearchStatus;
        }
    }
}
