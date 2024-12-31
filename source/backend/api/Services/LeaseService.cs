using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.CodeTypes;
using Pims.Core.Api.Exceptions;
using Pims.Core.Api.Services;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Extensions;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using static Pims.Dal.Entities.PimsLeaseStatusType;

namespace Pims.Api.Services
{
    public class LeaseService : BaseService, ILeaseService
    {
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly ILeaseRepository _leaseRepository;
        private readonly IPropertyImprovementRepository _propertyImprovementRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyLeaseRepository _propertyLeaseRepository;
        private readonly IEntityNoteRepository _entityNoteRepository;
        private readonly IInsuranceRepository _insuranceRepository;
        private readonly ILeaseStakeholderRepository _stakeholderRepository;
        private readonly ICompensationRequisitionRepository _compensationRequisitionRepository;
        private readonly ILeaseRenewalRepository _renewalRepository;
        private readonly IConsultationRepository _consultationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPropertyService _propertyService;
        private readonly ILookupRepository _lookupRepository;
        private readonly ICompReqFinancialService _compReqFinancialService;
        private readonly IPropertyOperationService _propertyOperationService;

        public LeaseService(
            ClaimsPrincipal user,
            ILogger<LeaseService> logger,
            ILeaseRepository leaseRepository,
            IPropertyRepository propertyRepository,
            IPropertyLeaseRepository propertyLeaseRepository,
            IPropertyImprovementRepository propertyImprovementRepository,
            IEntityNoteRepository entityNoteRepository,
            IInsuranceRepository insuranceRepository,
            ILeaseStakeholderRepository stakeholderRepository,
            ICompensationRequisitionRepository compensationRequisitionRepository,
            ILeaseRenewalRepository renewalRepository,
            IConsultationRepository consultationRepository,
            IUserRepository userRepository,
            IPropertyService propertyService,
            ILookupRepository lookupRepository,
            ICompReqFinancialService compReqFinancialService,
            IPropertyOperationService propertyOperationService)
            : base(user, logger)
        {
            _logger = logger;
            _user = user;
            _leaseRepository = leaseRepository;
            _propertyRepository = propertyRepository;
            _propertyLeaseRepository = propertyLeaseRepository;
            _entityNoteRepository = entityNoteRepository;
            _propertyImprovementRepository = propertyImprovementRepository;
            _insuranceRepository = insuranceRepository;
            _stakeholderRepository = stakeholderRepository;
            _compensationRequisitionRepository = compensationRequisitionRepository;
            _renewalRepository = renewalRepository;
            _consultationRepository = consultationRepository;
            _userRepository = userRepository;
            _propertyService = propertyService;
            _lookupRepository = lookupRepository;
            _compReqFinancialService = compReqFinancialService;
            _propertyOperationService = propertyOperationService;
        }

        public PimsLease GetById(long leaseId)
        {
            _logger.LogInformation("Getting lease {leaseId}", leaseId);
            _user.ThrowIfNotAuthorized(Permissions.LeaseView);
            var pimsUser = _userRepository.GetByKeycloakUserId(_user.GetUserKey());
            pimsUser.ThrowInvalidAccessToLeaseFile(_leaseRepository.GetNoTracking(leaseId).RegionCode);

            var lease = _leaseRepository.Get(leaseId);
            return lease;
        }

        public IEnumerable<PimsLease> GetAllByIds(IEnumerable<long> leaseIds)
        {
            _logger.LogInformation("Getting all leases with ids {leaseIds}", leaseIds);
            _user.ThrowIfNotAuthorized(Permissions.LeaseView);
            var pimsUser = _userRepository.GetByKeycloakUserId(_user.GetUserKey());
            var leases = _leaseRepository.GetAllByIds(leaseIds).ToList();
            leaseIds.ForEach(leaseId =>
            {
                pimsUser.ThrowInvalidAccessToLeaseFile(leases.FirstOrDefault(lease => lease.LeaseId == leaseId).RegionCode);
            });
            return leases;
        }

        public LastUpdatedByModel GetLastUpdateInformation(long leaseId)
        {
            _logger.LogInformation("Retrieving last updated-by information...");
            _user.ThrowIfNotAuthorized(Permissions.LeaseView);

            return _leaseRepository.GetLastUpdateBy(leaseId);
        }

