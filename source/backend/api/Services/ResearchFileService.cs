using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Constants;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Api.Services
{
    public class ResearchFileService : IResearchFileService
    {
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly IResearchFileRepository _researchFileRepository;
        private readonly IResearchFilePropertyRepository _researchFilePropertyRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly ICoordinateTransformService _coordinateService;

        public ResearchFileService(
            ClaimsPrincipal user,
            ILogger<ResearchFileService> logger,
            IResearchFileRepository researchFileRepository,
            IResearchFilePropertyRepository researchFilePropertyRepository,
            IPropertyRepository propertyRepository,
            ICoordinateTransformService coordinateService)
        {
            _user = user;
            _logger = logger;
            _researchFileRepository = researchFileRepository;
            _researchFilePropertyRepository = researchFilePropertyRepository;
            _propertyRepository = propertyRepository;
            _coordinateService = coordinateService;
        }

        public PimsResearchFile GetById(long id)
        {
            _logger.LogInformation("Getting research file with id {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.ResearchFileView);

            var researchFile = _researchFileRepository.GetById(id);
            ReprojectPropertyLocationsToWgs84(researchFile);

            return researchFile;
        }

        public PimsResearchFile Add(PimsResearchFile researchFile)
        {
            _logger.LogInformation("Adding research file...");
            _user.ThrowIfNotAuthorized(Permissions.ResearchFileAdd);
            researchFile.ResearchFileStatusTypeCode = "ACTIVE";

            MatchProperties(researchFile);

            var newResearchFile = _researchFileRepository.Add(researchFile);
            _researchFileRepository.CommitTransaction();
            return newResearchFile;
        }

        public PimsResearchFile Update(PimsResearchFile researchFile)
        {
            _logger.LogInformation("Updating research file...");
            _user.ThrowIfNotAuthorized(Permissions.ResearchFileEdit);
            ValidateVersion(researchFile.Id, researchFile.ConcurrencyControlNumber);

            var newResearchFile = _researchFileRepository.Update(researchFile);
            _researchFileRepository.CommitTransaction();
            return newResearchFile;
        }

        public PimsResearchFile UpdateProperties(PimsResearchFile researchFile)
        {
            _logger.LogInformation("Updating research file properties...");
            _user.ThrowIfNotAuthorized(Permissions.ResearchFileEdit);
            ValidateVersion(researchFile.Id, researchFile.ConcurrencyControlNumber);

            MatchProperties(researchFile);

            // Get the current properties in the research file
            var currentProperties = _researchFilePropertyRepository.GetByResearchFileId(researchFile.Id);

            // Check if the property is new or if it is being updated
            foreach (var incommingResearchProperty in researchFile.PimsPropertyResearchFiles)
            {
                // If the property is not new, check if the name has been updated.
                if (incommingResearchProperty.Id != 0)
                {
                    PimsPropertyResearchFile existingProperty = currentProperties.FirstOrDefault(x => x.Id == incommingResearchProperty.Id);
                    if (existingProperty.PropertyName != incommingResearchProperty.PropertyName)
                    {
                        existingProperty.PropertyName = incommingResearchProperty.PropertyName;
                        _researchFilePropertyRepository.Update(existingProperty);
                    }
                }
                else
                {
                    // New property needs to be added
                    _researchFilePropertyRepository.Add(incommingResearchProperty);
                }
            }

            // The ones not on the new set should be deleted
            List<PimsPropertyResearchFile> differenceSet = currentProperties.Where(x => !researchFile.PimsPropertyResearchFiles.Any(y => y.Id == x.Id)).ToList();
            foreach (var deletedProperty in differenceSet)
            {
                _researchFilePropertyRepository.Delete(deletedProperty);
                if (deletedProperty.Property.IsPropertyOfInterest.HasValue && deletedProperty.Property.IsPropertyOfInterest.Value)
                {
                    PimsProperty propertyWithAssociations = _propertyRepository.GetAssociations(deletedProperty.PropertyId);
                    var leaseAssociationCount = propertyWithAssociations.PimsPropertyLeases.Count;
                    var researchAssociationCount = propertyWithAssociations.PimsPropertyResearchFiles.Count;
                    var acquisitionAssociationCount = propertyWithAssociations.PimsPropertyAcquisitionFiles.Count;
                    if (leaseAssociationCount + acquisitionAssociationCount == 0 && researchAssociationCount == 1 && deletedProperty?.Property?.IsPropertyOfInterest == true)
                    {
                        _researchFilePropertyRepository.CommitTransaction(); // TODO: this can only be removed if cascade deletes are implemented. EF executes deletes in alphabetic order.
                        _propertyRepository.Delete(deletedProperty.Property);
                    }
                }
            }

            _researchFilePropertyRepository.CommitTransaction();
            return _researchFileRepository.GetById(researchFile.Id);
        }

        public Paged<PimsResearchFile> GetPage(ResearchFilter filter)
        {
            _logger.LogInformation("Searching for research files...");

            _logger.LogDebug("Research file search with filter", filter);
            _user.ThrowIfNotAuthorized(Permissions.ResearchFileView);

            return _researchFileRepository.GetPage(filter);
        }

        public PimsResearchFile UpdateProperty(long researchFileId, long researchFileVersion, PimsPropertyResearchFile propertyResearchFile)
        {
            _logger.LogInformation("Updating property research file...");
            _user.ThrowIfNotAuthorized(Permissions.ResearchFileEdit);
            ValidateVersion(researchFileId, researchFileVersion);

            _researchFilePropertyRepository.Update(propertyResearchFile);
            _researchFilePropertyRepository.CommitTransaction();
            return _researchFileRepository.GetById(researchFileId);
        }

        private void MatchProperties(PimsResearchFile researchFile)
        {
            foreach (var researchProperty in researchFile.PimsPropertyResearchFiles)
            {
                if (researchProperty.Property.Pid.HasValue)
                {
                    var pid = researchProperty.Property.Pid.Value;
                    try
                    {
                        var foundProperty = _propertyRepository.GetByPid(pid);
                        researchProperty.PropertyId = foundProperty.Id;
                        researchProperty.Property = foundProperty;
                    }
                    catch (KeyNotFoundException)
                    {
                        _logger.LogDebug("Adding new property with pid:{pid}", pid);
                        PopulateNewProperty(researchProperty.Property);
                    }
                }
                else if (researchProperty.Property.Pin.HasValue)
                {
                    var pin = researchProperty.Property.Pin.Value;
                    try
                    {
                        var foundProperty = _propertyRepository.GetByPin(pin);
                        researchProperty.PropertyId = foundProperty.Id;
                        researchProperty.Property = foundProperty;
                    }
                    catch (KeyNotFoundException)
                    {
                        _logger.LogDebug("Adding new property with pin:{pin}", pin);
                        PopulateNewProperty(researchProperty.Property);
                    }
                }
                else
                {
                    _logger.LogDebug("Adding new property without a pid");
                    PopulateNewProperty(researchProperty.Property);
                }
            }
        }

        private void PopulateNewProperty(PimsProperty property)
        {
            property.PropertyClassificationTypeCode = "UNKNOWN";
            property.PropertyDataSourceEffectiveDate = System.DateTime.Now;
            property.PropertyDataSourceTypeCode = "PMBC";

            property.PropertyTypeCode = "UNKNOWN";

            property.PropertyStatusTypeCode = "UNKNOWN";
            property.SurplusDeclarationTypeCode = "UNKNOWN";

            property.IsPropertyOfInterest = true;

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

        private void ReprojectPropertyLocationsToWgs84(PimsResearchFile researchFile)
        {
            if (researchFile == null)
            {
                return;
            }

            foreach (var researchProperty in researchFile.PimsPropertyResearchFiles)
            {
                if (researchProperty.Property.Location != null)
                {
                    var oldCoords = researchProperty.Property.Location.Coordinate;
                    var newCoords = _coordinateService.TransformCoordinates(SpatialReference.BC_ALBERS, SpatialReference.WGS_84, oldCoords);
                    researchProperty.Property.Location = GeometryHelper.CreatePoint(newCoords, SpatialReference.WGS_84);
                }
            }
        }
    }
}
