using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Api.Constants;
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
using Pims.Api.Models.CodeTypes;

namespace Pims.Api.Services
{
    public class DispositionFileService : IDispositionFileService
    {
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly IDispositionFileRepository _dispositionFileRepository;
        private readonly IDispositionFilePropertyRepository _dispositionFilePropertyRepository;
        private readonly ICoordinateTransformService _coordinateService;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyService _propertyService;
        private readonly ILookupRepository _lookupRepository;
        private readonly IDispositionFileChecklistRepository _checklistRepository;

        public DispositionFileService(
            ClaimsPrincipal user,
            ILogger<DispositionFileService> logger,
            IDispositionFileRepository dispositionFileRepository,
            IDispositionFilePropertyRepository dispositionFilePropertyRepository,
            ICoordinateTransformService coordinateService,
            IPropertyRepository propertyRepository,
            IPropertyService propertyService,
            ILookupRepository lookupRepository,
            IDispositionFileChecklistRepository checklistRepository)
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
        }

        public PimsDispositionFile Add(PimsDispositionFile dispositionFile, IEnumerable<UserOverrideCode> userOverrides)
        {
            _logger.LogInformation("Creating Disposition File {dispositionFile}", dispositionFile);
            _user.ThrowIfNotAuthorized(Permissions.DispositionAdd);

            dispositionFile.DispositionStatusTypeCode ??= EnumDispositionStatusTypeCode.UNKNOWN.ToString();
            dispositionFile.DispositionFileStatusTypeCode ??= EnumDispositionFileStatusTypeCode.ACTIVE.ToString();

            ValidateStaff(dispositionFile);

            MatchProperties(dispositionFile, userOverrides);

            var newDispositionFile = _dispositionFileRepository.Add(dispositionFile);
            _dispositionFileRepository.CommitTransaction();

            return newDispositionFile;
        }