        public Paged<PimsLease> GetPage(LeaseFilter filter, bool? all = false)
        {
            _logger.LogInformation("Getting lease page {filter}", filter);
            _user.ThrowIfNotAuthorized(Permissions.LeaseView);
            filter.Quantity = all.HasValue && all.Value ? _leaseRepository.Count() : filter.Quantity;
            var user = _userRepository.GetByKeycloakUserId(this.User.GetUserKey());

            var leases = _leaseRepository.GetPage(filter, user.PimsRegionUsers.Select(u => u.RegionCode).ToHashSet());
            return leases;
        }

        public IEnumerable<PimsInsurance> GetInsuranceByLeaseId(long leaseId)
        {
            _logger.LogInformation("Getting insurance on lease {leaseId}", leaseId);
            _user.ThrowIfNotAuthorized(Permissions.LeaseView);
            var pimsUser = _userRepository.GetByKeycloakUserId(_user.GetUserKey());
            pimsUser.ThrowInvalidAccessToLeaseFile(_leaseRepository.GetNoTracking(leaseId).RegionCode);

            return _insuranceRepository.GetByLeaseId(leaseId);
        }

        public IEnumerable<PimsInsurance> UpdateInsuranceByLeaseId(long leaseId, IEnumerable<PimsInsurance> pimsInsurances)
        {
            _logger.LogInformation("Updating insurance on lease {leaseId}", leaseId);
            _user.ThrowIfNotAuthorized(Permissions.LeaseEdit);
            var pimsUser = _userRepository.GetByKeycloakUserId(_user.GetUserKey());
            pimsUser.ThrowInvalidAccessToLeaseFile(_leaseRepository.GetNoTracking(leaseId).RegionCode);

            _insuranceRepository.UpdateLeaseInsurance(leaseId, pimsInsurances);
            _insuranceRepository.CommitTransaction();

            return _insuranceRepository.GetByLeaseId(leaseId);
        }

        public IEnumerable<PimsPropertyImprovement> GetImprovementsByLeaseId(long leaseId)
        {
            _logger.LogInformation("Getting property improvements on lease {leaseId}", leaseId);
            _user.ThrowIfNotAuthorized(Permissions.LeaseView);
            var pimsUser = _userRepository.GetByKeycloakUserId(_user.GetUserKey());
            pimsUser.ThrowInvalidAccessToLeaseFile(_leaseRepository.GetNoTracking(leaseId).RegionCode);

            return _propertyImprovementRepository.GetByLeaseId(leaseId);
        }

        public IEnumerable<PimsPropertyImprovement> UpdateImprovementsByLeaseId(long leaseId, IEnumerable<PimsPropertyImprovement> pimsPropertyImprovements)
        {
            _logger.LogInformation("Updating property improvements on lease {leaseId}", leaseId);
            _user.ThrowIfNotAuthorized(Permissions.LeaseEdit);
            var pimsUser = _userRepository.GetByKeycloakUserId(_user.GetUserKey());
            pimsUser.ThrowInvalidAccessToLeaseFile(_leaseRepository.GetNoTracking(leaseId).RegionCode);

            _propertyImprovementRepository.Update(leaseId, pimsPropertyImprovements);
            _propertyImprovementRepository.CommitTransaction();

            return _propertyImprovementRepository.GetByLeaseId(leaseId);
        }

        public IEnumerable<PimsLeaseStakeholder> GetStakeholdersByLeaseId(long leaseId)
        {
            _logger.LogInformation("Getting stakeholders on lease {leaseId}", leaseId);
            _user.ThrowIfNotAuthorized(Permissions.LeaseView);
            var pimsUser = _userRepository.GetByKeycloakUserId(_user.GetUserKey());
            pimsUser.ThrowInvalidAccessToLeaseFile(_leaseRepository.GetNoTracking(leaseId).RegionCode);

            return _stakeholderRepository.GetByLeaseId(leaseId);
        }

