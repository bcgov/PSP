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
using Pims.Dal.Entities.Extensions;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
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
        private readonly IAgreementRepository _agreementRepository;
        private readonly ICompensationRequisitionRepository _compensationRequisitionRepository;
        private readonly IInterestHolderRepository _interestHolderRepository;
        private readonly ICompReqH120Service _compReqH120Service;

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
            IAcquisitionFileChecklistRepository checklistRepository,
            IAgreementRepository agreementRepository,
            ICompensationRequisitionRepository compensationRequisitionRepository,
            IInterestHolderRepository interestHolderRepository,
            ICompReqH120Service compReqH120Service)
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
            _agreementRepository = agreementRepository;
            _compensationRequisitionRepository = compensationRequisitionRepository;
            _interestHolderRepository = interestHolderRepository;
            _compReqH120Service = compReqH120Service;
        }

        public Paged<PimsAcquisitionFile> GetPage(AcquisitionFilter filter)
        {
            _logger.LogInformation("Searching for acquisition files...");
            _logger.LogDebug("Acquisition file search with filter", filter);

            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileView);

            // Limit search results to user's assigned region(s)
            var pimsUser = _userRepository.GetUserInfoByKeycloakUserId(_user.GetUserKey());
            var userRegions = pimsUser.PimsRegionUsers.Select(r => r.RegionCode).ToHashSet();
            long? personId = pimsUser.IsContractor ? pimsUser.PersonId : null;

            return _acqFileRepository.GetPage(filter, userRegions, personId);
        }

        public PimsAcquisitionFile GetById(long id)
        {
            _logger.LogInformation("Getting acquisition file with id {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileView);
            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, id);

            var acqFile = _acqFileRepository.GetById(id);

            return acqFile;
        }

        public IEnumerable<PimsPropertyAcquisitionFile> GetProperties(long id)
        {
            _logger.LogInformation("Getting acquisition file with id {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileView);
            _user.ThrowIfNotAuthorized(Permissions.PropertyView);
            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, id);

            var properties = _acquisitionFilePropertyRepository.GetPropertiesByAcquisitionFileId(id);
            ReprojectPropertyLocationsToWgs84(properties);
            return properties;
        }

        public IEnumerable<PimsAcquisitionOwner> GetOwners(long id)
        {
            _logger.LogInformation("Getting acquisition file owners with AcquisitionFile id: {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileView);
            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, id);

            return _acquisitionFilePropertyRepository.GetOwnersByAcquisitionFileId(id);
        }

        public IEnumerable<PimsAcquisitionChecklistItem> GetChecklistItems(long id)
        {
            _logger.LogInformation("Getting acquisition file checklist with AcquisitionFile id: {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileView);
            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, id);

            var checklistItems = _checklistRepository.GetAllChecklistItemsByAcquisitionFileId(id);
            var acquisitionFile = _acqFileRepository.GetById(id);
            AppendToAcquisitionChecklist(acquisitionFile, ref checklistItems);

            return checklistItems;
        }

        public PimsAcquisitionFile Add(PimsAcquisitionFile acquisitionFile, IEnumerable<UserOverrideCode> userOverrides)
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
            MatchProperties(acquisitionFile, userOverrides);
            PopulateAcquisitionChecklist(acquisitionFile);

            var newAcqFile = _acqFileRepository.Add(acquisitionFile);
            _acqFileRepository.CommitTransaction();
            return newAcqFile;
        }

        public PimsAcquisitionFile Update(PimsAcquisitionFile acquisitionFile, IEnumerable<UserOverrideCode> userOverrides)
        {
            acquisitionFile.ThrowIfNull(nameof(acquisitionFile));

            _logger.LogInformation("Updating acquisition file with id {id}", acquisitionFile.Internal_Id);
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileEdit);

            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, acquisitionFile.Internal_Id);
            ValidateVersion(acquisitionFile.Internal_Id, acquisitionFile.ConcurrencyControlNumber);

            if (!userOverrides.Contains(UserOverrideCode.UpdateRegion))
            {
                ValidateMinistryRegion(acquisitionFile.Internal_Id, acquisitionFile.RegionCode);
            }

            if (acquisitionFile.AcquisitionFileStatusTypeCode == "COMPLT")
            {
                TransferPropertiesOfInterestToInventory(acquisitionFile, userOverrides.Contains(UserOverrideCode.PoiToInventory));
            }

            ValidateStaff(acquisitionFile);

            ValidatePayeeDependency(acquisitionFile);

            ValidateNewTotalAllowableCompensation(acquisitionFile.Internal_Id, acquisitionFile.TotalAllowableCompensation);

            // reset the region
            var cannotDetermineRegion = _lookupRepository.GetAllRegions().FirstOrDefault(x => x.RegionName == "Cannot determine");
            if (acquisitionFile.RegionCode == cannotDetermineRegion.RegionCode)
            {
                throw new BadRequestException("Cannot set an acquisition file's region to 'cannot determine'");
            }

            _acqFileRepository.Update(acquisitionFile);
            AddNoteIfStatusChanged(acquisitionFile);

            _acqFileRepository.CommitTransaction();
            return _acqFileRepository.GetById(acquisitionFile.Internal_Id);
        }

        public PimsAcquisitionFile UpdateProperties(PimsAcquisitionFile acquisitionFile, IEnumerable<UserOverrideCode> userOverrides)
        {
            _logger.LogInformation("Updating acquisition file properties...");
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileEdit, Permissions.PropertyView, Permissions.PropertyAdd);
            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, acquisitionFile.Internal_Id);

            ValidateVersion(acquisitionFile.Internal_Id, acquisitionFile.ConcurrencyControlNumber);

            MatchProperties(acquisitionFile, userOverrides);

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
            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, acquisitionFile.Internal_Id);

            // Get the current checklist items for this acquisition file.
            var currentItems = _checklistRepository.GetAllChecklistItemsByAcquisitionFileId(acquisitionFile.Internal_Id).ToDictionary(ci => ci.Internal_Id);

            foreach (var incomingItem in acquisitionFile.PimsAcquisitionChecklistItems)
            {
                if (!currentItems.TryGetValue(incomingItem.Internal_Id, out var existingItem) && incomingItem.Internal_Id != 0)
                {
                    throw new BadRequestException($"Cannot update checklist item. Item with Id: {incomingItem.Internal_Id} not found.");
                }

                // Only update checklist items that changed.
                if (existingItem == null)
                {
                    _checklistRepository.Add(incomingItem);
                }
                else if (existingItem.AcqChklstItemStatusTypeCode != incomingItem.AcqChklstItemStatusTypeCode)
                {
                    _checklistRepository.Update(incomingItem);
                }
            }

            _checklistRepository.CommitTransaction();
            return _acqFileRepository.GetById(acquisitionFile.Internal_Id);
        }

        public IEnumerable<PimsAgreement> GetAgreements(long id)
        {
            _logger.LogInformation("Getting acquisition file agreements with AcquisitionFile id: {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.AgreementView);
            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, id);

            return _agreementRepository.GetAgreementsByAquisitionFile(id);
        }

        public IEnumerable<PimsAgreement> UpdateAgreements(long acquisitionFileId, List<PimsAgreement> agreements)
        {
            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, acquisitionFileId);

            var updatedAgreements = _agreementRepository.UpdateAllForAcquisition(acquisitionFileId, agreements);
            _agreementRepository.CommitTransaction();

            return updatedAgreements;
        }

        public IEnumerable<PimsInterestHolder> GetInterestHolders(long id)
        {
            _logger.LogInformation("Getting acquisition file InterestHolders with AcquisitionFile id: {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileView);
            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, id);

            return _interestHolderRepository.GetInterestHoldersByAcquisitionFile(id);
        }

        public IEnumerable<PimsInterestHolder> UpdateInterestHolders(long acquisitionFileId, List<PimsInterestHolder> interestHolders)
        {
            _logger.LogInformation("Updating acquisition file InterestHolders with AcquisitionFile id: {acquisitionFileId}", acquisitionFileId);
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileEdit);
            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, acquisitionFileId);

            ValidateInterestHoldersDependency(acquisitionFileId, interestHolders);
            _interestHolderRepository.UpdateAllForAcquisition(acquisitionFileId, interestHolders);
            _interestHolderRepository.CommitTransaction();

            return _interestHolderRepository.GetInterestHoldersByAcquisitionFile(acquisitionFileId);
        }

        public IList<PimsCompensationRequisition> GetAcquisitionCompensations(long acquisitionFileId)
        {
            _logger.LogInformation("Getting compensations for acquisition file id ...", acquisitionFileId);
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView, Permissions.AcquisitionFileView);
            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, acquisitionFileId);

            return _compensationRequisitionRepository.GetAllByAcquisitionFileId(acquisitionFileId);
        }

        public PimsCompensationRequisition AddCompensationRequisition(long acquisitionFileId, PimsCompensationRequisition compensationRequisition)
        {
            _logger.LogInformation("Adding compensation requisition for acquisition file id ...", acquisitionFileId);

            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionAdd);
            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, acquisitionFileId);

            compensationRequisition.ThrowIfNull(nameof(compensationRequisition));

            var acquisitionFileParent = _acqFileRepository.GetById(acquisitionFileId);
            if (acquisitionFileId != compensationRequisition.AcquisitionFileId || acquisitionFileParent is null)
            {
                throw new BadRequestException("Invalid acquisitionFileId.");
            }

            compensationRequisition.IsDraft = compensationRequisition.IsDraft ?? true;
            compensationRequisition.PimsAcquisitionPayees = new List<PimsAcquisitionPayee>() { new PimsAcquisitionPayee() };

            var newCompensationRequisition = _compensationRequisitionRepository.Add(compensationRequisition);

            _compensationRequisitionRepository.CommitTransaction();

            return newCompensationRequisition;
        }

        private static void ValidateStaff(PimsAcquisitionFile pimsAcquisitionFile)
        {
            bool duplicate = pimsAcquisitionFile.PimsAcquisitionFilePeople.GroupBy(p => (p.AcqFlPersonProfileTypeCode, p.PersonId)).Any(g => g.Count() > 1);
            if (duplicate)
            {
                throw new BadRequestException("Invalid Acquisition team, each team member and role combination can only be added once.");
            }
        }

        private void UpdateLocation(PimsProperty researchProperty, ref PimsProperty propertyToUpdate, IEnumerable<UserOverrideCode> overrideCodes)
        {
            if (propertyToUpdate.Location == null)
            {
                if (overrideCodes.Contains(UserOverrideCode.AddLocationToProperty))
                {
                    // convert spatial location from lat/long (4326) to BC Albers (3005) for database storage
                    var geom = researchProperty.Location;
                    if (geom.SRID != SpatialReference.BCALBERS)
                    {
                        var newCoords = _coordinateService.TransformCoordinates(geom.SRID, SpatialReference.BCALBERS, geom.Coordinate);
                        propertyToUpdate.Location = GeometryHelper.CreatePoint(newCoords, SpatialReference.BCALBERS);
                        _propertyRepository.Update(propertyToUpdate, overrideLocation: true);
                    }
                }
                else
                {
                    throw new UserOverrideException(UserOverrideCode.AddLocationToProperty, "The selected property already exists in the system's inventory. However, the record is missing spatial details.\n\n To add the property, the spatial details for this property will need to be updated. The system will attempt to update the property record with spatial information from the current selection.");
                }
            }
        }

        private void MatchProperties(PimsAcquisitionFile acquisitionFile, IEnumerable<UserOverrideCode> userOverrideCodes)
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
                        UpdateLocation(acquisitionProperty.Property, ref foundProperty, userOverrideCodes);
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
                        UpdateLocation(acquisitionProperty.Property, ref foundProperty, userOverrideCodes);
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

        private void ValidateNewTotalAllowableCompensation(long currentAcquisitionFileId, decimal? newAllowableCompensation)
        {
            if(!newAllowableCompensation.HasValue)
            {
                return;
            }
            IEnumerable<PimsCompReqH120> allFinalFinancialsOnFile = _compReqH120Service.GetAllByAcquisitionFileId(currentAcquisitionFileId, true);
            var currentActualCompensation = allFinalFinancialsOnFile.Aggregate(0m, (acc, f) => acc + (f.TotalAmt ?? 0m));
            if(newAllowableCompensation < currentActualCompensation)
            {
                throw new BusinessRuleViolationException("The Total Allowable Compensation value cannot be saved because the value provided is less than current sum of the final compensation requisitions in this file. " +
                    "\n\nTo continue, adjust the value to accommodate the existing compensation requisitions in the file or contact your system administrator to adjust final compensations.");
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
                throw new UserOverrideException(UserOverrideCode.UpdateRegion, "The Ministry region has been changed, this will result in a change to the file's prefix. This requires user confirmation.");
            }
        }

        /// <summary>
        /// Attempt to transfer properties of interest to core inventory when an acquisition file is deemed to be completed.
        ///
        /// By default, do not allow a property of interest to be modified unless the userOverride flag is true.
        /// </summary>
        /// <param name="acquisitionFile"></param>
        /// <param name="userOverride"></param>
        private void TransferPropertiesOfInterestToInventory(PimsAcquisitionFile acquisitionFile, bool userOverride = false)
        {
            // Get the current properties in the research file
            var currentProperties = _acquisitionFilePropertyRepository.GetPropertiesByAcquisitionFileId(acquisitionFile.Internal_Id);
            var propertiesOfInterest = currentProperties.Where(p => p.Property.IsPropertyOfInterest.HasValue && p.Property.IsPropertyOfInterest.Value);

            // PSP-6111 Business rule: Transfer properties of interest to core inventory when acquisition file is completed
            foreach (var acquisitionProperty in propertiesOfInterest)
            {
                var property = acquisitionProperty.Property;
                if (!userOverride)
                {
                    throw new UserOverrideException(UserOverrideCode.PoiToInventory, "The properties of interest will be added to the inventory as acquired properties.");
                }
                _propertyRepository.TransferToCoreInventory(property);
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

        private void AppendToAcquisitionChecklist(PimsAcquisitionFile acquisitionFile, ref List<PimsAcquisitionChecklistItem> pimsAcquisitionChecklistItems)
        {
            var doNotAddToStatuses = new List<string>() { "COMPLT", "CANCEL", "ARCHIV" };
            if (doNotAddToStatuses.Contains(acquisitionFile.AcqPhysFileStatusTypeCode))
            {
                return;
            }
            var checklistStatusTypes = _lookupRepository.GetAllAcquisitionChecklistItemStatusTypes();
            foreach (var itemType in _checklistRepository.GetAllChecklistItemTypes().Where(x => !x.IsExpiredType()))
            {
                if (!pimsAcquisitionChecklistItems.Any(cli => cli.AcqChklstItemTypeCode == itemType.AcqChklstItemTypeCode) && acquisitionFile.AppCreateTimestamp >= itemType.EffectiveDate)
                {
                    var checklistItem = new PimsAcquisitionChecklistItem
                    {
                        AcqChklstItemTypeCode = itemType.AcqChklstItemTypeCode,
                        AcqChklstItemTypeCodeNavigation = itemType,
                        AcqChklstItemStatusTypeCode = "INCOMP",
                        AcquisitionFileId = acquisitionFile.AcquisitionFileId,
                        AcqChklstItemStatusTypeCodeNavigation = checklistStatusTypes.FirstOrDefault(cst => cst.Id == "INCOMP"),
                    };

                    pimsAcquisitionChecklistItems.Add(checklistItem);
                }
            }
        }

        private void ValidatePayeeDependency(PimsAcquisitionFile acquisitionFile)
        {
            var currentAquisitionFile = _acqFileRepository.GetById(acquisitionFile.Internal_Id);
            var compensationRequisitions = _compensationRequisitionRepository.GetAllByAcquisitionFileId(acquisitionFile.Internal_Id);

            if (compensationRequisitions.Count == 0 || !compensationRequisitions.Any(y => y.PimsAcquisitionPayees.Count > 0))
            {
                return;
            }

            foreach (var compReq in compensationRequisitions)
            {
                var payee = compReq.PimsAcquisitionPayees.FirstOrDefault();
                if (payee is null || !payee.HasPayeeAssigned)
                {
                    continue;
                }

                // Check for Acquisition File Owner removed
                if (payee.AcquisitionOwnerId is not null
                    && !acquisitionFile.PimsAcquisitionOwners.Any(x => x.Internal_Id.Equals(payee.AcquisitionOwnerId))
                    && currentAquisitionFile.PimsAcquisitionOwners.Any(x => x.Internal_Id.Equals(payee.AcquisitionOwnerId)))
                {
                    throw new ForeignKeyDependencyException("Acquisition File Owner can not be removed since it's assigned as a payee for a compensation requisition");
                }

                // Check for Acquisition InterestHolders
                if (payee.InterestHolderId is not null
                    && !acquisitionFile.PimsInterestHolders.Any(x => x.Internal_Id.Equals(payee.InterestHolderId))
                    && currentAquisitionFile.PimsInterestHolders.Any(x => x.Internal_Id.Equals(payee.InterestHolderId)))
                {
                    throw new ForeignKeyDependencyException("Acquisition File Interest Holders can not be removed since it's assigned as a payee for a compensation requisition");
                }

                // Check for File Person
                if (payee.AcquisitionFilePersonId is not null
                    && !acquisitionFile.PimsAcquisitionFilePeople.Any(x => x.Internal_Id.Equals(payee.AcquisitionFilePersonId))
                    && currentAquisitionFile.PimsAcquisitionFilePeople.Any(x => x.Internal_Id.Equals(payee.AcquisitionFilePersonId)))
                {
                    throw new ForeignKeyDependencyException("Acquisition File team member can not be removed since it's assigned as a payee for a compensation requisition");
                }
            }
        }

        private void ValidateInterestHoldersDependency(long acquisitionFileId, List<PimsInterestHolder> interestHolders)
        {
            var currentAquisitionFile = _acqFileRepository.GetById(acquisitionFileId);
            var compensationRequisitions = _compensationRequisitionRepository.GetAllByAcquisitionFileId(acquisitionFileId);

            if (compensationRequisitions.Count == 0 || !compensationRequisitions.Any(y => y.PimsAcquisitionPayees.Count > 0))
            {
                return;
            }

            foreach (var compReq in compensationRequisitions)
            {
                var payee = compReq.PimsAcquisitionPayees.FirstOrDefault();
                if (payee is null || !payee.HasPayeeAssigned)
                {
                    continue;
                }

                // Check for Interest Holder
                if (payee.InterestHolderId is not null
                && !interestHolders.Any(x => x.InterestHolderId.Equals(payee.InterestHolderId))
                && currentAquisitionFile.PimsInterestHolders.Any(x => x.Internal_Id.Equals(payee.InterestHolderId)))
                {
                    throw new ForeignKeyDependencyException("Acquisition File Interest Holder can not be removed since it's assigned as a payee for a compensation requisition");
                }
            }

        }
    }
}
