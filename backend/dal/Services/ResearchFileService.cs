using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
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
        private readonly IPropertyRepository _propertyRepository;

        public ResearchFileService(ClaimsPrincipal user, ILogger<ResearchFileService> logger, IResearchFileRepository researchFileRepository, IPropertyRepository propertyRepository)
        {
            _user = user;
            _logger = logger;
            _researchFileRepository = researchFileRepository;
            _propertyRepository = propertyRepository;
        }

        public PimsResearchFile GetById(long id)
        {
            _logger.LogInformation("Getting research file with id {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.ResearchFileView);
            return _researchFileRepository.GetById(id);
        }

        public PimsResearchFile Add(PimsResearchFile researchFile)
        {
            _logger.LogInformation("Adding research file...");
            _user.ThrowIfNotAuthorized(Permissions.ResearchFileAdd);
            researchFile.ResearchFileStatusTypeCode = "ACTIVE";

            foreach (var researchProperty in researchFile.PimsPropertyResearchFiles)
            {
                var pid = researchProperty.Property.Pid.Value;
                try
                {
                    var foundProperty = _propertyRepository.GetByPid(pid);
                    researchProperty.Property = foundProperty;
                }
                catch (KeyNotFoundException e)
                {
                    _logger.LogDebug("Adding new property with pid:{prop}", pid);
                    researchProperty.Property.PropertyClassificationTypeCode = "CORESTRAT"; // Todo: should be "UNKNOWN"
                    researchProperty.Property.PropertyDataSourceEffectiveDate = System.DateTime.Now;
                    researchProperty.Property.PropertyDataSourceTypeCode = "PAIMS"; // Todo: should be "PMBC"

                    researchProperty.Property.PropertyTypeCode = "UNKNOWN";

                    researchProperty.Property.PropertyStatusTypeCode = "UNSURVYED"; // Todo: should be 'UNKNOWN';
                    researchProperty.Property.SurplusDeclarationTypeCode = "UNKNOWN";

                    researchProperty.Property.RegionCode = 1; // TODO: this reallly needs to come from the app
                    researchProperty.Property.DistrictCode = 1; // TODO: this reallly needs to come from the app
                }
            }

            var newResearchFile = _researchFileRepository.Add(researchFile);
            _researchFileRepository.CommitTransaction();
            return newResearchFile;
        }

        public PimsResearchFile Update(PimsResearchFile researchFile)
        {
            _logger.LogInformation("Updating research file...");
            _user.ThrowIfNotAuthorized(Permissions.ResearchFileEdit);

            var newResearchFile = _researchFileRepository.Update(researchFile);
            _researchFileRepository.CommitTransaction();
            return newResearchFile;
        }

        public Paged<PimsResearchFile> GetPage(ResearchFilter filter)
        {
            _logger.LogInformation("Searching for research files...");

            _logger.LogDebug("Research file search with filter", filter);
            _user.ThrowIfNotAuthorized(Permissions.ResearchFileView);
            return _researchFileRepository.GetPage(filter);
        }
    }
}
