using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Dal.Services
{
    public class ResearchFileService : IResearchFileService
    {
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly IResearchFileRepository _researchFileRepository;

        public ResearchFileService(ClaimsPrincipal user, ILogger<ResearchFileService> logger, IResearchFileRepository researchFileRepository)
        {
            _user = user;
            _logger = logger;
            _researchFileRepository = researchFileRepository;
        }

        public PimsResearchFile Add(PimsResearchFile researchFile)
        {
            _logger.LogInformation("Adding research file...");
            _user.ThrowIfNotAuthorized(Permissions.ResearchFileAdd);
            researchFile.ResearchFileStatusTypeCode = "ACTIVE";
            var newResearchFile = _researchFileRepository.Add(researchFile);
            _researchFileRepository.CommitTransaction();
            return newResearchFile;
        }
    }
}
