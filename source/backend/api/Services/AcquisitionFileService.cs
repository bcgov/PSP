using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Helpers.Extensions;
using Pims.Api.Models.CodeTypes;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
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
        private readonly ICompReqFinancialService _compReqFinancialService;
        private readonly IExpropriationPaymentRepository _expropriationPaymentRepository;
        private readonly ITakeRepository _takeRepository;
        private readonly IAcquisitionStatusSolver _statusSolver;
        private readonly IPropertyService _propertyService;

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
            ICompReqFinancialService compReqFinancialService,
            IExpropriationPaymentRepository expropriationPaymentRepository,
            ITakeRepository takeRepository,
            IAcquisitionStatusSolver statusSolver,
            IPropertyService propertyService)
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
            _compReqFinancialService = compReqFinancialService;
            _expropriationPaymentRepository = expropriationPaymentRepository;
            _takeRepository = takeRepository;
            _statusSolver = statusSolver;
            _propertyService = propertyService;
        }

        public Paged<PimsAcquisitionFile> GetPage(AcquisitionFilter filter)
        {
            _logger.LogInformation("Searching for acquisition files...");
            _logger.LogDebug("Acquisition file search with filter: {filter}", filter);

            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileView);

            // Limit search results to user's assigned region(s)
            var pimsUser = _userRepository.GetUserInfoByKeycloakUserId(_user.GetUserKey());
            var userRegions = pimsUser.PimsRegionUsers.Select(r => r.RegionCode).ToHashSet();
            long? contractorPersonId = pimsUser.IsContractor ? pimsUser.PersonId : null;

            return _acqFileRepository.GetPageDeep(filter, userRegions, contractorPersonId);
        }

        public List<AcquisitionFileExportModel> GetAcquisitionFileExport(AcquisitionFilter filter)
        {
            _logger.LogInformation("Searching all Acquisition Files matching the filter: {filter}", filter);
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileView);

            // Limit search results to user's assigned region(s)
            var pimsUser = _userRepository.GetUserInfoByKeycloakUserId(_user.GetUserKey());
            var userRegions = pimsUser.PimsRegionUsers.Select(r => r.RegionCode).ToHashSet();
            long? contractorPersonId = pimsUser.IsContractor ? pimsUser.PersonId : null;

            var acqFiles = _acqFileRepository.GetAcquisitionFileExportDeep(filter, userRegions, contractorPersonId);

            return acqFiles.SelectMany(file => file.PimsPropertyAcquisitionFiles.Where(fp => fp.AcquisitionFileId.Equals(file.AcquisitionFileId)).DefaultIfEmpty(), (file, fp) => (file, fp))
                .Select(fileProperty => new AcquisitionFileExportModel
                {
                    FileNumber = fileProperty.file.FileNumber ?? string.Empty,
                    LegacyFileNumber = fileProperty.file.LegacyFileNumber ?? string.Empty,
                    FileName = fileProperty.file.FileName ?? string.Empty,
                    MotiRegion = fileProperty.file.RegionCodeNavigation?.Description ?? string.Empty,
                    MinistryProject = fileProperty.file.Project is not null ? $"{fileProperty.file.Project.Code} {fileProperty.file.Project.Description}" : string.Empty,
                    CivicAddress = (fileProperty.fp?.Property is not null && fileProperty.fp.Property.Address is not null) ? fileProperty.fp.Property.Address.FormatFullAddressString() : string.Empty,
                    GeneralLocation = (fileProperty.fp?.Property is not null) ? fileProperty.fp.Property.GeneralLocation : string.Empty,
                    Pid = fileProperty.fp is not null && fileProperty.fp.Property.Pid.HasValue ? fileProperty.fp.Property.Pid.ToString() : string.Empty,
                    Pin = fileProperty.fp is not null && fileProperty.fp.Property.Pin.HasValue ? fileProperty.fp.Property.Pin.ToString() : string.Empty,
                    AcquisitionFileStatusTypeCode = fileProperty.file.AcquisitionFileStatusTypeCodeNavigation is not null ? fileProperty.file.AcquisitionFileStatusTypeCodeNavigation.Description : string.Empty,
                    FileFunding = fileProperty.file.AcquisitionFundingTypeCodeNavigation is not null ? fileProperty.file.AcquisitionFundingTypeCodeNavigation.Description : string.Empty,
                    FileAssignedDate = fileProperty.file.AssignedDate.HasValue ? fileProperty.file.AssignedDate.Value.ToString("dd-MMM-yyyy") : string.Empty,
                    FileDeliveryDate = fileProperty.file.DeliveryDate.HasValue ? fileProperty.file.DeliveryDate.Value.ToString("dd-MMM-yyyy") : string.Empty,
                    FileAcquisitionCompleted = fileProperty.file.CompletionDate.HasValue ? fileProperty.file.CompletionDate.Value.ToString("dd-MMM-yyyy") : string.Empty,
                    FilePhysicalStatus = fileProperty.file.AcqPhysFileStatusTypeCodeNavigation is not null ? fileProperty.file.AcqPhysFileStatusTypeCodeNavigation.Description : string.Empty,
                    FileAcquisitionType = fileProperty.file.AcquisitionTypeCodeNavigation is not null ? fileProperty.file.AcquisitionTypeCodeNavigation.Description : string.Empty,
                    FileAcquisitionTeam = string.Join(", ", fileProperty.file.PimsAcquisitionFileTeams.Select(x => x.PersonId.HasValue ? x.Person.GetFullName(true) : x.Organization.Name)),
                    FileAcquisitionOwners = string.Join(", ", fileProperty.file.PimsAcquisitionOwners.Select(x => x.FormatOwnerName())),
                }).ToList();
        }

        public PimsAcquisitionFile GetById(long id)
        {
            _logger.LogInformation("Getting acquisition file with id {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileView);
            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, id);

            var acqFile = _acqFileRepository.GetById(id);

            return acqFile;
        }

        public LastUpdatedByModel GetLastUpdateInformation(long acquisitionFileId)
        {
            _logger.LogInformation("Retrieving last updated-by information...");
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileView);

            return _acqFileRepository.GetLastUpdateBy(acquisitionFileId);
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

            return _acqFileRepository.GetOwnersByAcquisitionFileId(id);
        }

        public IEnumerable<PimsAcquisitionFileTeam> GetTeamMembers()
        {
            _logger.LogInformation("Getting acquisition team members");
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileView);

            var pimsUser = _userRepository.GetUserInfoByKeycloakUserId(_user.GetUserKey());
            var userRegions = pimsUser.PimsRegionUsers.Select(r => r.RegionCode).ToHashSet();
            long? contractorPersonId = pimsUser.IsContractor ? pimsUser.PersonId : null;

            var teamMembers = _acqFileRepository.GetTeamMembers(userRegions, contractorPersonId);

            var persons = teamMembers.Where(x => x.Person != null).GroupBy(x => x.PersonId).Select(x => x.First()).ToList();
            var organizations = teamMembers.Where(x => x.Organization != null).GroupBy(x => x.OrganizationId).Select(x => x.First()).ToList();

            List<PimsAcquisitionFileTeam> teamFilterOptions = new();
            teamFilterOptions.AddRange(persons);
            teamFilterOptions.AddRange(organizations);

            return teamFilterOptions;
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
            acquisitionFile.ThrowMissingContractorInTeam(_user, _userRepository);

            // validate the new acq region
            var cannotDetermineRegion = _lookupRepository.GetAllRegions().FirstOrDefault(x => x.RegionName == "Cannot determine");
            if (acquisitionFile.RegionCode == cannotDetermineRegion.RegionCode)
            {
                throw new BadRequestException("Cannot set an acquisition file's region to 'cannot determine'");
            }

            ValidateStaff(acquisitionFile);
            ValidateOrganizationStaff(acquisitionFile);

            MatchProperties(acquisitionFile, userOverrides);
            ValidatePropertyRegions(acquisitionFile);

            PopulateAcquisitionChecklist(acquisitionFile);

            acquisitionFile.AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString();
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

            AcquisitionStatusTypes? currentAcquisitionStatus = GetCurrentAcquisitionStatus(acquisitionFile.Internal_Id);

            if (currentAcquisitionStatus != AcquisitionStatusTypes.COMPLT && acquisitionFile.AcquisitionFileStatusTypeCode == AcquisitionStatusTypes.COMPLT.ToString())
            {
                ValidateDraftsOnComplete(acquisitionFile);
            }

            if (!_statusSolver.CanEditDetails(currentAcquisitionStatus) && !_user.HasPermission(Permissions.SystemAdmin))
            {
                throw new BusinessRuleViolationException("The file you are editing is not active or draft, so you cannot save changes. Refresh your browser to see file state.");
            }

            if (!userOverrides.Contains(UserOverrideCode.UpdateRegion))
            {
                ValidateMinistryRegion(acquisitionFile.Internal_Id, acquisitionFile.RegionCode);
            }

            ValidateStaff(acquisitionFile);
            ValidateOrganizationStaff(acquisitionFile);

            acquisitionFile.ThrowContractorRemovedFromTeam(_user, _userRepository);

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
            _user.ThrowIfNotAllAuthorized(Permissions.AcquisitionFileEdit, Permissions.PropertyView, Permissions.PropertyAdd);
            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, acquisitionFile.Internal_Id);

            ValidateVersion(acquisitionFile.Internal_Id, acquisitionFile.ConcurrencyControlNumber);

            MatchProperties(acquisitionFile, userOverrides);

            ValidatePropertyRegions(acquisitionFile);

            AcquisitionStatusTypes? currentAcquisitionStatus = GetCurrentAcquisitionStatus(acquisitionFile.Internal_Id);
            if (!_statusSolver.CanEditProperties(currentAcquisitionStatus) && !_user.HasPermission(Permissions.SystemAdmin))
            {
                throw new BusinessRuleViolationException("The file you are editing is not active or draft, so you cannot save changes. Refresh your browser to see file state.");
            }

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
                if (acqFileProperties.PimsTakes.Any() || acqFileProperties.PimsInthldrPropInterests.Any())
                {
                    throw new BusinessRuleViolationException("You must remove all takes and interest holders from an acquisition file property before removing that property from an acquisition file");
                }
                _acquisitionFilePropertyRepository.Delete(deletedProperty);

                var totalAssociationCount = _propertyRepository.GetAllAssociationsCountById(deletedProperty.PropertyId);
                if (totalAssociationCount <= 1)
                {
                    _acqFileRepository.CommitTransaction(); // TODO: this can only be removed if cascade deletes are implemented. EF executes deletes in alphabetic order.
                    _propertyRepository.Delete(deletedProperty.Property);
                }

            }

            _acqFileRepository.CommitTransaction();
            return _acqFileRepository.GetById(acquisitionFile.Internal_Id);
        }

        public PimsAcquisitionFile UpdateChecklistItems(IList<PimsAcquisitionChecklistItem> checklistItems)
        {
            checklistItems.ThrowIfNull(nameof(checklistItems));
            if (checklistItems.Count == 0)
            {
                throw new BadRequestException("Checklist items must be greater than zero");
            }

            var acquisitionFileId = checklistItems.FirstOrDefault().AcquisitionFileId;

            _logger.LogInformation("Updating acquisition file checklist with AcquisitionFile id: {id}", acquisitionFileId);
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileEdit);
            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, acquisitionFileId);

            var currentAcquisitionStatus = GetCurrentAcquisitionStatus(acquisitionFileId);
            if (!_statusSolver.CanEditChecklists(currentAcquisitionStatus) && !_user.HasPermission(Permissions.SystemAdmin))
            {
                throw new BusinessRuleViolationException("The file you are editing is not active or draft, so you cannot save changes. Refresh your browser to see file state.");
            }

            // Get the current checklist items for this acquisition file.
            var currentItems = _checklistRepository.GetAllChecklistItemsByAcquisitionFileId(acquisitionFileId).ToDictionary(ci => ci.Internal_Id);

            foreach (var incomingItem in checklistItems)
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
            return _acqFileRepository.GetById(acquisitionFileId);
        }

        public IEnumerable<PimsAgreement> GetAgreements(long id)
        {
            _logger.LogInformation("Getting acquisition file agreements with AcquisitionFile id: {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.AgreementView);
            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, id);

            return _agreementRepository.GetAgreementsByAcquisitionFile(id);
        }

        public PimsAgreement AddAgreement(long acquisitionFileId, PimsAgreement agreement)
        {
            _user.ThrowIfNotAuthorized(Permissions.AgreementView);
            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, acquisitionFileId);

            ValidateAcquisitionFileStatus(acquisitionFileId, agreement.AgreementStatusTypeCode);

            var newAgreement = _agreementRepository.AddAgreement(agreement);
            _agreementRepository.CommitTransaction();

            return newAgreement;
        }

        public PimsAgreement GetAgreementById(long acquisitionFileId, long agreementId)
        {
            _logger.LogInformation("Getting acquisition file agreement with Agreement id: {agreementId}", agreementId);
            _user.ThrowIfNotAuthorized(Permissions.AgreementView);
            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, acquisitionFileId);

            return _agreementRepository.GetAgreementById(agreementId);
        }

        public IEnumerable<PimsAgreement> SearchAgreements(AcquisitionReportFilterModel filter)
        {
            _logger.LogInformation("Searching all agreements matching the filter: {filter} ", filter);
            _user.ThrowIfNotAuthorized(Permissions.AgreementView);
            var pimsUser = _userRepository.GetUserInfoByKeycloakUserId(_user.GetUserKey());
            var allMatchingAgreements = _agreementRepository.SearchAgreements(filter);
            if (pimsUser.IsContractor)
            {
                return allMatchingAgreements.Where(a => a.AcquisitionFile.PimsAcquisitionFileTeams.Any(afp => afp.PersonId == pimsUser.PersonId));
            }

            return allMatchingAgreements.Where(a => pimsUser.PimsRegionUsers.Any(ur => ur.RegionCode == a.AcquisitionFile.RegionCode));
        }

        public PimsAgreement UpdateAgreement(long acquisitionFileId, PimsAgreement agreement)
        {
            _user.ThrowIfNotAuthorized(Permissions.AgreementView);
            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, acquisitionFileId);

            var currentAgreement = _agreementRepository.GetAgreementById(agreement.AgreementId);

            ValidateAcquisitionFileStatus(acquisitionFileId, currentAgreement.AgreementStatusTypeCode);

            var updatedAgreement = _agreementRepository.UpdateAgreement(agreement);
            _agreementRepository.CommitTransaction();

            return updatedAgreement;
        }

        public bool DeleteAgreement(long acquisitionFileId, long agreementId)
        {
            _user.ThrowIfNotAuthorized(Permissions.AgreementView);
            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, acquisitionFileId);

            var currentAcquisitionStatus = GetCurrentAcquisitionStatus(acquisitionFileId);
            var agreement = _agreementRepository.GetAgreementById(agreementId);

            ValidateAcquisitionFileStatus(acquisitionFileId, agreement.AgreementStatusTypeCode);

            bool deleteResult = _agreementRepository.TryDeleteAgreement(acquisitionFileId, agreementId);
            _agreementRepository.CommitTransaction();

            return deleteResult;
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

            var currentAcquisitionStatus = GetCurrentAcquisitionStatus(acquisitionFileId);
            if (!_statusSolver.CanEditStakeholders(currentAcquisitionStatus) && !_user.HasPermission(Permissions.SystemAdmin))
            {
                throw new BusinessRuleViolationException("The file you are editing is not active or draft, so you cannot save changes. Refresh your browser to see file state.");
            }

            var currentInterestHolders = _interestHolderRepository.GetInterestHoldersByAcquisitionFile(acquisitionFileId);

            // Verify that the interest holder is still the same (person or org)
            if (currentInterestHolders.Count > 0)
            {
                foreach (var newInterestHolder in interestHolders)
                {
                    if (newInterestHolder.InterestHolderId != 0)
                    {
                        PimsInterestHolder currentInterestHolder = currentInterestHolders
                            .FirstOrDefault(oldInth => oldInth.InterestHolderId == newInterestHolder.InterestHolderId);

                        // If the stakeholder has changed (person or org) it means it is actually a new stakeholder, so reset all the ids.
                        if (newInterestHolder.PersonId != currentInterestHolder.PersonId ||
                            newInterestHolder.OrganizationId != currentInterestHolder.OrganizationId)
                        {
                            newInterestHolder.InterestHolderId = 0;
                            newInterestHolder.PimsInthldrPropInterests.ForEach(pi => pi.PimsInthldrPropInterestId = 0);
                        }
                    }
                }
            }

            ValidateInterestHoldersDependency(acquisitionFileId, interestHolders);
            _interestHolderRepository.UpdateAllForAcquisition(acquisitionFileId, interestHolders);
            _interestHolderRepository.CommitTransaction();

            return _interestHolderRepository.GetInterestHoldersByAcquisitionFile(acquisitionFileId);
        }

        public IList<PimsCompensationRequisition> GetAcquisitionCompensations(long acquisitionFileId)
        {
            _logger.LogInformation("Getting compensations for acquisition file id: {acquisitionFileId}", acquisitionFileId);
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView, Permissions.AcquisitionFileView);
            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, acquisitionFileId);

            return _compensationRequisitionRepository.GetAllByAcquisitionFileId(acquisitionFileId);
        }

        public PimsCompensationRequisition AddCompensationRequisition(long acquisitionFileId, PimsCompensationRequisition compensationRequisition)
        {
            _logger.LogInformation("Adding compensation requisition for acquisition file id: {acquisitionFileId}", acquisitionFileId);

            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionAdd);
            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, acquisitionFileId);

            compensationRequisition.ThrowIfNull(nameof(compensationRequisition));

            var acquisitionFileParent = _acqFileRepository.GetById(acquisitionFileId);
            if (acquisitionFileId != compensationRequisition.AcquisitionFileId || acquisitionFileParent is null)
            {
                throw new BadRequestException("Invalid acquisitionFileId.");
            }

            compensationRequisition.IsDraft = compensationRequisition.IsDraft ?? true;

            var newCompensationRequisition = _compensationRequisitionRepository.Add(compensationRequisition);

            _compensationRequisitionRepository.CommitTransaction();

            return newCompensationRequisition;
        }

        public PimsExpropriationPayment AddExpropriationPayment(long acquisitionFileId, PimsExpropriationPayment expPayment)
        {
            _logger.LogInformation("Adding Expropiation Payment for acquisition file id: {acquisitionFileId}", acquisitionFileId);

            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileEdit);
            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, acquisitionFileId);

            expPayment.ThrowIfNull(nameof(expPayment));

            var acquisitionFileParent = _acqFileRepository.GetById(acquisitionFileId);
            if (acquisitionFileId != expPayment.AcquisitionFileId || acquisitionFileParent is null)
            {
                throw new BadRequestException("Invalid acquisitionFileId.");
            }

            var newForm8 = _expropriationPaymentRepository.Add(expPayment);
            _expropriationPaymentRepository.CommitTransaction();

            return newForm8;
        }

        public IList<PimsExpropriationPayment> GetAcquisitionExpropriationPayments(long acquisitionFileId)
        {

            _logger.LogInformation("Getting Expropiation Payments for acquisition file id: {acquisitionFileId}", acquisitionFileId);
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileView);
            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, acquisitionFileId);

            return _expropriationPaymentRepository.GetAllByAcquisitionFileId(acquisitionFileId);
        }

        private static void ValidateStaff(PimsAcquisitionFile pimsAcquisitionFile)
        {
            bool duplicate = pimsAcquisitionFile.PimsAcquisitionFileTeams.GroupBy(p => (p.AcqFlTeamProfileTypeCode, p.PersonId)).Any(g => g.Count() > 1);
            if (duplicate)
            {
                throw new BadRequestException("Invalid Acquisition team, each team member and role combination can only be added once.");
            }
        }

        private static void ValidateOrganizationStaff(PimsAcquisitionFile pimsAcquisitionFile)
        {
            bool duplicate = pimsAcquisitionFile.PimsAcquisitionFileTeams.GroupBy(p => (p.AcqFlTeamProfileTypeCode, p.OrganizationId)).Any(g => g.Count() > 1);
            if (duplicate)
            {
                throw new BadRequestException("Invalid Acquisition team, each team member and role combination can only be added once.");
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
                        var foundProperty = _propertyRepository.GetByPid(pid, true);
                        if (foundProperty.IsRetired.HasValue && foundProperty.IsRetired.Value)
                        {
                            throw new BusinessRuleViolationException("Retired property can not be selected.");
                        }

                        acquisitionProperty.PropertyId = foundProperty.Internal_Id;
                        _propertyService.UpdateLocation(acquisitionProperty.Property, ref foundProperty, userOverrideCodes);
                        acquisitionProperty.Property = foundProperty;
                    }
                    catch (KeyNotFoundException)
                    {
                        _logger.LogDebug("Adding new property with pid:{pid}", pid);
                        acquisitionProperty.Property = _propertyService.PopulateNewProperty(acquisitionProperty.Property);
                    }
                }
                else if (acquisitionProperty.Property.Pin.HasValue)
                {
                    var pin = acquisitionProperty.Property.Pin.Value;
                    try
                    {
                        var foundProperty = _propertyRepository.GetByPin(pin, true);
                        if (foundProperty.IsRetired.HasValue && foundProperty.IsRetired.Value)
                        {
                            throw new BusinessRuleViolationException("Retired property can not be selected.");
                        }

                        acquisitionProperty.PropertyId = foundProperty.Internal_Id;
                        _propertyService.UpdateLocation(acquisitionProperty.Property, ref foundProperty, userOverrideCodes);
                        acquisitionProperty.Property = foundProperty;
                    }
                    catch (KeyNotFoundException)
                    {
                        _logger.LogDebug("Adding new property with pin:{pin}", pin);
                        acquisitionProperty.Property = _propertyService.PopulateNewProperty(acquisitionProperty.Property);
                    }
                }
                else
                {
                    _logger.LogDebug("Adding new property without a pid");
                    acquisitionProperty.Property = _propertyService.PopulateNewProperty(acquisitionProperty.Property);
                }
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
            if (!newAllowableCompensation.HasValue)
            {
                return;
            }
            IEnumerable<PimsCompReqFinancial> allFinalFinancialsOnFile = _compReqFinancialService.GetAllByAcquisitionFileId(currentAcquisitionFileId, true);
            var currentActualCompensation = allFinalFinancialsOnFile.Aggregate(0m, (acc, f) => acc + (f.TotalAmt ?? 0m));
            if (newAllowableCompensation < currentActualCompensation)
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
                throw new UserOverrideException(UserOverrideCode.UpdateRegion, "The selected Ministry region is different from that associated to one or more selected properties\n\nDo you want to proceed?");
            }
        }

        private void ValidateDraftsOnComplete(PimsAcquisitionFile incomingFile)
        {
            var agreements = _agreementRepository.GetAgreementsByAcquisitionFile(incomingFile.AcquisitionFileId);
            var compensations = _compensationRequisitionRepository.GetAllByAcquisitionFileId(incomingFile.AcquisitionFileId);
            if (agreements.Any(a => a?.AgreementStatusTypeCode == AgreementStatusTypes.DRAFT.ToString()) || compensations.Any(c => c.IsDraft.HasValue && c.IsDraft.Value))
            {
                throw new BusinessRuleViolationException("You cannot complete a file when there are one or more draft agreements, or one or more draft compensations requisitions." +
                    "\n\nRemove any draft compensations requisitions. Agreements should be set to final, cancelled, or removed.");
            }

            var takes = _takeRepository.GetAllByAcquisitionFileId(incomingFile.AcquisitionFileId);

            if (!takes.Any())
            {
                throw new BusinessRuleViolationException("You cannot complete an acquisition file that has no takes.");
            }

            if (takes.Any(t => t.TakeStatusTypeCode == AcquisitionTakeStatusTypes.INPROGRESS.ToString()))
            {
                throw new BusinessRuleViolationException("Please ensure all in-progress property takes have been completed or canceled before completing an Acquisition File.");
            }
        }

        private void ValidateAcquisitionFileStatus(long acquisitionFileId, string agreementCurrentStatusTypeCode)
        {
            var currentAcquisitionStatus = GetCurrentAcquisitionStatus(acquisitionFileId);
            var agreementStatus = Enum.Parse<AgreementStatusTypes>(agreementCurrentStatusTypeCode);

            if (!_statusSolver.CanEditOrDeleteAgreement(currentAcquisitionStatus, agreementStatus) && !_user.HasPermission(Permissions.SystemAdmin))
            {
                throw new BusinessRuleViolationException("The file you are editing is not active or draft, so you cannot save changes. Refresh your browser to see file state.");
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
            if (doNotAddToStatuses.Contains(acquisitionFile.AcquisitionFileStatusTypeCode))
            {
                return;
            }
            var checklistStatusTypes = _lookupRepository.GetAllAcquisitionChecklistItemStatusTypes();
            foreach (var itemType in _checklistRepository.GetAllChecklistItemTypes().Where(x => !x.IsExpiredType()))
            {
                if (!pimsAcquisitionChecklistItems.Any(cli => cli.AcqChklstItemTypeCode == itemType.AcqChklstItemTypeCode) && DateOnly.FromDateTime(acquisitionFile.AppCreateTimestamp) >= itemType.EffectiveDate)
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
            var currentAcquisitionFile = _acqFileRepository.GetById(acquisitionFile.Internal_Id);
            var compensationRequisitions = _compensationRequisitionRepository.GetAllByAcquisitionFileId(acquisitionFile.Internal_Id);

            if (compensationRequisitions.Count == 0)
            {
                return;
            }

            foreach (var compReq in compensationRequisitions)
            {
                // Check for Acquisition File Owner removed
                if (compReq.AcquisitionOwnerId is not null
                    && !acquisitionFile.PimsAcquisitionOwners.Any(x => x.Internal_Id.Equals(compReq.AcquisitionOwnerId))
                    && currentAcquisitionFile.PimsAcquisitionOwners.Any(x => x.Internal_Id.Equals(compReq.AcquisitionOwnerId)))
                {
                    throw new ForeignKeyDependencyException("Acquisition File Owner can not be removed since it's assigned as a payee for a compensation requisition");
                }

                // Check for Acquisition InterestHolders
                if (compReq.InterestHolderId is not null
                    && !acquisitionFile.PimsInterestHolders.Any(x => x.Internal_Id.Equals(compReq.InterestHolderId))
                    && currentAcquisitionFile.PimsInterestHolders.Any(x => x.Internal_Id.Equals(compReq.InterestHolderId)))
                {
                    throw new ForeignKeyDependencyException("Acquisition File Interest Holders can not be removed since it's assigned as a payee for a compensation requisition");
                }

                // Check for File Person
                if (compReq.AcquisitionFileTeamId is not null
                    && !acquisitionFile.PimsAcquisitionFileTeams.Any(x => x.Internal_Id.Equals(compReq.AcquisitionFileTeamId))
                    && currentAcquisitionFile.PimsAcquisitionFileTeams.Any(x => x.Internal_Id.Equals(compReq.AcquisitionFileTeamId)))
                {
                    throw new ForeignKeyDependencyException("Acquisition File team member can not be removed since it's assigned as a payee for a compensation requisition");
                }
            }
        }

        private void ValidateInterestHoldersDependency(long acquisitionFileId, List<PimsInterestHolder> interestHolders)
        {
            var currentAcquisitionFile = _acqFileRepository.GetById(acquisitionFileId);
            var compensationRequisitions = _compensationRequisitionRepository.GetAllByAcquisitionFileId(acquisitionFileId);

            if (compensationRequisitions.Count == 0)
            {
                return;
            }

            foreach (var compReq in compensationRequisitions)
            {
                // Check for Interest Holder
                if (compReq.InterestHolderId is not null
                    && !interestHolders.Any(x => x.InterestHolderId.Equals(compReq.InterestHolderId))
                    && currentAcquisitionFile.PimsInterestHolders.Any(x => x.Internal_Id.Equals(compReq.InterestHolderId)))
                {
                    throw new ForeignKeyDependencyException("Acquisition File Interest Holder can not be removed since it's assigned as a payee for a compensation requisition");
                }
            }
        }

        private void ValidatePropertyRegions(PimsAcquisitionFile acquisitionFile)
        {
            var userRegions = _user.GetUserRegions(_userRepository);
            foreach (var acquisitionProperty in acquisitionFile.PimsPropertyAcquisitionFiles)
            {
                var propertyRegion = acquisitionProperty.Property?.RegionCode ?? _propertyRepository.GetPropertyRegion(acquisitionProperty.PropertyId);
                if (!userRegions.Contains(propertyRegion))
                {
                    throw new BadRequestException("You cannot add a property that is outside of your user account region(s).\n\nPlease select a different property or contact admin at pims@gov.bc.ca to add the required region to your user account settings.");
                }
            }
        }

        private AcquisitionStatusTypes? GetCurrentAcquisitionStatus(long acquisitionFileId)
        {
            var currentAcquisitionFile = _acqFileRepository.GetById(acquisitionFileId);
            AcquisitionStatusTypes currentAcquisitionStatus;

            if (Enum.TryParse(currentAcquisitionFile.AcquisitionFileStatusTypeCode, out currentAcquisitionStatus))
            {
                return currentAcquisitionStatus;
            }

            return currentAcquisitionStatus;
        }
    }
}
