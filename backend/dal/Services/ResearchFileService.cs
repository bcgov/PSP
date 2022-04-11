using System.Collections.Generic;
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
        private readonly IPropertyRepository _propertyRepository;

        public ResearchFileService(ClaimsPrincipal user, ILogger<ResearchFileService> logger, IResearchFileRepository researchFileRepository, IPropertyRepository propertyRepository)
        {
            _user = user;
            _logger = logger;
            _researchFileRepository = researchFileRepository;
            _propertyRepository = propertyRepository;
        }

        public PimsResearchFile Add(PimsResearchFile researchFile)
        {
            _logger.LogInformation("Adding research file...");
            _user.ThrowIfNotAuthorized(Permissions.ResearchFileAdd);
            researchFile.ResearchFileStatusTypeCode = "ACTIVE";

            foreach (var researchProperty in researchFile.PimsPropertyResearchFiles)
            {
                //var currentProperty = researchProperty.Property;
                var pid = researchProperty.Property.Pid.Value;
                try
                {
                    var foundProperty = _propertyRepository.GetByPid(pid);
                    researchProperty.Property = foundProperty;
                }
                catch (KeyNotFoundException e)
                {
                    _logger.LogDebug("Adding new property with pid:{prop}", pid);
                    researchProperty.Property.PropertyTypeCode = "UNKNOWN";
                    researchProperty.Property.PropertyClassificationTypeCode = "CORESTRAT";
                    researchProperty.Property.PropertyDataSourceTypeCode = "PAIMS";
                    researchProperty.Property.PropertyStatusTypeCode = "UNSURVYED";
                    researchProperty.Property.SurplusDeclarationTypeCode = "UNKNOWN";
                    researchProperty.Property.RegionCode = 1; // TODO: this reallly needs to come from the app
                    researchProperty.Property.DistrictCode = 1; // TODO: this reallly needs to come from the app
                    researchProperty.Property.AddressId = 1; // TODO: this would be nullable
                }
            }
            //_propertyRepository.GetForPID()
            var newResearchFile = _researchFileRepository.Add(researchFile);
            _researchFileRepository.CommitTransaction();
            return newResearchFile;
            //return researchFile;
        }
    }
}
