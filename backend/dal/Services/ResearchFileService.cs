using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using static Pims.Dal.Entities.PimsLeaseStatusType;

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
            _user.ThrowIfNotAuthorized(Permissions.ResearchFileAdd);
            return _researchFileRepository.Add(researchFile);
        }
    }
}
