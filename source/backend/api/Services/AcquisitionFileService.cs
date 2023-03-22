using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Api.Helpers.Exceptions;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Dal.Constants;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Helpers.Extensions;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Api.Services
{
    public class AcquisitionFileService : IAcquisitionFileService
    {
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly IAcquisitionFileRepository _acqFileRepository;
        private readonly IAcquisitionFilePropertyRepository _acquisitionFilePropertyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly ICoordinateTransformService _coordinateService;
        private readonly ILookupRepository _lookupRepository;
        private readonly IEntityNoteRepository _entityNoteRepository;
        private readonly IAcquisitionFileChecklistRepository _checklistRepository;

        public AcquisitionFileService(
            ClaimsPrincipal user,
            ILogger<AcquisitionFileService> logger,
            IAcquisitionFileRepository acqFileRepository,
            IAcquisitionFilePropertyRepository acqFilePropertyRepository,
            IUserRepository userRepository,
            IPropertyRepository propertyRepository,
            ICoordinateTransformService coordinateService,
            ILookupRepository lookupRepository,
            IEntityNoteRepository entityNoteRepository,
            IAcquisitionFileChecklistRepository checklistRepository)
        {
            _user = user;
            _logger = logger;
            _acqFileRepository = acqFileRepository;
            _acquisitionFilePropertyRepository = acqFilePropertyRepository;
            _userRepository = userRepository;
            _propertyRepository = propertyRepository;
            _coordinateService = coordinateService;
            _lookupRepository = lookupRepository;
            _entityNoteRepository = entityNoteRepository;
            _checklistRepository = checklistRepository;
        }

        public Paged<PimsAcquisitionFile> GetPage(AcquisitionFilter filter)
        {
            _logger.LogInformation("Searching for acquisition files...");
            _logger.LogDebug("Acquisition file search with filter", filter);

            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileView);

            // Limit search results to user's assigned region(s)
            var pimsUser = _userRepository.GetUserInfoByKeycloakUserId(_user.GetUserKey());
            var userRegions = pimsUser.PimsRegionUsers.Select(r => r.RegionCode).ToHashSet();

            return _acqFileRepository.GetPage(filter, userRegions);
        }

        public PimsAcquisitionFile GetById(long id)
        {
            _logger.LogInformation("Getting acquisition file with id {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileView);

            var acqFile = _acqFileRepository.GetById(id);
            return acqFile;
        }

        public IEnumerable<PimsPropertyAcquisitionFile> GetProperties(long id)
        {
            _logger.LogInformation("Getting acquisition file with id {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileView);
            _user.ThrowIfNotAuthorized(Permissions.PropertyView);

            var properties = _acquisitionFilePropertyRepository.GetPropertiesByAcquisitionFileId(id);
            ReprojectPropertyLocationsToWgs84(properties);
            return properties;
        }

        public IEnumerable<PimsAcquisitionOwner> GetOwners(long id)
        {
            _logger.LogInformation("Getting acquisition file owners with AcquisitionFile id: {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileView);

            return _acquisitionFilePropertyRepository.GetOwnersByAcquisitionFileId(id);
        }

        public IEnumerable<PimsAcquisitionChecklistItem> GetChecklistItems(long id)
        {
            _logger.LogInformation("Getting acquisition file checklist with AcquisitionFile id: {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileView);

            return _checklistRepository.GetAllChecklistItemsByAcquisitionFileId(id);
        }

        public PimsAcquisitionFile Add(PimsAcquisitionFile acquisitionFile)
        {
            acquisitionFile.ThrowIfNull(nameof(acquisitionFile));

            _logger.LogInformation("Adding acquisition file with id {id}", acquisitionFile.Internal_Id);
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileAdd);

            // validate the new acq region
            var cannotDetermineRegion = _lookupRepository.GetAllRegions().FirstOrDefault(x => x.RegionName == "Cannot determine");
            if (acquisitionFile.RegionCode == cannotDetermineRegion.RegionCode)
            {
                throw new BadRequestException("Cannot set an acquisition file's region to 'cannot determine'");
            }

            ValidateStaff(acquisitionFile);

            acquisitionFile.AcquisitionFileStatusTypeCode = "ACTIVE";
            MatchProperties(acquisitionFile);
            PopulateAcquisitionChecklist(acquisitionFile);

            var newAcqFile = _acqFileRepository.Add(acquisitionFile);
            _acqFileRepository.CommitTransaction();
            return newAcqFile;
        }

        public PimsAcquisitionFile Update(PimsAcquisitionFile acquisitionFile, bool userOverride)
        {
            acquisitionFile.ThrowIfNull(nameof(acquisitionFile));

            _logger.LogInformation("Updating acquisition file with id {id}", acquisitionFile.Internal_Id);
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileEdit);

            ValidateVersion(acquisitionFile.Internal_Id, acquisitionFile.ConcurrencyControlNumber);

            if (!userOverride)
            {
                ValidateMinistryRegion(acquisitionFile.Internal_Id, acquisitionFile.RegionCode);
            }

            ValidateStaff(acquisitionFile);

            // reset the region
            var cannotDetermineRegion = _lookupRepository.GetAllRegions().FirstOrDefault(x => x.RegionName == "Cannot determine");
            if (acquisitionFile.RegionCode == cannotDetermineRegion.RegionCode)
            {
                throw new BadRequestException("Cannot set an acquisition file's region to 'cannot determine'");
            }

            var newAcqFile = _acqFileRepository.Update(acquisitionFile);
            AddNoteIfStatusChanged(acquisitionFile);

            _acqFileRepository.CommitTransaction();
            return newAcqFile;
        }

        public PimsAcquisitionFile UpdateProperties(PimsAcquisitionFile acquisitionFile)
        {
            _logger.LogInformation("Updating acquisition file properties...");
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileEdit, Permissions.PropertyView, Permissions.PropertyAdd);
            ValidateVersion(acquisitionFile.Internal_Id, acquisitionFile.ConcurrencyControlNumber);

            MatchProperties(acquisitionFile);

            // Get the current properties in the research file
            var currentProperties = _acquisitionFilePropertyRepository.GetPropertiesByAcquisitionFileId(acquisitionFile.Internal_Id);

            // Check if the property is new or if it is being updated
            foreach (var incomingAcquisitionProperty in acquisitionFile.PimsPropertyAcquisitionFiles)
            {
                // If the property is not new, check if the name has been updated.
                if (incomingAcquisitionProperty.Internal_Id != 0)
                {
                    PimsPropertyAcquisitionFile existingProperty = currentProperties.FirstOrDefault(x => x.Internal_Id == incomingAcquisitionProperty.Internal_Id);
                    if (existingProperty.PropertyName != incomingAcquisitionProperty.PropertyName)
                    {
                        existingProperty.PropertyName = incomingAcquisitionProperty.PropertyName;
                        _acquisitionFilePropertyRepository.Update(existingProperty);
                    }
                }
                else
                {
                    // New property needs to be added
                    _acquisitionFilePropertyRepository.Add(incomingAcquisitionProperty);
                }
            }

            // The ones not on the new set should be deleted
            List<PimsPropertyAcquisitionFile> differenceSet = currentProperties.Where(x => !acquisitionFile.PimsPropertyAcquisitionFiles.Any(y => y.Internal_Id == x.Internal_Id)).ToList();
            foreach (var deletedProperty in differenceSet)
            {
                var acqFileProperties = _acquisitionFilePropertyRepository.GetPropertiesByAcquisitionFileId(acquisitionFile.Internal_Id).FirstOrDefault(ap => ap.PropertyId == deletedProperty.PropertyId);
                if (acqFileProperties.PimsActInstPropAcqFiles.Any() || acqFileProperties.PimsTakes.Any())
                {
                    throw new BusinessRuleViolationException();
                }
                _acquisitionFilePropertyRepository.Delete(deletedProperty);
                if (deletedProperty.Property.IsPropertyOfInterest.HasValue && deletedProperty.Property.IsPropertyOfInterest.Value)
                {
                    PimsProperty propertyWithAssociations = _propertyRepository.GetAllAssociationsById(deletedProperty.PropertyId);
                    var leaseAssociationCount = propertyWithAssociations.PimsPropertyLeases.Count;
                    var researchAssociationCount = propertyWithAssociations.PimsPropertyResearchFiles.Count;
                    var acquisitionAssociationCount = propertyWithAssociations.PimsPropertyAcquisitionFiles.Count;
                    if (leaseAssociationCount + researchAssociationCount == 0 && acquisitionAssociationCount <= 1 && deletedProperty?.Property?.IsPropertyOfInterest == true)
                    {
                        _acqFileRepository.CommitTransaction(); // TODO: this can only be removed if cascade deletes are implemented. EF executes deletes in alphabetic order.
                        _propertyRepository.Delete(deletedProperty.Property);
                    }
                }
            }

            _acqFileRepository.CommitTransaction();
            return _acqFileRepository.GetById(acquisitionFile.Internal_Id);
        }

        public PimsAcquisitionFile UpdateChecklistItems(PimsAcquisitionFile acquisitionFile)
        {
            acquisitionFile.ThrowIfNull(nameof(acquisitionFile));
            _logger.LogInformation("Updating acquisition file checklist with AcquisitionFile id: {id}", acquisitionFile.Internal_Id);
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileEdit);

            // Get the current checklist items for this acquisition file.
            var currentItems = _checklistRepository.GetAllChecklistItemsByAcquisitionFileId(acquisitionFile.Internal_Id).ToDictionary(ci => ci.Internal_Id);

            foreach (var incomingItem in acquisitionFile.PimsAcquisitionChecklistItems)
            {
                if (!currentItems.TryGetValue(incomingItem.Internal_Id, out var existingItem))
                {
                    throw new BadRequestException($"Cannot update checklist item. Item with Id: {incomingItem.Internal_Id} not found.");
                }

                // Only update checklist items that changed.
                if (existingItem.AcqChklstItemStatusTypeCode != incomingItem.AcqChklstItemStatusTypeCode)
                {
                    _checklistRepository.Update(incomingItem);
                }
            }

            _checklistRepository.CommitTransaction();
            return _acqFileRepository.GetById(acquisitionFile.Internal_Id);
        }

        private static void ValidateStaff(PimsAcquisitionFile pimsAcquisitionFile)
        {
            bool duplicate = pimsAcquisitionFile.PimsAcquisitionFilePeople.GroupBy(p => (p.AcqFlPersonProfileTypeCode, p.PersonId)).Any(g => g.Count() > 1);
            if (duplicate)
            {
                throw new BadRequestException("Invalid Acquisition team, each team member and role combination can only be added once.");
            }
        }

        private void MatchProperties(PimsAcquisitionFile acquisitionFile)
        {
            foreach (var acquisitionProperty in acquisitionFile.PimsPropertyAcquisitionFiles)
            {
                if (acquisitionProperty.Property.Pid.HasValue)
                {
                    var pid = acquisitionProperty.Property.Pid.Value;
                    try
                    {
                        var foundProperty = _propertyRepository.GetByPid(pid);
                        acquisitionProperty.PropertyId = foundProperty.Internal_Id;
                        acquisitionProperty.Property = foundProperty;
                    }
                    catch (KeyNotFoundException)
                    {
                        _logger.LogDebug("Adding new property with pid:{pid}", pid);
                        PopulateNewProperty(acquisitionProperty.Property);
                    }
                }
                else if (acquisitionProperty.Property.Pin.HasValue)
                {
                    var pin = acquisitionProperty.Property.Pin.Value;
                    try
                    {
                        var foundProperty = _propertyRepository.GetByPin(pin);
                        acquisitionProperty.PropertyId = foundProperty.Internal_Id;
                        acquisitionProperty.Property = foundProperty;
                    }
                    catch (KeyNotFoundException)
                    {
                        _logger.LogDebug("Adding new property with pin:{pin}", pin);
                        PopulateNewProperty(acquisitionProperty.Property);
                    }
                }
                else
                {
                    _logger.LogDebug("Adding new property without a pid");
                    PopulateNewProperty(acquisitionProperty.Property);
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

            if (property.Address != null)
            {
                var provinceId = _lookupRepository.GetAllProvinces().FirstOrDefault(p => p.ProvinceStateCode == "BC")?.Id;
                if (provinceId.HasValue)
                {
                    property.Address.ProvinceStateId = provinceId.Value;
                }
                property.Address.CountryId = _lookupRepository.GetAllCountries().FirstOrDefault(p => p.CountryCode == "CA")?.Id;
            }

            // convert spatial location from lat/long (4326) to BC Albers (3005) for database storage
            var geom = property.Location;
            if (geom.SRID != SpatialReference.BCALBERS)
            {
                var newCoords = _coordinateService.TransformCoordinates(geom.SRID, SpatialReference.BCALBERS, geom.Coordinate);
                property.Location = GeometryHelper.CreatePoint(newCoords, SpatialReference.BCALBERS);
            }
        }

        private void ReprojectPropertyLocationsToWgs84(IEnumerable<PimsPropertyAcquisitionFile> propertyAcquisitionFiles)
        {
            foreach (var acquisitionProperty in propertyAcquisitionFiles)
            {
                if (acquisitionProperty.Property.Location != null)
                {
                    var oldCoords = acquisitionProperty.Property.Location.Coordinate;
                    var newCoords = _coordinateService.TransformCoordinates(SpatialReference.BCALBERS, SpatialReference.WGS84, oldCoords);
                    acquisitionProperty.Property.Location = GeometryHelper.CreatePoint(newCoords, SpatialReference.WGS84);
                }
            }
        }

        private void ValidateVersion(long acqFileId, long acqFileVersion)
        {
            long currentRowVersion = _acqFileRepository.GetRowVersion(acqFileId);
            if (currentRowVersion != acqFileVersion)
            {
                throw new DbUpdateConcurrencyException("You are working with an older version of this acquisition file, please refresh the application and retry.");
            }
        }

        private void ValidateMinistryRegion(long acqFileId, short updatedRegion)
        {
            short currentRegion = _acqFileRepository.GetRegion(acqFileId);
            if (currentRegion != updatedRegion)
            {
                throw new BusinessRuleViolationException("The Ministry region has been changed, this will result in a change to the file's prefix. This requires user confirmation.");
            }
        }

        private void AddNoteIfStatusChanged(PimsAcquisitionFile updateAcquisitionFile)
        {
            var currentAcquisitionFile = _acqFileRepository.GetById(updateAcquisitionFile.Internal_Id);
            bool statusChanged = currentAcquisitionFile.AcquisitionFileStatusTypeCode != updateAcquisitionFile.AcquisitionFileStatusTypeCode;
            if (!statusChanged)
            {
                return;
            }

            var newStatus = _lookupRepository.GetAllAcquisitionFileStatusTypes()
                .FirstOrDefault(x => x.AcquisitionFileStatusTypeCode == updateAcquisitionFile.AcquisitionFileStatusTypeCode);

            PimsAcquisitionFileNote fileNoteInstance = new()
            {
                AcquisitionFileId = updateAcquisitionFile.Internal_Id,
                AppCreateTimestamp = DateTime.Now,
                AppCreateUserid = _user.GetUsername(),
                Note = new PimsNote()
                {
                    IsSystemGenerated = true,
                    NoteTxt = $"Acquisition File status changed from {currentAcquisitionFile.AcquisitionFileStatusTypeCodeNavigation.Description} to {newStatus.Description}",
                    AppCreateTimestamp = DateTime.Now,
                    AppCreateUserid = this._user.GetUsername(),
                },
            };

            _entityNoteRepository.Add(fileNoteInstance);
        }

        private void PopulateAcquisitionChecklist(PimsAcquisitionFile acquisitionFile)
        {
            // Ensure the checklist is empty before populating it
            acquisitionFile.PimsAcquisitionChecklistItems.Clear();

            foreach (var itemType in _checklistRepository.GetAllChecklistItemTypes().Where(x => !x.IsExpiredType()))
            {
                var checklistItem = new PimsAcquisitionChecklistItem
                {
                    AcqChklstItemTypeCode = itemType.AcqChklstItemTypeCode,
                    AcqChklstItemStatusTypeCode = "INCOMP",
                };

                acquisitionFile.PimsAcquisitionChecklistItems.Add(checklistItem);
            }
        }
    }
}
