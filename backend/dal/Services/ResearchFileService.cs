using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Pims.Dal.Helpers;
using Pims.Dal.Constants;

namespace Pims.Dal.Services
{
    public class ResearchFileService : IResearchFileService
    {
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly IResearchFileRepository _researchFileRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly ICoordinateTransformService _coordinateService;


        public ResearchFileService(ClaimsPrincipal user, ILogger<ResearchFileService> logger, IResearchFileRepository researchFileRepository, IPropertyRepository propertyRepository, ICoordinateTransformService coordinateService)
        {
            _user = user;
            _logger = logger;
            _researchFileRepository = researchFileRepository;
            _propertyRepository = propertyRepository;
            _coordinateService = coordinateService;
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
                if (researchProperty.Property.Pid.HasValue)
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
                        PopulateResearchFile(researchProperty.Property);
                    }
                }
                else
                {
                    _logger.LogDebug("Adding new property without a pid");
                    PopulateResearchFile(researchProperty.Property);
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

        public PimsResearchFile UpdateProperty(long researchFileId, long researchFilePropertyId, long researchFileVersion, PimsPropertyResearchFile propertyResearchFile)
        {
            _logger.LogInformation("Updating property research file...");
            _user.ThrowIfNotAuthorized(Permissions.ResearchFileEdit);
            ValidateVersion(researchFileId, researchFileVersion);
            return _researchFileRepository.UpdateProperty(researchFileId, propertyResearchFile);
        }

        private void PopulateResearchFile(PimsProperty property)
        {
            property.PropertyClassificationTypeCode = "UNKNOWN";
            property.PropertyDataSourceEffectiveDate = System.DateTime.Now;
            property.PropertyDataSourceTypeCode = "PMBC";

            property.PropertyTypeCode = "UNKNOWN";

            property.PropertyStatusTypeCode = "UNKNOWN";
            property.SurplusDeclarationTypeCode = "UNKNOWN";

            // convert spatial location from lat/long (4326) to BC Albers (3005) for database storage
            var geom = property.Location;
            if (geom.SRID != SpatialReference.BC_ALBERS)
            {
                var newCoords = _coordinateService.TransformCoordinates(geom.SRID, SpatialReference.BC_ALBERS, geom.Coordinate);
                property.Location = GeometryHelper.CreatePoint(newCoords, SpatialReference.BC_ALBERS);
            }
        }

        private void ValidateVersion(long researchFileId, long researchFileVersion)
        {
            long currentRowVersion = _researchFileRepository.GetRowVersion(researchFileId);
            if (currentRowVersion != researchFileVersion)
            {
                throw new DbUpdateConcurrencyException("You are working with an older version of this research file, please refresh the application and retry.");
            }
        }
    }
}