        public IEnumerable<PimsLeaseStakeholder> UpdateStakeholdersByLeaseId(long leaseId, IEnumerable<PimsLeaseStakeholder> pimsLeaseStakeholders)
        {
            _logger.LogInformation("Updating stakeholders on lease {leaseId}", leaseId);
            _user.ThrowIfNotAuthorized(Permissions.LeaseEdit);
            var pimsUser = _userRepository.GetByKeycloakUserId(_user.GetUserKey());
            pimsUser.ThrowInvalidAccessToLeaseFile(_leaseRepository.GetNoTracking(leaseId).RegionCode);

            ValidateStakeholdersDependency(leaseId, pimsLeaseStakeholders);
            _stakeholderRepository.Update(leaseId, pimsLeaseStakeholders);
            _stakeholderRepository.CommitTransaction();

            return _stakeholderRepository.GetByLeaseId(leaseId);
        }

        public PimsLease Add(PimsLease lease, IEnumerable<UserOverrideCode> userOverrides)
        {
            _logger.LogInformation("Adding lease");
            _user.ThrowIfNotAuthorized(Permissions.LeaseAdd);
            var pimsUser = _userRepository.GetByKeycloakUserId(_user.GetUserKey());
            pimsUser.ThrowInvalidAccessToLeaseFile(lease.RegionCode);

            var leaseWithProperties = AssociatePropertyLeases(lease, userOverrides);

            lease.PimsLeaseChecklistItems = GetActiveChecklistItemsForLease();

            // Update marker locations in the context of this file
            foreach (var incomingLeaseProperty in leaseWithProperties.PimsPropertyLeases)
            {
                _propertyService.PopulateNewFileProperty(incomingLeaseProperty);
            }

            return _leaseRepository.Add(leaseWithProperties);
        }

        public IEnumerable<PimsPropertyLease> GetPropertiesByLeaseId(long leaseId)
        {
            _logger.LogInformation("Getting properties on lease {leaseId}", leaseId);
            _user.ThrowIfNotAuthorized(Permissions.LeaseView);
            var pimsUser = _userRepository.GetByKeycloakUserId(_user.GetUserKey());
            pimsUser.ThrowInvalidAccessToLeaseFile(_leaseRepository.GetNoTracking(leaseId).RegionCode);

            var propertyLeases = _propertyLeaseRepository.GetAllByLeaseId(leaseId);
            return _propertyService.TransformAllPropertiesToLatLong(propertyLeases.ToList());
        }

