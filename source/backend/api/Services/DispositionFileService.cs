using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Api.Constants;
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
    public class DispositionFileService : IDispositionFileService
    {
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly IUserRepository _userRepository;
        private readonly IDispositionFileRepository _dispositionFileRepository;
        private readonly IDispositionFilePropertyRepository _dispositionFilePropertyRepository;
        private readonly ICoordinateTransformService _coordinateService;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyService _propertyService;
        private readonly ILookupRepository _lookupRepository;
        private readonly IDispositionFileChecklistRepository _checklistRepository;
        private readonly IEntityNoteRepository _entityNoteRepository;
        private readonly IDispositionStatusSolver _dispositionStatusSolver;

        public DispositionFileService(
            ClaimsPrincipal user,
            ILogger<DispositionFileService> logger,
            IDispositionFileRepository dispositionFileRepository,
            IDispositionFilePropertyRepository dispositionFilePropertyRepository,
            ICoordinateTransformService coordinateService,
            IPropertyRepository propertyRepository,
            IPropertyService propertyService,
            ILookupRepository lookupRepository,
            IDispositionFileChecklistRepository checklistRepository,
            IEntityNoteRepository entityNoteRepository,
            IUserRepository userRepository,
            IDispositionStatusSolver dispositionStatusSolver)
        {
            _user = user;
            _logger = logger;
            _dispositionFileRepository = dispositionFileRepository;
            _dispositionFilePropertyRepository = dispositionFilePropertyRepository;
            _coordinateService = coordinateService;
            _propertyRepository = propertyRepository;
            _propertyService = propertyService;
            _lookupRepository = lookupRepository;
            _checklistRepository = checklistRepository;
            _entityNoteRepository = entityNoteRepository;
            _userRepository = userRepository;
            _dispositionStatusSolver = dispositionStatusSolver;
        }

        public PimsDispositionFile Add(PimsDispositionFile dispositionFile, IEnumerable<UserOverrideCode> userOverrides)
        {
            _logger.LogInformation("Creating Disposition File {dispositionFile}", dispositionFile);
            _user.ThrowIfNotAuthorized(Permissions.DispositionAdd);
            dispositionFile.ThrowMissingContractorInTeam(_user, _userRepository);

            dispositionFile.DispositionStatusTypeCode ??= EnumDispositionStatusTypeCode.UNKNOWN.ToString();
            dispositionFile.DispositionFileStatusTypeCode ??= EnumDispositionFileStatusTypeCode.ACTIVE.ToString();
            ValidateStaff(dispositionFile);

            MatchProperties(dispositionFile, userOverrides);
            ValidatePropertyRegions(dispositionFile);

            var newDispositionFile = _dispositionFileRepository.Add(dispositionFile);
            _dispositionFileRepository.CommitTransaction();

            return newDispositionFile;
        }

        public PimsDispositionFile GetById(long id)
        {
            _logger.LogInformation("Getting disposition file with id {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.DispositionView);
            _user.ThrowInvalidAccessToDispositionFile(_userRepository, _dispositionFileRepository, id);

            var dispositionFile = _dispositionFileRepository.GetById(id);

            return dispositionFile;
        }

        public PimsDispositionFile Update(long id, PimsDispositionFile dispositionFile, IEnumerable<UserOverrideCode> userOverrides)
        {
            dispositionFile.ThrowIfNull(nameof(dispositionFile));

            _logger.LogInformation("Updating disposition file with id {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.DispositionEdit);
            _user.ThrowInvalidAccessToDispositionFile(_userRepository, _dispositionFileRepository, id);

            if (id != dispositionFile.DispositionFileId)
            {
                throw new BadRequestException("Invalid dispositionFileId.");
            }

            DispositionStatusTypes? currentDispositionStatus = GetCurrentDispositionStatus(dispositionFile.Internal_Id);
            if (!_dispositionStatusSolver.CanEditDetails(currentDispositionStatus) && !_user.HasPermission(Permissions.SystemAdmin))
            {
                throw new BusinessRuleViolationException("The file you are editing is not active or draft, so you cannot save changes. Refresh your browser to see file state.");
            }

            ValidateVersion(id, dispositionFile.ConcurrencyControlNumber);

            // validate disposition file state before proceeding with any database updates
            var currentDispositionFile = _dispositionFileRepository.GetById(id);
            ValidateFileBeforeUpdate(dispositionFile, currentDispositionFile, userOverrides);

            var isFileClosing = currentDispositionFile.DispositionFileStatusTypeCode != EnumDispositionFileStatusTypeCode.COMPLETE.ToString() &&
                                dispositionFile.DispositionFileStatusTypeCode == EnumDispositionFileStatusTypeCode.COMPLETE.ToString();

            if (isFileClosing && currentDispositionFile.PimsDispositionFileProperties?.Count > 0)
            {
                DisposeOfProperties(dispositionFile);
            }

            _dispositionFileRepository.Update(id, dispositionFile);
            AddNoteIfStatusChanged(dispositionFile);
            _dispositionFileRepository.CommitTransaction();

            return _dispositionFileRepository.GetById(id);
        }

        public LastUpdatedByModel GetLastUpdateInformation(long dispositionFileId)
        {
            _logger.LogInformation("Retrieving last updated-by information...");
            _user.ThrowIfNotAuthorized(Permissions.DispositionView);

            return _dispositionFileRepository.GetLastUpdateBy(dispositionFileId);
        }

        public Paged<PimsDispositionFile> GetPage(DispositionFilter filter)
        {
            _logger.LogInformation("Searching for disposition files...");
            _logger.LogDebug("Disposition file search with filter: {filter}", filter);
            _user.ThrowIfNotAuthorized(Permissions.DispositionView);

            var pimsUser = _userRepository.GetUserInfoByKeycloakUserId(_user.GetUserKey());
            long? contractorPersonId = (pimsUser != null && pimsUser.IsContractor) ? pimsUser.PersonId : null;

            return _dispositionFileRepository.GetPageDeep(filter, contractorPersonId);
        }

        public IEnumerable<PimsDispositionFileProperty> GetProperties(long id)
        {
            _logger.LogInformation("Getting disposition file properties with id {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.DispositionView);
            _user.ThrowIfNotAuthorized(Permissions.PropertyView);

            var properties = _dispositionFilePropertyRepository.GetPropertiesByDispositionFileId(id);
            ReprojectPropertyLocationsToWgs84(properties);
            return properties;
        }

        public IEnumerable<PimsDispositionFileTeam> GetTeamMembers()
        {
            _logger.LogInformation("Getting disposition team members");
            _user.ThrowIfNotAuthorized(Permissions.DispositionView);
            _user.ThrowIfNotAuthorized(Permissions.ContactView);

            var teamMembers = _dispositionFileRepository.GetTeamMembers();

            var persons = teamMembers.Where(x => x.Person != null).GroupBy(x => x.PersonId).Select(x => x.First());
            var organizations = teamMembers.Where(x => x.Organization != null).GroupBy(x => x.OrganizationId).Select(x => x.First());

            List<PimsDispositionFileTeam> teamFilterOptions = new();
            teamFilterOptions.AddRange(persons);
            teamFilterOptions.AddRange(organizations);

            return teamFilterOptions;
        }

        public IEnumerable<PimsDispositionOffer> GetOffers(long dispositionFileId)
        {
            _logger.LogInformation("Getting disposition file offers with DispositionFileId: {id}", dispositionFileId);
            _user.ThrowIfNotAuthorized(Permissions.DispositionView);

            return _dispositionFileRepository.GetDispositionOffers(dispositionFileId);
        }

        public PimsDispositionOffer GetDispositionOfferById(long dispositionFileId, long dispositionOfferId)
        {
            _logger.LogInformation("Getting disposition file offers with Id: {id}", dispositionOfferId);
            _user.ThrowIfNotAuthorized(Permissions.DispositionView);

            var dispositionFileParent = _dispositionFileRepository.GetById(dispositionFileId);
            if (dispositionFileParent is null
                || (dispositionFileParent is not null && !dispositionFileParent.PimsDispositionOffers.Any(x => x.DispositionOfferId.Equals(dispositionOfferId))))
            {
                throw new BadRequestException("Invalid dispositionFileId.");
            }

            return _dispositionFileRepository.GetDispositionOfferById(dispositionFileId, dispositionOfferId);
        }

        public PimsDispositionOffer AddDispositionFileOffer(long dispositionFileId, PimsDispositionOffer dispositionOffer)
        {
            _logger.LogInformation("Adding disposition file offer to Disposition File with Id: {id}", dispositionFileId);
            _user.ThrowIfNotAuthorized(Permissions.DispositionEdit);

            var dispositionFileParent = _dispositionFileRepository.GetById(dispositionFileId);
            if (dispositionFileId != dispositionOffer.DispositionFileId || dispositionFileParent is null)
            {
                throw new BadRequestException("Invalid dispositionFileId.");
            }

            DispositionStatusTypes? currentDispositionStatus = GetCurrentDispositionStatus(dispositionFileParent.Internal_Id);
            if (!_dispositionStatusSolver.CanEditOrDeleteValuesOffersSales(currentDispositionStatus) && !_user.HasPermission(Permissions.SystemAdmin))
            {
                throw new BusinessRuleViolationException("The file you are editing is not active or draft, so you cannot save changes. Refresh your browser to see file state.");
            }

            ValidateDispositionOfferStatus(dispositionFileParent, dispositionOffer);

            var newOffer = _dispositionFileRepository.AddDispositionOffer(dispositionOffer);
            _dispositionFileRepository.CommitTransaction();

            return newOffer;
        }

        public PimsDispositionOffer UpdateDispositionFileOffer(long dispositionFileId, long offerId, PimsDispositionOffer dispositionOffer)
        {
            _logger.LogInformation("Getting disposition file offers with DispositionFileId: {id}", dispositionFileId);
            _user.ThrowIfNotAuthorized(Permissions.DispositionEdit);

            var dispositionFileParent = _dispositionFileRepository.GetById(dispositionFileId);
            if (dispositionFileId != dispositionOffer.DispositionFileId || dispositionOffer.DispositionOfferId != offerId || dispositionFileParent is null)
            {
                throw new BadRequestException("Invalid dispositionFileId.");
            }

            DispositionStatusTypes? currentDispositionStatus = GetCurrentDispositionStatus(dispositionFileParent.Internal_Id);
            if (!_dispositionStatusSolver.CanEditOrDeleteValuesOffersSales(currentDispositionStatus) && !_user.HasPermission(Permissions.SystemAdmin))
            {
                throw new BusinessRuleViolationException("The file you are editing is not active or draft, so you cannot save changes. Refresh your browser to see file state.");
            }

            ValidateDispositionOfferStatus(dispositionFileParent, dispositionOffer);

            var updatedOffer = _dispositionFileRepository.UpdateDispositionOffer(dispositionOffer);
            _dispositionFileRepository.CommitTransaction();

            return updatedOffer;
        }

        public bool DeleteDispositionFileOffer(long dispositionFileId, long offerId)
        {
            _logger.LogInformation("Deleting Disposition Offer with id: {offerId}", offerId);
            _user.ThrowIfNotAuthorized(Permissions.DispositionEdit);

            var dispositionFile = _dispositionFileRepository.GetById(dispositionFileId);
            DispositionStatusTypes? currentDispositionStatus = GetCurrentDispositionStatus(dispositionFile.Internal_Id);
            if (!_dispositionStatusSolver.CanEditOrDeleteValuesOffersSales(currentDispositionStatus) && !_user.HasPermission(Permissions.SystemAdmin))
            {
                throw new BusinessRuleViolationException("The file you are editing is not active or draft, so you cannot save changes. Refresh your browser to see file state.");
            }

            var deleteResult = _dispositionFileRepository.TryDeleteDispositionOffer(dispositionFileId, offerId);
            _dispositionFileRepository.CommitTransaction();

            return deleteResult;
        }

        public PimsDispositionSale GetDispositionFileSale(long dispositionFileId)
        {
            _logger.LogInformation("Getting disposition file sales with DispositionFileId: {id}", dispositionFileId);
            _user.ThrowIfNotAuthorized(Permissions.DispositionView);

            return _dispositionFileRepository.GetDispositionFileSale(dispositionFileId);
        }

        public PimsDispositionSale UpdateDispositionFileSale(PimsDispositionSale dispositionSale)
        {
            _logger.LogInformation("Updating disposition file Sale with DispositionFileId: {id}", dispositionSale.DispositionSaleId);
            _user.ThrowIfNotAuthorized(Permissions.DispositionEdit);

            DispositionStatusTypes? currentDispositionStatus = GetCurrentDispositionStatus(dispositionSale.DispositionFileId);
            if (!_dispositionStatusSolver.CanEditOrDeleteValuesOffersSales(currentDispositionStatus) && !_user.HasPermission(Permissions.SystemAdmin))
            {
                throw new BusinessRuleViolationException("The file you are editing is not active or draft, so you cannot save changes. Refresh your browser to see file state.");
            }

            var updatedSale = _dispositionFileRepository.UpdateDispositionFileSale(dispositionSale);

            _dispositionFileRepository.CommitTransaction();

            return updatedSale;
        }

        public PimsDispositionSale AddDispositionFileSale(PimsDispositionSale dispositionSale)
        {
            _logger.LogInformation("Adding disposition file Sale to Disposition File with Id: {id}", dispositionSale.DispositionFileId);
            _user.ThrowIfNotAuthorized(Permissions.DispositionEdit);

            DispositionStatusTypes? currentDispositionStatus = GetCurrentDispositionStatus(dispositionSale.DispositionFileId);
            if (!_dispositionStatusSolver.CanEditOrDeleteValuesOffersSales(currentDispositionStatus) && !_user.HasPermission(Permissions.SystemAdmin))
            {
                throw new BusinessRuleViolationException("The file you are editing is not active or draft, so you cannot save changes. Refresh your browser to see file state.");
            }

            var dispositionFileParent = _dispositionFileRepository.GetById(dispositionSale.DispositionFileId);
            if (dispositionFileParent.PimsDispositionSales.Count > 0)
            {
                throw new DuplicateEntityException("Invalid Disposition Sale. A Sale has been already created for this Disposition File");
            }

            _dispositionFileRepository.AddDispositionFileSale(dispositionSale);
            _dispositionFileRepository.CommitTransaction();

            return dispositionSale;
        }

        public PimsDispositionAppraisal GetDispositionFileAppraisal(long dispositionFileId)
        {
            _logger.LogInformation("Getting disposition file appraisal with DispositionFileId: {id}", dispositionFileId);
            _user.ThrowIfNotAuthorized(Permissions.DispositionView);

            return _dispositionFileRepository.GetDispositionFileAppraisal(dispositionFileId);
        }

        public PimsDispositionAppraisal AddDispositionFileAppraisal(long dispositionFileId, PimsDispositionAppraisal dispositionAppraisal)
        {
            _logger.LogInformation("Adding disposition file offer to Disposition File with Id: {id}", dispositionFileId);
            _user.ThrowIfNotAuthorized(Permissions.DispositionEdit);

            var dispositionFileParent = _dispositionFileRepository.GetById(dispositionFileId);
            if (dispositionFileId != dispositionAppraisal.DispositionFileId || dispositionFileParent is null)
            {
                throw new BadRequestException("Invalid dispositionFileId.");
            }

            if (dispositionFileParent.PimsDispositionAppraisals.Count > 0)
            {
                throw new DuplicateEntityException("Invalid Disposition Appraisal. An Appraisal has been already created for this Disposition File");
            }

            DispositionStatusTypes? currentDispositionStatus = GetCurrentDispositionStatus(dispositionFileParent.Internal_Id);
            if (!_dispositionStatusSolver.CanEditOrDeleteValuesOffersSales(currentDispositionStatus) && !_user.HasPermission(Permissions.SystemAdmin))
            {
                throw new BusinessRuleViolationException("The file you are editing is not active or draft, so you cannot save changes. Refresh your browser to see file state.");
            }

            var newAppraisal = _dispositionFileRepository.AddDispositionFileAppraisal(dispositionAppraisal);
            _dispositionFileRepository.CommitTransaction();

            return newAppraisal;
        }

        public PimsDispositionAppraisal UpdateDispositionFileAppraisal(long dispositionFileId, long appraisalId, PimsDispositionAppraisal dispositionAppraisal)
        {
            _logger.LogInformation("Updating disposition file Appraisal with DispositionFileId: {id}", dispositionFileId);
            _user.ThrowIfNotAuthorized(Permissions.DispositionEdit);

            var dispositionFileParent = _dispositionFileRepository.GetById(dispositionFileId);
            if (dispositionFileId != dispositionAppraisal.DispositionFileId || dispositionAppraisal.DispositionAppraisalId != appraisalId || dispositionFileParent is null)
            {
                throw new BadRequestException("Invalid dispositionFileId.");
            }

            DispositionStatusTypes? currentDispositionStatus = GetCurrentDispositionStatus(dispositionFileParent.Internal_Id);
            if (!_dispositionStatusSolver.CanEditOrDeleteValuesOffersSales(currentDispositionStatus) && !_user.HasPermission(Permissions.SystemAdmin))
            {
                throw new BusinessRuleViolationException("The file you are editing is not active or draft, so you cannot save changes. Refresh your browser to see file state.");
            }

            var updatedAppraisal = _dispositionFileRepository.UpdateDispositionFileAppraisal(appraisalId, dispositionAppraisal);
            _dispositionFileRepository.CommitTransaction();

            return updatedAppraisal;
        }

        public IEnumerable<PimsDispositionChecklistItem> GetChecklistItems(long id)
        {
            _logger.LogInformation("Getting disposition file checklist with DispositionFile id: {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.DispositionView);

            var checklistItems = _checklistRepository.GetAllChecklistItemsByDispositionFileId(id);
            var dispositionFile = _dispositionFileRepository.GetById(id);
            AppendToDispositionChecklist(dispositionFile, ref checklistItems);

            return checklistItems;
        }

        public PimsDispositionFile UpdateChecklistItems(IList<PimsDispositionChecklistItem> checklistItems)
        {
            checklistItems.ThrowIfNull(nameof(checklistItems));
            if (checklistItems.Count == 0)
            {
                throw new BadRequestException("Checklist items must be greater than zero");
            }

            var dispositionFileId = checklistItems.FirstOrDefault().DispositionFileId;
            _logger.LogInformation("Updating disposition file checklist with DispositionFile id: {id}", dispositionFileId);
            _user.ThrowIfNotAuthorized(Permissions.DispositionEdit);

            // Get the current checklist items for this disposition file.
            var currentItems = _checklistRepository.GetAllChecklistItemsByDispositionFileId(dispositionFileId).ToDictionary(ci => ci.Internal_Id);

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
                else if (existingItem.DspChklstItemStatusTypeCode != incomingItem.DspChklstItemStatusTypeCode)
                {
                    _checklistRepository.Update(incomingItem);
                }
            }

            _checklistRepository.CommitTransaction();
            return _dispositionFileRepository.GetById(dispositionFileId);
        }

        public List<DispositionFileExportModel> GetDispositionFileExport(DispositionFilter filter)
        {
            _logger.LogInformation("Searching all Disposition Files matching the filter: {filter}", filter);
            _user.ThrowIfNotAuthorized(Permissions.DispositionView);

            var dispositionFiles = _dispositionFileRepository.GetDispositionFileExportDeep(filter);

            return dispositionFiles
                .Select(file => new DispositionFileExportModel
                {
                    FileNumber = file.FileNumber != null ? $"D-{file.FileNumber}" : string.Empty,
                    ReferenceNumber = file.FileReference ?? string.Empty,
                    FileName = file.FileName ?? string.Empty,
                    DispositionType = file.DispositionTypeCodeNavigation?.Description ?? string.Empty,
                    MotiRegion = file.RegionCodeNavigation?.Description ?? string.Empty,
                    TeamMembers = string.Join("|", file.PimsDispositionFileTeams.Select(x => (x.PersonId.HasValue ? x.Person.GetFullName(true) + $" ({x.DspFlTeamProfileTypeCodeNavigation?.Description})" : x.Organization.Name + $" (Role: {x.DspFlTeamProfileTypeCodeNavigation?.Description}, Primary: {x.PrimaryContact?.GetFullName(true) ?? "N/A"})")) ?? Array.Empty<string>()),
                    CivicAddress = string.Join("|", file.PimsDispositionFileProperties.Select(x => x.Property?.Address?.FormatFullAddressString()).Where(x => x != null)),
                    Pid = string.Join("|", file.PimsDispositionFileProperties.Select(x => x.Property?.Pid).Where(x => x != null)),
                    Pin = string.Join("|", file.PimsDispositionFileProperties.Select(x => x.Property?.Pin).Where(x => x != null)),
                    GeneralLocation = string.Join("|", file.PimsDispositionFileProperties.Select(x => x.Property?.GeneralLocation).Where(x => x != null)),
                    DispositionStatusTypeCode = file.DispositionStatusTypeCodeNavigation?.Description ?? string.Empty,
                    DispositionFileStatusTypeCode = file.DispositionFileStatusTypeCodeNavigation?.Description ?? string.Empty,
                    FileFunding = file.DispositionFundingTypeCodeNavigation is not null ? file.DispositionFundingTypeCodeNavigation.Description : string.Empty,
                    FileAssignedDate = file.AssignedDt.HasValue ? file.AssignedDt.Value.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture) : string.Empty,
                    DispositionCompleted = file.CompletedDt.HasValue ? file.CompletedDt.Value.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture) : string.Empty,
                    InitiatingDocument = file.DispositionInitiatingDocTypeCode == "OTHER" ? $"Other - {file.OtherInitiatingDocType ?? string.Empty}" : file.DispositionInitiatingDocTypeCodeNavigation?.Description ?? string.Empty,
                    InitiatingDocumentDate = file.InitiatingDocumentDt.HasValue ? file.InitiatingDocumentDt.Value.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture) : string.Empty,
                    PhysicalFileStatus = file.DspPhysFileStatusTypeCodeNavigation?.Description ?? string.Empty,
                    AppraisalValue = file.PimsDispositionAppraisals?.FirstOrDefault()?.AppraisedAmt ?? 0,
                    AppraisalDate = file.PimsDispositionAppraisals?.FirstOrDefault()?.AppraisalDt != null ? file.PimsDispositionAppraisals?.FirstOrDefault()?.AppraisalDt.Value.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture) : string.Empty,
                    AssessmentValue = file.PimsDispositionAppraisals?.FirstOrDefault()?.BcaValueAmt ?? 0,
                    RollYear = file.PimsDispositionAppraisals?.FirstOrDefault()?.BcaRollYear?.ToString() ?? string.Empty,
                    ListPrice = file.PimsDispositionAppraisals?.FirstOrDefault()?.ListPriceAmt ?? 0,
                    PurchaserNames = string.Join("|", file.PimsDispositionSales?.FirstOrDefault()?.PimsDispositionPurchasers.Select(x => (x.PersonId.HasValue ? x.Person.GetFullName(true) : x.Organization.Name + $" (Primary: {x.PrimaryContact?.GetFullName(true) ?? "N/A"})")) ?? Array.Empty<string>()),
                    SaleCompletionDate = file.PimsDispositionSales?.FirstOrDefault()?.SaleCompletionDt != null ? file.PimsDispositionSales?.FirstOrDefault()?.SaleCompletionDt.Value.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture) : string.Empty,
                    FiscalYearOfSale = file.PimsDispositionSales?.FirstOrDefault()?.SaleFiscalYear?.ToString() ?? string.Empty,
                    FinalSalePrice = file.PimsDispositionSales?.FirstOrDefault()?.SaleFinalAmt ?? 0,
                    RealtorCommission = file.PimsDispositionSales?.FirstOrDefault()?.RealtorCommissionAmt ?? 0,
                    GstCollected = file.PimsDispositionSales?.FirstOrDefault()?.GstCollectedAmt ?? 0,
                    NetBookValue = file.PimsDispositionSales?.FirstOrDefault()?.NetBookAmt ?? 0,
                    TotalCostOfSale = file.PimsDispositionSales?.FirstOrDefault()?.TotalCostAmt ?? 0,
                    SppAmount = file.PimsDispositionSales?.FirstOrDefault()?.SppAmt ?? 0,
                    RemediationCost = file.PimsDispositionSales?.FirstOrDefault()?.RemediationAmt ?? 0,
                    NetBeforeSpp = CalculateNetProceedsBeforeSpp(file.PimsDispositionSales?.FirstOrDefault()),
                    NetAfterSpp = CalculateNetProceedsAfterSpp(file.PimsDispositionSales?.FirstOrDefault()),
                    Project = file?.Project?.Description is not null ? $"{file.Project.Code} {file.Project.Description}" : string.Empty,
                }).ToList();
        }

        public PimsDispositionFile UpdateProperties(PimsDispositionFile dispositionFile, IEnumerable<UserOverrideCode> userOverrides)
        {
            _logger.LogInformation("Updating disposition file properties...");
            _user.ThrowIfNotAllAuthorized(Permissions.DispositionEdit, Permissions.PropertyView, Permissions.PropertyAdd);
            _user.ThrowInvalidAccessToDispositionFile(_userRepository, _dispositionFileRepository, dispositionFile.Internal_Id);

            ValidateVersion(dispositionFile.Internal_Id, dispositionFile.ConcurrencyControlNumber);

            MatchProperties(dispositionFile, userOverrides);

            ValidatePropertyRegions(dispositionFile);

            // Get the current properties in the research file
            var currentProperties = _dispositionFilePropertyRepository.GetPropertiesByDispositionFileId(dispositionFile.Internal_Id);

            // Check if the property is new or if it is being updated
            foreach (var incomingDispositionProperty in dispositionFile.PimsDispositionFileProperties)
            {
                // If the property is not new, check if the name has been updated.
                if (incomingDispositionProperty.Internal_Id != 0)
                {
                    PimsDispositionFileProperty existingProperty = currentProperties.FirstOrDefault(x => x.Internal_Id == incomingDispositionProperty.Internal_Id);
                    if (existingProperty.PropertyName != incomingDispositionProperty.PropertyName)
                    {
                        existingProperty.PropertyName = incomingDispositionProperty.PropertyName;
                        _dispositionFilePropertyRepository.Update(existingProperty);
                    }
                }
                else
                {
                    // New property needs to be added
                    _dispositionFilePropertyRepository.Add(incomingDispositionProperty);
                }
            }

            // The ones not on the new set should be deleted
            List<PimsDispositionFileProperty> differenceSet = currentProperties.Where(x => !dispositionFile.PimsDispositionFileProperties.Any(y => y.Internal_Id == x.Internal_Id)).ToList();
            foreach (var deletedProperty in differenceSet)
            {
                _dispositionFilePropertyRepository.Delete(deletedProperty);

                var totalAssociationCount = _propertyRepository.GetAllAssociationsCountById(deletedProperty.PropertyId);
                if (totalAssociationCount <= 1)
                {
                    _dispositionFileRepository.CommitTransaction(); // TODO: this can only be removed if cascade deletes are implemented. EF executes deletes in alphabetic order.
                    _propertyRepository.Delete(deletedProperty.Property);
                }
            }

            _dispositionFileRepository.CommitTransaction();
            return _dispositionFileRepository.GetById(dispositionFile.Internal_Id);
        }

        private void ValidateFileBeforeUpdate(PimsDispositionFile incomingDispositionFile, PimsDispositionFile currentDispositionFile, IEnumerable<UserOverrideCode> userOverrides)
        {
            // Implement file validation logic before proceeding to update. This includes file closing validation.
            // The order of validation checks is important as it has been requested by business users.
            var isFileClosing = currentDispositionFile.DispositionFileStatusTypeCode != EnumDispositionFileStatusTypeCode.COMPLETE.ToString() &&
                                incomingDispositionFile.DispositionFileStatusTypeCode == EnumDispositionFileStatusTypeCode.COMPLETE.ToString();

            var currentProperties = _dispositionFilePropertyRepository.GetPropertiesByDispositionFileId(incomingDispositionFile.Internal_Id);

            // The following checks result in hard STOP errors
            if (isFileClosing)
            {
                if (currentProperties.Any(p => !p.Property.IsOwned))
                {
                    throw new BusinessRuleViolationException("You have one or more properties attached to this Disposition file that is NOT in the \"Core Inventory\" (i.e. owned by BCTFA and/or HMK). To complete this file you must either, remove these non \"Non-Core Inventory\" properties, OR make sure the property is added to the PIMS inventory first.");
                }

                if (currentDispositionFile.PimsDispositionSales?.FirstOrDefault()?.SaleFinalAmt == null)
                {
                    throw new BusinessRuleViolationException("You have not added a Sales Price. Please add a Sales Price before completion.");
                }
            }

            ValidateStaff(incomingDispositionFile);
            incomingDispositionFile.ThrowContractorRemovedFromTeam(_user, _userRepository);

            // From here on - these checks result in warnings that require user confirmation
            if (!userOverrides.Contains(UserOverrideCode.UpdateRegion))
            {
                // confirm user action - file region was changed
                ValidateMinistryRegion(incomingDispositionFile.Internal_Id, incomingDispositionFile.RegionCode);
            }

            var nonEditableStatuses = new List<string>() { EnumDispositionFileStatusTypeCode.COMPLETE.ToString(), EnumDispositionFileStatusTypeCode.ARCHIVED.ToString(), EnumDispositionFileStatusTypeCode.CANCELLED.ToString(), };
            var isFileChangingToNonEditableState = !nonEditableStatuses.Contains(currentDispositionFile.DispositionFileStatusTypeCode) && nonEditableStatuses.Contains(incomingDispositionFile.DispositionFileStatusTypeCode);

            if (isFileClosing && incomingDispositionFile.DispositionStatusTypeCode != EnumDispositionStatusTypeCode.SOLD.ToString())
            {
                throw new BusinessRuleViolationException("File Disposition Status has not been set to SOLD, so the related file properties cannot be Disposed. To proceed, set file disposition status to SOLD, or cancel the Disposition file.");
            }

            // confirm user action - file is changing to non-editable state
            if (!userOverrides.Contains(UserOverrideCode.DispositionFileFinalStatus) && isFileChangingToNonEditableState)
            {
                throw new UserOverrideException(UserOverrideCode.DispositionFileFinalStatus, "You are changing this file to a non-editable state. (Only system administrators can edit the file when set to Archived, Cancelled or Completed state). Do you wish to continue?");
            }

            if (isFileClosing && !userOverrides.Contains(UserOverrideCode.DisposeOfProperties))
            {
                throw new UserOverrideException(UserOverrideCode.DisposeOfProperties, "You are completing this Disposition File with owned PIMS inventory properties. All properties will be removed from the PIMS inventory (any Other Interests will remain). Do you wish to proceed?");
            }
        }

        /// <summary>
        /// Attempt to dispose of any properties if all business rules are met.
        /// </summary>
        /// <param name="dispositionFile">The disposition file entity.</param>
        private void DisposeOfProperties(PimsDispositionFile dispositionFile)
        {
            // Get the current properties in the disposition file
            var currentProperties = _dispositionFilePropertyRepository.GetPropertiesByDispositionFileId(dispositionFile.Internal_Id);
            var ownedProperties = currentProperties.Where(p => p.Property.IsOwned);

            // PSP-7275 Business rule: Transfer properties of interest to disposed when disposition file is completed
            foreach (var dispositionProperty in ownedProperties)
            {
                var property = dispositionProperty.Property;
                _propertyRepository.TransferFileProperty(property, false);
            }
        }

        private static decimal CalculateNetProceedsBeforeSpp(PimsDispositionSale sale)
        {
            if (sale != null)
            {
                return (sale?.SaleFinalAmt ?? 0) - ((sale?.RealtorCommissionAmt ?? 0) + (sale.GstCollectedAmt ?? 0) + (sale.NetBookAmt ?? 0) + (sale.TotalCostAmt ?? 0));
            }
            return 0;
        }

        private static decimal CalculateNetProceedsAfterSpp(PimsDispositionSale sale)
        {
            if (sale != null)
            {
                return (sale?.SaleFinalAmt ?? 0) - ((sale?.RealtorCommissionAmt ?? 0) + (sale.GstCollectedAmt ?? 0) + (sale.NetBookAmt ?? 0) + (sale.TotalCostAmt ?? 0) + (sale.SppAmt ?? 0));
            }
            return 0;
        }

        private static void ValidateStaff(PimsDispositionFile dispositionFile)
        {
            bool duplicate = dispositionFile.PimsDispositionFileTeams.GroupBy(p => p.DspFlTeamProfileTypeCode).Any(g => g.Count() > 1);
            if (duplicate)
            {
                throw new BadRequestException("Invalid Disposition team, each team member and role combination can only be added once.");
            }
        }

        private static void ValidateDispositionOfferStatus(PimsDispositionFile dispositionFile, PimsDispositionOffer newOffer)
        {
            bool offerAlreadyAccepted = dispositionFile.PimsDispositionOffers.Any(x => x.DispositionOfferStatusTypeCode == EnumDispositionOfferStatusTypeCode.ACCCEPTED.ToString() && x.DispositionOfferId != newOffer.DispositionOfferId);
            if (offerAlreadyAccepted && !string.IsNullOrEmpty(newOffer.DispositionOfferStatusTypeCode) && newOffer.DispositionOfferStatusTypeCode == EnumDispositionOfferStatusTypeCode.ACCCEPTED.ToString())
            {
                throw new DuplicateEntityException("Invalid Disposition Offer, an Offer has been already accepted for this Disposition File");
            }
        }

        private void AddNoteIfStatusChanged(PimsDispositionFile updateDispositionFile)
        {
            var currentDispositionFile = _dispositionFileRepository.GetById(updateDispositionFile.Internal_Id);
            bool statusChanged = currentDispositionFile.DispositionFileStatusTypeCode != updateDispositionFile.DispositionFileStatusTypeCode;
            if (!statusChanged)
            {
                return;
            }

            var newStatus = _lookupRepository.GetAllDispositionFileStatusTypes()
                .FirstOrDefault(x => x.DispositionFileStatusTypeCode == updateDispositionFile.DispositionFileStatusTypeCode);

            PimsDispositionFileNote fileNoteInstance = new()
            {
                DispositionFileId = updateDispositionFile.Internal_Id,
                AppCreateTimestamp = DateTime.Now,
                AppCreateUserid = _user.GetUsername(),
                Note = new PimsNote()
                {
                    IsSystemGenerated = true,
                    NoteTxt = $"Disposition File status changed from {currentDispositionFile.DispositionFileStatusTypeCodeNavigation.Description} to {newStatus.Description}",
                    AppCreateTimestamp = DateTime.Now,
                    AppCreateUserid = this._user.GetUsername(),
                },
            };

            _entityNoteRepository.Add(fileNoteInstance);
        }

        private void ValidateMinistryRegion(long dispositionFileId, short updatedRegion)
        {
            short currentRegion = _dispositionFileRepository.GetRegion(dispositionFileId);
            if (currentRegion != updatedRegion)
            {
                throw new UserOverrideException(UserOverrideCode.UpdateRegion, "The Ministry region has been changed, this will result in a change to the file's prefix. This requires user confirmation.");
            }
        }

        private void ReprojectPropertyLocationsToWgs84(IEnumerable<PimsDispositionFileProperty> dispositionPropertyFiles)
        {
            foreach (var dispositionProperty in dispositionPropertyFiles)
            {
                if (dispositionProperty.Property.Location != null)
                {
                    var oldCoords = dispositionProperty.Property.Location.Coordinate;
                    var newCoords = _coordinateService.TransformCoordinates(SpatialReference.BCALBERS, SpatialReference.WGS84, oldCoords);
                    dispositionProperty.Property.Location = GeometryHelper.CreatePoint(newCoords, SpatialReference.WGS84);
                }
            }
        }

        private void MatchProperties(PimsDispositionFile dispositionFile, IEnumerable<UserOverrideCode> overrideCodes)
        {
            foreach (var dispProperty in dispositionFile.PimsDispositionFileProperties)
            {
                if (dispProperty.Property.Pid.HasValue)
                {
                    var pid = dispProperty.Property.Pid.Value;
                    try
                    {
                        var foundProperty = _propertyRepository.GetByPid(pid, true);
                        if (foundProperty.IsRetired.HasValue && foundProperty.IsRetired.Value)
                        {
                            throw new BusinessRuleViolationException("Retired property can not be selected.");
                        }

                        dispProperty.PropertyId = foundProperty.Internal_Id;
                        _propertyService.UpdateLocation(dispProperty.Property, ref foundProperty, overrideCodes);
                        dispProperty.Property = foundProperty;
                    }
                    catch (KeyNotFoundException)
                    {
                        if (overrideCodes.Contains(UserOverrideCode.DisposingPropertyNotInventoried))
                        {
                            _logger.LogDebug("Adding new property with pid:{pid}", pid);
                            dispProperty.Property = _propertyService.PopulateNewProperty(dispProperty.Property, true, false);
                        }
                        else
                        {
                            throw new UserOverrideException(UserOverrideCode.DisposingPropertyNotInventoried, "You have added one or more properties to the disposition file that are not in the MOTI Inventory. Do you want to proceed?");
                        }
                    }
                }
                else if (dispProperty.Property.Pin.HasValue)
                {
                    var pin = dispProperty.Property.Pin.Value;
                    try
                    {
                        var foundProperty = _propertyRepository.GetByPin(pin, true);
                        if (foundProperty.IsRetired.HasValue && foundProperty.IsRetired.Value)
                        {
                            throw new BusinessRuleViolationException("Retired property can not be selected.");
                        }

                        dispProperty.PropertyId = foundProperty.Internal_Id;
                        _propertyService.UpdateLocation(dispProperty.Property, ref foundProperty, overrideCodes);
                        dispProperty.Property = foundProperty;
                    }
                    catch (KeyNotFoundException)
                    {
                        if (overrideCodes.Contains(UserOverrideCode.DisposingPropertyNotInventoried))
                        {
                            _logger.LogDebug("Adding new property with pin:{pin}", pin);
                            dispProperty.Property = _propertyService.PopulateNewProperty(dispProperty.Property, true, false);
                        }
                        else
                        {
                            throw new UserOverrideException(UserOverrideCode.DisposingPropertyNotInventoried, "You have added one or more properties to the disposition file that are not in the MoTI Inventory. Do you want to proceed?");
                        }
                    }
                }
                else
                {
                    if (overrideCodes.Contains(UserOverrideCode.DisposingPropertyNotInventoried))
                    {
                        _logger.LogDebug("Adding new property without a pid");
                        dispProperty.Property = _propertyService.PopulateNewProperty(dispProperty.Property, true, false);
                    }
                    else
                    {
                        throw new UserOverrideException(UserOverrideCode.DisposingPropertyNotInventoried, "You have added one or more properties to the disposition file that are not in the MoTI Inventory. Do you want to proceed?");
                    }
                }
            }
        }

        private void ValidatePropertyRegions(PimsDispositionFile dispositionFile)
        {
            var userRegions = _user.GetUserRegions(_userRepository);
            foreach (var dispProperty in dispositionFile.PimsDispositionFileProperties)
            {
                var propertyRegion = dispProperty.Property?.RegionCode ?? _propertyRepository.GetPropertyRegion(dispProperty.PropertyId);
                if (!userRegions.Contains(propertyRegion))
                {
                    throw new BadRequestException("You cannot add a property that is outside of your user account region(s). Either select a different property, or get your system administrator to add the required region to your user account settings.");
                }
            }
        }

        private void AppendToDispositionChecklist(PimsDispositionFile dispositionFile, ref List<PimsDispositionChecklistItem> pimsDispositionChecklistItems)
        {
            var doNotAddToStatuses = new List<string>() { "COMPLT", "CANCEL", "ARCHIV" };
            if (doNotAddToStatuses.Contains(dispositionFile.DispositionFileStatusTypeCode))
            {
                return;
            }
            var checklistStatusTypes = _lookupRepository.GetAllDispositionChecklistItemStatusTypes();
            foreach (var itemType in _checklistRepository.GetAllChecklistItemTypes().Where(x => !x.IsExpiredType()))
            {
                if (!pimsDispositionChecklistItems.Any(cli => cli.DspChklstItemTypeCode == itemType.DspChklstItemTypeCode) && DateOnly.FromDateTime(dispositionFile.AppCreateTimestamp) >= itemType.EffectiveDate)
                {
                    var checklistItem = new PimsDispositionChecklistItem
                    {
                        DspChklstItemTypeCode = itemType.DspChklstItemTypeCode,
                        DspChklstItemTypeCodeNavigation = itemType,
                        DspChklstItemStatusTypeCode = "INCOMP",
                        DispositionFileId = dispositionFile.DispositionFileId,
                        DspChklstItemStatusTypeCodeNavigation = checklistStatusTypes.FirstOrDefault(cst => cst.Id == "INCOMP"),
                    };

                    pimsDispositionChecklistItems.Add(checklistItem);
                }
            }
        }

        private void ValidateVersion(long dispositionFileId, long dispositionFileVersion)
        {
            long currentRowVersion = _dispositionFileRepository.GetRowVersion(dispositionFileId);
            if (currentRowVersion != dispositionFileVersion)
            {
                throw new DbUpdateConcurrencyException("You are working with an older version of this disposition file, please refresh the application and retry.");
            }
        }

        private DispositionStatusTypes? GetCurrentDispositionStatus(long dispositionFileId)
        {
            var currentDispositionFile = _dispositionFileRepository.GetById(dispositionFileId);
            DispositionStatusTypes currentDispositionStatus;

            if (Enum.TryParse(currentDispositionFile.DispositionFileStatusTypeCode, out currentDispositionStatus))
            {
                return currentDispositionStatus;
            }

            return currentDispositionStatus;
        }
    }
}