        public PimsDispositionFile GetById(long id)
        {
            _logger.LogInformation("Getting disposition file with id {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.DispositionView);

            var dispositionFile = _dispositionFileRepository.GetById(id);

            return dispositionFile;
        }

        public PimsDispositionFile Update(long id, PimsDispositionFile dispositionFile, IEnumerable<UserOverrideCode> userOverrides)
        {
            dispositionFile.ThrowIfNull(nameof(dispositionFile));

            _logger.LogInformation("Updating acquisition file with id {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.DispositionEdit);

            if (id != dispositionFile.DispositionFileId)
            {
                throw new BadRequestException("Invalid dispositionFileId.");
            }

            ValidateStaff(dispositionFile);
            ValidateVersion(id, dispositionFile.ConcurrencyControlNumber);

            if (!userOverrides.Contains(UserOverrideCode.DispositionFileFinalStatus))
            {
                var doNotAddToStatuses = new List<string>() { EnumDispositionFileStatusTypeCode.COMPLETE.ToString(), EnumDispositionFileStatusTypeCode.ARCHIVED.ToString() };
                if(doNotAddToStatuses.Contains(dispositionFile.DispositionFileStatusTypeCode))
                {
                    throw new UserOverrideException(UserOverrideCode.DispositionFileFinalStatus, "You are changing this file to a non-editable state. Only system administrators can edit the file when set to Archived, Cancelled or Completed state). Do you wish to continue?");
                }
            }
            if (!userOverrides.Contains(UserOverrideCode.UpdateRegion))
            {
                ValidateMinistryRegion(id, dispositionFile.RegionCode);
            }

            _dispositionFileRepository.Update(id, dispositionFile);
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

            return _dispositionFileRepository.GetPageDeep(filter);
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

            ValidateDispositionOfferStatus(dispositionFileParent, dispositionOffer);

            var updatedOffer = _dispositionFileRepository.UpdateDispositionOffer(dispositionOffer);
            _dispositionFileRepository.CommitTransaction();

            return updatedOffer;
        }

        public bool DeleteDispositionFileOffer(long dispositionFileId, long offerId)
        {
            _logger.LogInformation("Deleting Disposition Offer with id: {offerId}", offerId);
            _user.ThrowIfNotAuthorized(Permissions.DispositionEdit);

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

        public IEnumerable<PimsDispositionChecklistItem> GetChecklistItems(long id)
        {
            _logger.LogInformation("Getting disposition file checklist with DispositionFile id: {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.DispositionView);

            var checklistItems = _checklistRepository.GetAllChecklistItemsByDispositionFileId(id);
            var dispositionFile = _dispositionFileRepository.GetById(id);
            AppendToDispositionChecklist(dispositionFile, ref checklistItems);

            return checklistItems;
        }

        public PimsDispositionFile UpdateChecklistItems(PimsDispositionFile dispositionFile)
        {
            dispositionFile.ThrowIfNull(nameof(dispositionFile));
            _logger.LogInformation("Updating disposition file checklist with DispositionFile id: {id}", dispositionFile.Internal_Id);
            _user.ThrowIfNotAuthorized(Permissions.DispositionEdit);

            // Get the current checklist items for this disposition file.
            var currentItems = _checklistRepository.GetAllChecklistItemsByDispositionFileId(dispositionFile.Internal_Id).ToDictionary(ci => ci.Internal_Id);

            foreach (var incomingItem in dispositionFile.PimsDispositionChecklistItems)
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
            return _dispositionFileRepository.GetById(dispositionFile.Internal_Id);
        }

        public List<DispositionFileExportModel> GetDispositionFileExport(DispositionFilter filter)
        {
            _logger.LogInformation("Searching all Disposition Files matching the filter: {filter}", filter);
            _user.ThrowIfNotAuthorized(Permissions.DispositionView);

            var dispositionFiles = _dispositionFileRepository.GetDispositionFileExportDeep(filter);

            return dispositionFiles
                .Select(file => new DispositionFileExportModel
                {
                    FileNumber = file.FileNumber ?? string.Empty,
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
                    FileAssignedDate = file.AssignedDt.HasValue ? file.AssignedDt.Value.ToString("dd-MMM-yyyy") : string.Empty,
                    DispositionCompleted = file.CompletedDt.HasValue ? file.CompletedDt.Value.ToString("dd-MMM-yyyy") : string.Empty,
                    InitiatingDocument = file.DispositionInitiatingDocTypeCode == "OTHER" ? $"Other - {file.OtherInitiatingDocType ?? string.Empty}" : file.DispositionInitiatingDocTypeCodeNavigation?.Description ?? string.Empty,
                    InitiatingDocumentDate = file.InitiatingDocumentDt.HasValue ? file.InitiatingDocumentDt.Value.ToString("dd-MMM-yyyy") : string.Empty,
                    PhysicalFileStatus = file.DspPhysFileStatusTypeCodeNavigation?.Description ?? string.Empty,
                    AppraisalValue = file.PimsDispositionAppraisals?.FirstOrDefault()?.AppraisedAmt ?? 0,
                    AppraisalDate = file.PimsDispositionAppraisals?.FirstOrDefault()?.AppraisalDt != null ? file.PimsDispositionAppraisals?.FirstOrDefault()?.AppraisalDt.Value.ToString("dd-MMM-yyyy") : string.Empty,
                    AssessmentValue = file.PimsDispositionAppraisals?.FirstOrDefault()?.BcaValueAmt ?? 0,
                    RollYear = file.PimsDispositionAppraisals?.FirstOrDefault()?.BcaRollYear?.ToString() ?? string.Empty,
                    ListPrice = file.PimsDispositionAppraisals?.FirstOrDefault()?.ListPriceAmt ?? 0,
                    PurchaserNames = string.Join("|", file.PimsDispositionSales?.FirstOrDefault()?.PimsDispositionPurchasers.Select(x => (x.PersonId.HasValue ? x.Person.GetFullName(true) : x.Organization.Name + $" (Primary: {x.PrimaryContact?.GetFullName(true) ?? "N/A"})")) ?? Array.Empty<string>()),
                    SaleCompletionDate = file.PimsDispositionSales?.FirstOrDefault()?.SaleCompletionDt != null ? file.PimsDispositionSales?.FirstOrDefault()?.SaleCompletionDt.Value.ToString("dd-MMM-yyyy") : string.Empty,
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
                }).ToList();
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
                        var foundProperty = _propertyRepository.GetByPid(pid);
                        dispProperty.PropertyId = foundProperty.Internal_Id;
                        _propertyService.UpdateLocation(dispProperty.Property, ref foundProperty, overrideCodes);
                        dispProperty.Property = null;
                    }
                    catch (KeyNotFoundException)
                    {
                        if (overrideCodes.Contains(UserOverrideCode.DisposingPropertyNotInventoried))
                        {
                            _logger.LogDebug("Adding new property with pid:{pid}", pid);
                            dispProperty.Property = _propertyService.PopulateNewProperty(dispProperty.Property);
                        }
                        else
                        {
                            throw new UserOverrideException(UserOverrideCode.DisposingPropertyNotInventoried, "You have added one or more properties to the disposition file that are not in the MoTI Inventory. Do you want to proceed?");
                        }
                    }
                }
                else if (dispProperty.Property.Pin.HasValue)
                {
                    var pin = dispProperty.Property.Pin.Value;
                    try
                    {
                        var foundProperty = _propertyRepository.GetByPin(pin);
                        dispProperty.PropertyId = foundProperty.Internal_Id;
                        _propertyService.UpdateLocation(dispProperty.Property, ref foundProperty, overrideCodes);
                        dispProperty.Property = null;
                    }
                    catch (KeyNotFoundException)
                    {
                        if (overrideCodes.Contains(UserOverrideCode.DisposingPropertyNotInventoried))
                        {
                            _logger.LogDebug("Adding new property with pin:{pin}", pin);
                            dispProperty.Property = _propertyService.PopulateNewProperty(dispProperty.Property);
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
                        dispProperty.Property = _propertyService.PopulateNewProperty(dispProperty.Property);
                    }
                    else
                    {
                        throw new UserOverrideException(UserOverrideCode.DisposingPropertyNotInventoried, "You have added one or more properties to the disposition file that are not in the MoTI Inventory. Do you want to proceed?");
                    }
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
                throw new DbUpdateConcurrencyException("You are working with an older version of this acquisition file, please refresh the application and retry.");
            }
        }
    }
}