        public PimsLease Update(PimsLease lease, IEnumerable<UserOverrideCode> userOverrides)
        {
            _logger.LogInformation("Updating lease {leaseId}", lease.LeaseId);
            _user.ThrowIfNotAuthorized(Permissions.LeaseEdit);

            var pimsUser = _userRepository.GetByKeycloakUserId(_user.GetUserKey());
            var currentLease = _leaseRepository.GetNoTracking(lease.LeaseId);

            pimsUser.ThrowInvalidAccessToLeaseFile(currentLease.RegionCode); // need to check that the user is able to access the current lease as well as has the region for the updated lease.
            pimsUser.ThrowInvalidAccessToLeaseFile(lease.RegionCode);

            var currentFileProperties = _propertyLeaseRepository.GetAllByLeaseId(lease.LeaseId);

            if (currentLease.LeaseStatusTypeCode != lease.LeaseStatusTypeCode)
            {
                PimsLeaseNote newLeaseNote = GeneratePimsLeaseNote(currentLease, lease);

                _entityNoteRepository.Add(newLeaseNote);
            }

            ValidateRenewalDates(lease, lease.PimsLeaseRenewals);

            ValidateNewTotalAllowableCompensation(lease.LeaseId, lease.TotalAllowableCompensation);

            _leaseRepository.Update(lease, false);
            var leaseWithProperties = AssociatePropertyLeases(lease, userOverrides);

            // Update marker locations in the context of this file
            foreach (var incomingLeaseProperty in leaseWithProperties.PimsPropertyLeases)
            {
                var matchingProperty = currentFileProperties.FirstOrDefault(c => c.PropertyId == incomingLeaseProperty.PropertyId);
                if (matchingProperty is not null && incomingLeaseProperty.Internal_Id == 0)
                {
                    incomingLeaseProperty.Internal_Id = matchingProperty.Internal_Id;
                }

                // If the property is not new, check if the marker location has been updated.
                if (incomingLeaseProperty.Internal_Id != 0)
                {
                    var existingFileProperty = currentFileProperties.FirstOrDefault(x => x.Internal_Id == incomingLeaseProperty.Internal_Id);

                    var incomingGeom = incomingLeaseProperty?.Location;
                    var existingGeom = existingFileProperty?.Location;
                    if (existingGeom is null || (incomingGeom is not null && !existingGeom.EqualsExact(incomingGeom)))
                    {
                        _propertyService.UpdateFilePropertyLocation(incomingLeaseProperty, existingFileProperty);
                        incomingLeaseProperty.Location = existingFileProperty?.Location;
                    }
                }
                else
                {
                    // New property needs to be added
                    _propertyService.PopulateNewFileProperty(incomingLeaseProperty);
                }
            }

            _propertyLeaseRepository.UpdatePropertyLeases(lease.Internal_Id, leaseWithProperties.PimsPropertyLeases);

            _leaseRepository.UpdateLeaseRenewals(lease.Internal_Id, lease.ConcurrencyControlNumber, lease.PimsLeaseRenewals);

            List<PimsPropertyLease> differenceSet = currentFileProperties.Where(x => !lease.PimsPropertyLeases.Any(y => y.Internal_Id == x.Internal_Id)).ToList();
            foreach (var deletedProperty in differenceSet)
            {
                if (_propertyLeaseRepository.LeaseFilePropertyInCompensationReq(deletedProperty.PropertyLeaseId))
                {
                    throw new BusinessRuleViolationException("Lease File property can not be removed since it's assigned as a property for a compensation requisition");
                }

                if (_propertyOperationService.GetOperationsForProperty(deletedProperty.PropertyId).Count > 0)
                {
                    throw new BusinessRuleViolationException("This property cannot be deleted because it is part of a subdivision or consolidation");
                }

                var totalAssociationCount = _propertyRepository.GetAllAssociationsCountById(deletedProperty.PropertyId);
                if (totalAssociationCount <= 1)
                {
                    _leaseRepository.CommitTransaction(); // TODO: this can only be removed if cascade deletes are implemented. EF executes deletes in alphabetic order.
                    _propertyRepository.Delete(deletedProperty.Property);
                }
            }

            _leaseRepository.CommitTransaction();

            return _leaseRepository.GetNoTracking(lease.LeaseId);
        }

        public IEnumerable<PimsLeaseRenewal> GetRenewalsByLeaseId(long leaseId)
        {
            _logger.LogInformation("Getting renewals on lease {leaseId}", leaseId);
            _user.ThrowIfNotAuthorized(Permissions.LeaseView);
            var pimsUser = _userRepository.GetByKeycloakUserId(_user.GetUserKey());
            pimsUser.ThrowInvalidAccessToLeaseFile(_leaseRepository.GetNoTracking(leaseId).RegionCode);

            return _renewalRepository.GetByLeaseId(leaseId);
        }

