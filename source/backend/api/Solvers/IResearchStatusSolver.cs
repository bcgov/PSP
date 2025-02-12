using Pims.Api.Models.CodeTypes;

namespace Pims.Api.Services
{
    public interface IResearchStatusSolver
    {
        bool CanEditDetails(ResearchFileStatusTypes? researchStatus);

        bool CanEditProperties(ResearchFileStatusTypes? researchStatus);

        ResearchFileStatusTypes? GetCurrentResearchStatus(string pimsResearchStatusType);
    }
}