        public IEnumerable<PimsLeaseChecklistItem> GetChecklistItems(long id)
        {
            _logger.LogInformation("Getting Lease checklist with Id: {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.LeaseView);

            var pimsUser = _userRepository.GetByKeycloakUserId(_user.GetUserKey());
            pimsUser.ThrowInvalidAccessToLeaseFile(_leaseRepository.GetNoTracking(id).RegionCode);

            var checklistItems = _leaseRepository.GetAllChecklistItemsByLeaseId(id);

            var lease = _leaseRepository.Get(id);
            AppendNewItemsToChecklist(lease, ref checklistItems);

            return checklistItems;
        }

        public PimsLease UpdateChecklistItems(long leaseId, IList<PimsLeaseChecklistItem> checklistItems)
        {
            checklistItems.ThrowIfNull(nameof(checklistItems));

            _logger.LogInformation("Updating Lease checklist with id: {leaseId}", leaseId);
            _user.ThrowIfNotAuthorized(Permissions.LeaseEdit);

            var pimsUser = _userRepository.GetByKeycloakUserId(_user.GetUserKey());
            pimsUser.ThrowInvalidAccessToLeaseFile(_leaseRepository.GetNoTracking(leaseId).RegionCode);

            // Get the current checklist items for this acquisition file.
            var currentItems = _leaseRepository.GetAllChecklistItemsByLeaseId(leaseId).ToDictionary(ci => ci.LeaseChecklistItemId);

            foreach (var incomingItem in checklistItems)
            {
                if (!currentItems.TryGetValue(incomingItem.LeaseChecklistItemId, out var existingItem) && incomingItem.LeaseChecklistItemId != 0)
                {
                    throw new BadRequestException($"Cannot update checklist item. Item with Id: {incomingItem.LeaseChecklistItemId} not found.");
                }

                // Only update checklist items that changed.
                if (existingItem == null)
                {
                    _leaseRepository.AddChecklistItem(incomingItem);
                }
                else if (existingItem.ChklstItemStatusTypeCode != incomingItem.ChklstItemStatusTypeCode)
                {
                    _leaseRepository.UpdateChecklistItem(incomingItem);
                }
            }

            _leaseRepository.CommitTransaction();

            return _leaseRepository.Get(leaseId);
        }

        public IEnumerable<PimsLeaseStakeholderType> GetAllStakeholderTypes()
        {
            return _leaseRepository.GetAllLeaseStakeholderTypes();
        }

        public IEnumerable<PimsLeaseConsultation> GetConsultations(long leaseId)
        {
            _logger.LogInformation("Getting lease consultations with Lease id: {leaseId}", leaseId);
            _user.ThrowIfNotAuthorized(Permissions.LeaseView);

            return _consultationRepository.GetConsultationsByLease(leaseId);
        }

        public PimsLeaseConsultation GetConsultationById(long consultationId)
        {
            _logger.LogInformation("Getting consultation with id: {consultationId}", consultationId);
            _user.ThrowIfNotAuthorized(Permissions.LeaseView);

            return _consultationRepository.GetConsultationById(consultationId);
        }

        public PimsLeaseConsultation AddConsultation(PimsLeaseConsultation consultation)
        {
            _user.ThrowIfNotAuthorized(Permissions.LeaseEdit);

            var newConsultation = _consultationRepository.AddConsultation(consultation);
            _consultationRepository.CommitTransaction();

            return newConsultation;
        }

        public PimsLeaseConsultation UpdateConsultation(PimsLeaseConsultation consultation)
        {
            _user.ThrowIfNotAuthorized(Permissions.LeaseEdit);

            var updatedConsultation = _consultationRepository.UpdateConsultation(consultation);
            _consultationRepository.CommitTransaction();

            return updatedConsultation;
        }

        public bool DeleteConsultation(long consultationId)
        {
            _user.ThrowIfNotAuthorized(Permissions.LeaseEdit);

            bool deleteResult = _consultationRepository.TryDeleteConsultation(consultationId);
            _consultationRepository.CommitTransaction();

            return deleteResult;
        }

        private static void ValidateRenewalDates(PimsLease lease, ICollection<PimsLeaseRenewal> renewals)
        {
            if (lease.LeaseStatusTypeCode != PimsLeaseStatusTypes.ACTIVE)
            {
                return;
            }

            List<Tuple<DateTime, DateTime>> renewalDates = new();

            foreach (var renewal in renewals)
            {
                if (renewal.IsExercised == true)
                {
                    if (renewal.CommencementDt.HasValue && renewal.ExpiryDt.HasValue)
                    {
                        renewalDates.Add(new Tuple<DateTime, DateTime>(renewal.CommencementDt.Value, renewal.ExpiryDt.Value));
                    }
                    else
                    {
                        throw new BusinessRuleViolationException("Exercised renewals must have a commencement date and expiry date");
                    }
                }
            }

            // Sort agreement dates/renewals by start date.
            renewalDates.Sort((a, b) => DateTime.Compare(a.Item1, b.Item1));

            DateTime currentEndDate;

            if (lease.OrigStartDate.HasValue && lease.OrigExpiryDate.HasValue)
            {
                var agreementStart = lease.OrigStartDate.Value;
                var agreementEnd = lease.OrigExpiryDate.Value;
                currentEndDate = agreementEnd;
                if (DateTime.Compare(agreementEnd, agreementStart) <= 0)
                {
                    throw new BusinessRuleViolationException("The lease commencement date must be before its expiry date");
                }
            }
            else
            {
                throw new BusinessRuleViolationException("Active leases must have commencement and expiry dates");
            }

            for (int i = 0; i < renewalDates.Count; i++)
            {
                var startDate = renewalDates[i].Item1;
                var endDate = renewalDates[i].Item2;

                if (DateTime.Compare(endDate, startDate) <= 0)
                {
                    throw new BusinessRuleViolationException("The expiry date of your renewal should be later than its commencement date");
                }

                if (DateTime.Compare(currentEndDate, startDate) >= 0)
                {
                    throw new BusinessRuleViolationException("The commencement date of your renewal should be later than the previous expiry date (agreement or renewal)");
                }
                currentEndDate = endDate;
            }
        }

        private void ValidateNewTotalAllowableCompensation(long currentLeaseFileId, decimal? newAllowableCompensation)
        {
            if (newAllowableCompensation is null)
            {
                return;
            }

            IEnumerable<PimsCompReqFinancial> allFinalFinancialsOnFile = _compReqFinancialService.GetAllByLeaseFileId(currentLeaseFileId, true);
            var currentActualCompensation = allFinalFinancialsOnFile.Aggregate(0m, (acc, f) => acc + (f.TotalAmt ?? 0m));
            if (newAllowableCompensation < currentActualCompensation)
            {
                throw new BusinessRuleViolationException("The Total Allowable Compensation value cannot be saved because the value provided is less than current sum of the final compensation requisitions in this file. " +
                    "\n\nTo continue, adjust the value to accommodate the existing compensation requisitions in the file or contact your system administrator to adjust final compensations.");
            }
        }

        private PimsLeaseNote GeneratePimsLeaseNote(PimsLease currentLease, PimsLease lease)
        {
            var leaseStatuses = _lookupRepository.GetAllLeaseStatusTypes();
            string currentStatusDescription = leaseStatuses.FirstOrDefault(x => x.Id == currentLease.LeaseStatusTypeCode).Description.ToUpper();
            string newStatusDescription = leaseStatuses.FirstOrDefault(x => x.Id == lease.LeaseStatusTypeCode).Description.ToUpper();

            StringBuilder leaseNoteTextValue = new();
            PimsLeaseNote leaseNote = new()
            {
                LeaseId = currentLease.LeaseId,
                AppCreateTimestamp = DateTime.Now,
                AppCreateUserid = User.GetUsername(),
                Note = new PimsNote()
                {
                    IsSystemGenerated = true,
                    NoteTxt = leaseNoteTextValue.Append($"Lease status changed from {currentStatusDescription} to {newStatusDescription}").ToString(),
                    AppCreateTimestamp = DateTime.Now,
                    AppCreateUserid = User.GetUsername(),
                },
            };

            if (lease.LeaseStatusTypeCode == LeaseStatusTypes.DISCARD.ToString() || lease.LeaseStatusTypeCode == LeaseStatusTypes.TERMINATED.ToString())
            {
                leaseNoteTextValue.Append(". Reason: ");
                if (lease.LeaseStatusTypeCode == LeaseStatusTypes.DISCARD.ToString())
                {
                    leaseNoteTextValue.Append($"{lease.CancellationReason}.");
                }
                else
                {
                    leaseNoteTextValue.Append($"{lease.TerminationReason}.");
                }

                leaseNote.Note.NoteTxt = leaseNoteTextValue.ToString();
            }

            return leaseNote;
        }

        /// <summary>
        /// Attempt to associate property leases with real properties in the system using the pid/pin identifiers.
        /// Do not attempt to update any preexisiting properties, simply refer to them by id.
        ///
        /// By default, do not allow a property with existing leases to be associated unless the userOverride flag is true.
        /// </summary>
        /// <param name="lease"></param>
        /// <param name="userOverrides"></param>
        /// <returns></returns>
        private PimsLease AssociatePropertyLeases(PimsLease lease, IEnumerable<UserOverrideCode> userOverrides)
        {
            MatchProperties(lease, userOverrides);

            foreach (var propertyLease in lease.PimsPropertyLeases)
            {
                PimsProperty property = propertyLease.Property;
                var existingPropertyLeases = _propertyLeaseRepository.GetAllByPropertyId(property.PropertyId);
                var propertyWithAssociations = _propertyRepository.GetAllAssociationsById(property.PropertyId);
                var isPropertyOnOtherLease = existingPropertyLeases.Any(p => p.LeaseId != lease.Internal_Id);
                var isPropertyOnThisLease = existingPropertyLeases.Any(p => p.LeaseId == lease.Internal_Id);
                var isDisposedOrRetired = existingPropertyLeases.Any(p => p.Property.IsRetired.HasValue && p.Property.IsRetired.Value)
                    || propertyWithAssociations?.PimsDispositionFileProperties?.Any(d => d.DispositionFile.DispositionFileStatusTypeCode == DispositionFileStatusTypes.COMPLETE.ToString()) == true;

                if (isPropertyOnOtherLease && !isPropertyOnThisLease && !userOverrides.Contains(UserOverrideCode.AddPropertyToInventory))
                {
                    var genericOverrideErrorMsg = $"is attached to L-File # {existingPropertyLeases.FirstOrDefault().Lease.LFileNo}";
                    if (propertyLease?.Property?.Pin != null)
                    {
                        throw new UserOverrideException(UserOverrideCode.AddPropertyToInventory, $"PIN {propertyLease?.Property?.Pin.ToString() ?? string.Empty} {genericOverrideErrorMsg}");
                    }
                    if (propertyLease?.Property?.Pid != null)
                    {
                        throw new UserOverrideException(UserOverrideCode.AddPropertyToInventory, $"PID {propertyLease?.Property?.Pid.ToString() ?? string.Empty} {genericOverrideErrorMsg}");
                    }
                    string overrideError = $"Lng/Lat {propertyLease?.Property?.Location.Coordinate.X.ToString(CultureInfo.CurrentCulture) ?? string.Empty}, " +
                        $"{propertyLease?.Property?.Location.Coordinate.Y.ToString(CultureInfo.CurrentCulture) ?? string.Empty} {genericOverrideErrorMsg}";

                    throw new UserOverrideException(UserOverrideCode.AddPropertyToInventory, overrideError);
                }

                if (isDisposedOrRetired && !isPropertyOnThisLease)
                {
                    throw new BusinessRuleViolationException("Disposed or retired properties may not be added to a Lease. Remove any disposed or retired properties before continuing.");
                }

                // If the property exist dont update it, just refer to it by id.
                if (property.Internal_Id != 0)
                {
                    propertyLease.PropertyId = property.PropertyId;
                    propertyLease.Property = null;
                }
            }

            return lease;
        }

        private void MatchProperties(PimsLease lease, IEnumerable<UserOverrideCode> userOverrides)
        {
            foreach (var leaseProperty in lease.PimsPropertyLeases)
            {
                if (leaseProperty.Property.Pid.HasValue)
                {
                    var pid = leaseProperty.Property.Pid.Value;
                    try
                    {
                        var foundProperty = _propertyRepository.GetByPid(pid, true);
                        if (foundProperty.IsRetired.HasValue && foundProperty.IsRetired.Value)
                        {
                            throw new BusinessRuleViolationException("Retired property can not be selected.");
                        }

                        leaseProperty.PropertyId = foundProperty.Internal_Id;
                        _propertyService.UpdateLocation(leaseProperty.Property, ref foundProperty, userOverrides);
                        leaseProperty.Property = foundProperty;
                    }
                    catch (KeyNotFoundException)
                    {
                        _logger.LogDebug("Adding new property with pid:{pid}", pid);
                        _propertyService.PopulateNewProperty(leaseProperty.Property, true, false);
                    }
                }
                else if (leaseProperty.Property.Pin.HasValue)
                {
                    var pin = leaseProperty.Property.Pin.Value;
                    try
                    {
                        var foundProperty = _propertyRepository.GetByPin(pin, true);
                        if (foundProperty.IsRetired.HasValue && foundProperty.IsRetired.Value)
                        {
                            throw new BusinessRuleViolationException("Retired property can not be selected.");
                        }

                        leaseProperty.PropertyId = foundProperty.Internal_Id;
                        _propertyService.UpdateLocation(leaseProperty.Property, ref foundProperty, userOverrides);
                        leaseProperty.Property = foundProperty;
                    }
                    catch (KeyNotFoundException)
                    {
                        _logger.LogDebug("Adding new property with pin:{pin}", pin);
                        _propertyService.PopulateNewProperty(leaseProperty.Property, true, false);
                    }
                }
                else
                {
                    _logger.LogDebug("Adding new property without a pid or pin");
                    _propertyService.PopulateNewProperty(leaseProperty.Property, true, false);
                }
            }
        }

        private List<PimsLeaseChecklistItem> GetActiveChecklistItemsForLease()
        {
            List<PimsLeaseChecklistItem> chklistItems = new();
            foreach (var itemType in _leaseRepository.GetAllChecklistItemTypes().Where(x => !x.IsExpiredType() && !x.IsDisabled))
            {
                PimsLeaseChecklistItem checklistItem = new()
                {
                    LeaseChklstItemTypeCode = itemType.LeaseChklstItemTypeCode,
                    ChklstItemStatusTypeCode = ChecklistItemStatusTypes.INCOMP.ToString(),
                };

                chklistItems.Add(checklistItem);
            }

            return chklistItems;
        }

        private void AppendNewItemsToChecklist(PimsLease lease, ref List<PimsLeaseChecklistItem> pimsLeaseChecklistItems)
        {
            PimsChklstItemStatusType incompleteStatusType = _lookupRepository.GetAllChecklistItemStatusTypes().FirstOrDefault(cst => cst.Id == ChecklistItemStatusTypes.INCOMP.ToString());
            foreach (var itemType in _leaseRepository.GetAllChecklistItemTypes().Where(x => !x.IsExpiredType() && !x.IsDisabled))
            {
                if (!pimsLeaseChecklistItems.Any(cli => cli.LeaseChklstItemTypeCode == itemType.LeaseChklstItemTypeCode))
                {
                    var checklistItem = new PimsLeaseChecklistItem
                    {
                        LeaseChklstItemTypeCode = itemType.LeaseChklstItemTypeCode,
                        LeaseChklstItemTypeCodeNavigation = itemType,
                        ChklstItemStatusTypeCode = incompleteStatusType.Id,
                        LeaseId = lease.LeaseId,
                        ChklstItemStatusTypeCodeNavigation = incompleteStatusType,
                    };

                    pimsLeaseChecklistItems.Add(checklistItem);
                }
            }
        }

        private void ValidateStakeholdersDependency(long leaseId, IEnumerable<PimsLeaseStakeholder> stakeholders)
        {
            var currentLease = _leaseRepository.GetNoTracking(leaseId);
            var compensationRequisitions = _compensationRequisitionRepository.GetAllByLeaseFileId(leaseId);

            if (compensationRequisitions.Count == 0)
            {
                return;
            }

            foreach (var compReq in compensationRequisitions)
            {
                var leaseStakeholderCompensationRequisitions = compReq.PimsLeaseStakeholderCompReqs;
                if (leaseStakeholderCompensationRequisitions is null || leaseStakeholderCompensationRequisitions.Count == 0)
                {
                    continue;
                }

                // Check for lease stakeholders
                foreach (var leaseStakeholderCompReq in leaseStakeholderCompensationRequisitions)
                {
                    if (!stakeholders.Any(x => x.Internal_Id.Equals(leaseStakeholderCompReq.LeaseStakeholderId))
                        && currentLease.PimsLeaseStakeholders.Any(x => x.Internal_Id.Equals(leaseStakeholderCompReq.LeaseStakeholderId)))
                    {
                        throw new ForeignKeyDependencyException("Lease File Stakeholder can not be removed since it's assigned as a payee for a compensation requisition");
                    }
                }
            }
        }
    }
}
