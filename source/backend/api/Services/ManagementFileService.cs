using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.CodeTypes;
using Pims.Core.Api.Exceptions;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;

namespace Pims.Api.Services
{
    public class ManagementFileService : IManagementFileService
    {
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly IManagementFileRepository _managementFileRepository;
        private readonly IManagementFilePropertyRepository _managementFilePropertyRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyService _propertyService;
        private readonly ILookupRepository _lookupRepository;
        private readonly INoteRelationshipRepository<PimsManagementFileNote> _entityNoteRepository;
        private readonly IManagementFileStatusSolver _managementStatusSolver;
        private readonly IPropertyOperationService _propertyOperationService;
        private readonly IManagementActivityRepository _managementActivityRepository;

        public ManagementFileService(
            ClaimsPrincipal user,
            ILogger<ManagementFileService> logger,
            IManagementFileRepository managementFileRepository,
            IManagementFilePropertyRepository managementFilePropertyRepository,
            IPropertyRepository propertyRepository,
            IPropertyService propertyService,
            ILookupRepository lookupRepository,
            INoteRelationshipRepository<PimsManagementFileNote> entityNoteRepository,
            IManagementFileStatusSolver managementStatusSolver,
            IPropertyOperationService propertyOperationService,
            IManagementActivityRepository managementActivityRepository)
        {
            _user = user;
            _logger = logger;
            _managementFileRepository = managementFileRepository;
            _managementFilePropertyRepository = managementFilePropertyRepository;
            _propertyRepository = propertyRepository;
            _propertyService = propertyService;
            _lookupRepository = lookupRepository;
            _entityNoteRepository = entityNoteRepository;
            _managementStatusSolver = managementStatusSolver;
            _propertyOperationService = propertyOperationService;
            _managementActivityRepository = managementActivityRepository;
        }

        public PimsManagementFile Add(PimsManagementFile managementFile, IEnumerable<UserOverrideCode> userOverrides)
        {
            _logger.LogInformation("Creating management file {managementFile}", managementFile);
            _user.ThrowIfNotAuthorized(Permissions.ManagementAdd);
            ArgumentNullException.ThrowIfNull(managementFile);

            ValidateName(managementFile);
            managementFile.ManagementFileStatusTypeCode ??= ManagementFileStatusTypes.ACTIVE.ToString();
            ValidateStaff(managementFile);

            MatchProperties(managementFile, userOverrides);

            // Update marker locations in the context of this file
            foreach (var incomingManagementProperty in managementFile.PimsManagementFileProperties)
            {
                incomingManagementProperty.IsActive = true;
                _propertyService.PopulateNewFileProperty(incomingManagementProperty);
            }

            var newManagementFile = _managementFileRepository.Add(managementFile);
            _managementFileRepository.CommitTransaction();

            return newManagementFile;
        }

        public PimsManagementFile GetById(long id)
        {
            _logger.LogInformation("Getting management file with id {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.ManagementView);

            var managementFile = _managementFileRepository.GetById(id);

            return managementFile;
        }

        public PimsManagementFile Update(long id, PimsManagementFile managementFile, IEnumerable<UserOverrideCode> userOverrides)
        {
            managementFile.ThrowIfNull(nameof(managementFile));

            _logger.LogInformation("Updating management file with id {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.ManagementEdit);

            if (id != managementFile.ManagementFileId)
            {
                throw new BadRequestException("Invalid managementFileId.");
            }

            ValidateName(managementFile);

            ManagementFileStatusTypes? currentManagementStatus = GetCurrentManagementStatus(managementFile.Internal_Id);
            ManagementFileStatusTypes newManagementStatus = (ManagementFileStatusTypes)Enum.Parse(typeof(ManagementFileStatusTypes), managementFile.ManagementFileStatusTypeCode);
            ValidateStatus(currentManagementStatus, newManagementStatus);

            ValidateVersion(id, managementFile.ConcurrencyControlNumber);

            // validate management file state before proceeding with any database updates
            var currentManagementFile = _managementFileRepository.GetById(id);
            ValidateFileBeforeUpdate(managementFile, currentManagementFile);

            _managementFileRepository.Update(id, managementFile);
            AddNoteIfStatusChanged(managementFile);
            _managementFileRepository.CommitTransaction();

            return _managementFileRepository.GetById(id);
        }

        public LastUpdatedByModel GetLastUpdateInformation(long managementFileId)
        {
            _logger.LogInformation("Retrieving last updated-by information...");
            _user.ThrowIfNotAuthorized(Permissions.ManagementView);

            return _managementFileRepository.GetLastUpdateBy(managementFileId);
        }

        public IEnumerable<PimsManagementFileProperty> GetProperties(long id)
        {
            _logger.LogInformation("Getting management file properties with id {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.ManagementView);
            _user.ThrowIfNotAuthorized(Permissions.PropertyView);

            var properties = _managementFilePropertyRepository.GetPropertiesByManagementFileId(id);
            return _propertyService.TransformAllPropertiesToLatLong(properties);
        }

        public IEnumerable<PimsManagementFileTeam> GetTeamMembers()
        {
            _logger.LogInformation("Getting management team members");
            _user.ThrowIfNotAuthorized(Permissions.ManagementView);
            _user.ThrowIfNotAuthorized(Permissions.ContactView);

            var teamMembers = _managementFileRepository.GetTeamMembers();

            var persons = teamMembers.Where(x => x.Person != null).GroupBy(x => x.PersonId).Select(x => x.First());
            var organizations = teamMembers.Where(x => x.Organization != null).GroupBy(x => x.OrganizationId).Select(x => x.First());

            List<PimsManagementFileTeam> teamFilterOptions = new();
            teamFilterOptions.AddRange(persons);
            teamFilterOptions.AddRange(organizations);

            return teamFilterOptions;
        }

        public PimsManagementFile UpdateProperties(PimsManagementFile managementFile, IEnumerable<UserOverrideCode> userOverrides)
        {
            _logger.LogInformation("Updating management file properties with ManagementFile id: {id}", managementFile.Internal_Id);
            _user.ThrowIfNotAllAuthorized(Permissions.ManagementEdit, Permissions.PropertyView, Permissions.PropertyAdd);

            var currentManagementFile = _managementFileRepository.GetById(managementFile.ManagementFileId);
            var currentManagementStatus = _managementStatusSolver.GetCurrentManagementStatus(currentManagementFile?.ManagementFileStatusTypeCode);
            if (!_managementStatusSolver.CanEditProperties(currentManagementStatus))
            {
                throw new BusinessRuleViolationException("The file you are editing is not active, so you cannot save changes. Refresh your browser to see file state.");
            }

            ValidateVersion(managementFile.Internal_Id, managementFile.ConcurrencyControlNumber);

            MatchProperties(managementFile, userOverrides);

            // Get the current properties in the management file
            var currentFileProperties = _managementFilePropertyRepository.GetPropertiesByManagementFileId(managementFile.Internal_Id);

            // Check if the property is new or if it is being updated
            foreach (var incomingManagementProperty in managementFile.PimsManagementFileProperties)
            {
                var matchingProperty = currentFileProperties.FirstOrDefault(c => c.PropertyId == incomingManagementProperty.PropertyId);
                if (matchingProperty is not null && incomingManagementProperty.Internal_Id == 0)
                {
                    incomingManagementProperty.Internal_Id = matchingProperty.Internal_Id;
                }

                // If the property is not new, check if the name has been updated.
                if (incomingManagementProperty.Internal_Id != 0)
                {
                    var needsUpdate = false;
                    PimsManagementFileProperty existingProperty = currentFileProperties.FirstOrDefault(x => x.Internal_Id == incomingManagementProperty.Internal_Id);
                    if (existingProperty.PropertyName != incomingManagementProperty.PropertyName)
                    {
                        existingProperty.PropertyName = incomingManagementProperty.PropertyName;
                        needsUpdate = true;
                    }

                    if (incomingManagementProperty.IsActive != existingProperty.IsActive)
                    {
                        existingProperty.IsActive = incomingManagementProperty.IsActive;
                        AddPropertyActiveNote(incomingManagementProperty);
                        needsUpdate = true;
                    }

                    var incomingGeom = incomingManagementProperty.Location;
                    var existingGeom = existingProperty.Location;
                    if (existingGeom is null || (incomingGeom is not null && !existingGeom.EqualsExact(incomingGeom)))
                    {
                        _propertyService.UpdateFilePropertyLocation(incomingManagementProperty, existingProperty);
                        needsUpdate = true;
                    }

                    if (needsUpdate)
                    {
                        _managementFilePropertyRepository.Update(existingProperty);
                    }
                }
                else
                {
                    // New property needs to be added
                    var newFileProperty = _propertyService.PopulateNewFileProperty(incomingManagementProperty);
                    _managementFilePropertyRepository.Add(newFileProperty);
                }
            }

            IEnumerable<PimsManagementActivityProperty> fileActivityProperties = _managementActivityRepository.GetActivitiesByManagementFile(managementFile.Internal_Id).SelectMany(pa => pa.PimsManagementActivityProperties);

            // The ones not on the new set should be deleted
            List<PimsManagementFileProperty> differenceSet = currentFileProperties.Where(x => !managementFile.PimsManagementFileProperties.Any(y => y.Internal_Id == x.Internal_Id)).ToList();
            foreach (var deletedProperty in differenceSet)
            {
                if (_propertyOperationService.GetOperationsForProperty(deletedProperty.PropertyId).Count > 0)
                {
                    throw new BusinessRuleViolationException("This property cannot be deleted because it is part of a subdivision or consolidation");
                }

                if (fileActivityProperties.Any(ppa => ppa.PropertyId == deletedProperty.PropertyId))
                {
                    throw new BusinessRuleViolationException("This property cannot be deleted as it is part of an activity in this file");
                }

                _managementFilePropertyRepository.Delete(deletedProperty);

                var totalAssociationCount = _propertyRepository.GetAllAssociationsCountById(deletedProperty.PropertyId);
                if (totalAssociationCount <= 1)
                {
                    _managementFileRepository.CommitTransaction(); // TODO: this can only be removed if cascade deletes are implemented. EF executes deletes in alphabetic order.
                    _propertyRepository.Delete(deletedProperty.Property);
                }
            }

            _managementFileRepository.CommitTransaction();
            return _managementFileRepository.GetById(managementFile.Internal_Id);
        }

        public Paged<PimsManagementFile> GetPage(ManagementFilter filter)
        {
            _logger.LogInformation("Searching for management files...");
            _logger.LogDebug("Management file search with filter: {filter}", filter);
            _user.ThrowIfNotAuthorized(Permissions.ManagementView);

            return _managementFileRepository.GetPageDeep(filter);
        }

        public IEnumerable<PimsManagementFileContact> GetContacts(long id)
        {
            _logger.LogInformation("Getting management file contacts");
            _user.ThrowIfNotAuthorized(Permissions.ManagementView);

            return _managementFileRepository.GetContacts(id);
        }

        public PimsManagementFileContact GetContact(long managementFileId, long contactId)
        {
            _logger.LogInformation("Getting management file contact");
            _user.ThrowIfNotAuthorized(Permissions.ManagementView);

            return _managementFileRepository.GetContact(managementFileId, contactId);
        }

        public PimsManagementFileContact AddContact(PimsManagementFileContact contact)
        {
            _logger.LogInformation("Creating file contact");
            _user.ThrowIfNotAuthorized(Permissions.ManagementEdit);

            _managementFileRepository.AddContact(contact);
            _managementActivityRepository.CommitTransaction();

            return contact;
        }

        public PimsManagementFileContact UpdateContact(PimsManagementFileContact contact)
        {
            _logger.LogInformation("Updating file contact...");
            _user.ThrowIfNotAuthorized(Permissions.ManagementEdit);

            _managementFileRepository.UpdateContact(contact);
            _managementActivityRepository.CommitTransaction();

            return contact;
        }

        public bool DeleteContact(long managementFileId, long contactId)
        {
            _logger.LogInformation("Deleting file contact...");
            _user.ThrowIfNotAuthorized(Permissions.ManagementEdit);

            _managementFileRepository.DeleteContact(managementFileId, contactId);
            _managementActivityRepository.CommitTransaction();

            return true;
        }

        private static void ValidateStaff(PimsManagementFile managementFile)
        {
            bool duplicate = managementFile.PimsManagementFileTeams.GroupBy(p => $"{p.ManagementFileProfileTypeCode}-O-{p.OrganizationId}-P-{p.PersonId}").Any(g => g.Count() > 1);
            if (duplicate)
            {
                throw new BadRequestException("Invalid Management team, a team member can only be added to a role once.");
            }
        }

        private void ValidateStatus(ManagementFileStatusTypes? currentManagementStatus, ManagementFileStatusTypes newManagementStatus)
        {
            // All users can change states back to Active or Draft, from Hold, Cancelled.
            if ((currentManagementStatus == ManagementFileStatusTypes.HOLD || currentManagementStatus == ManagementFileStatusTypes.CANCELLED)
                && (newManagementStatus == ManagementFileStatusTypes.ACTIVE || newManagementStatus == ManagementFileStatusTypes.DRAFT))
            {
                return;
            }

            // Not Editable and Admin protected must have Admin
            if (!_managementStatusSolver.CanEditDetails(currentManagementStatus) && _managementStatusSolver.IsAdminProtected(currentManagementStatus) && !_user.HasPermission(Permissions.SystemAdmin))
            {
                throw new BusinessRuleViolationException("The file you are editing is not active, only an Administrator may update this file to an Active status.");
            }

            // Not Editable -- Status must be updated first
            if (!_managementStatusSolver.CanEditDetails(currentManagementStatus) && !_managementStatusSolver.CanEditDetails(newManagementStatus))
            {
                throw new BusinessRuleViolationException("The file you are editing is not active, so you cannot save changes. Refresh your browser to see file state.");
            }
        }

        private void ValidateName(PimsManagementFile managementFile)
        {
            var existingFile = _managementFileRepository.GetByName(managementFile.FileName);
            if (existingFile != null && managementFile.ManagementFileId != existingFile.ManagementFileId)
            {
                throw new BadRequestException("The specified file name already exists. Management File names must be unique, please choose another file name.");
            }
        }

        private void ValidateFileBeforeUpdate(PimsManagementFile incomingManagementFile, PimsManagementFile currentManagementFile)
        {
            // Implement file validation logic before proceeding to update. This includes file closing validation.
            // The order of validation checks is important as it has been requested by business users.
            var isFileClosing = currentManagementFile.ManagementFileStatusTypeCode != ManagementFileStatusTypes.COMPLETE.ToString() &&
                                incomingManagementFile.ManagementFileStatusTypeCode == ManagementFileStatusTypes.COMPLETE.ToString();

            var currentProperties = _managementFilePropertyRepository.GetPropertiesByManagementFileId(incomingManagementFile.Internal_Id);

            // The following checks result in hard STOP errors
            if (isFileClosing && currentProperties is not null && currentProperties.Any(p => !p.Property.IsOwned))
            {
                throw new BusinessRuleViolationException("You have one or more properties attached to this Management file that is NOT in the \"Core Inventory\" (i.e. owned by BCTFA and/or HMK). To complete this file you must either, remove these non \"Non-Core Inventory\" properties, OR make sure the property is added to the PIMS inventory first.");
            }

            ValidateStaff(incomingManagementFile);
        }

        private void AddNoteIfStatusChanged(PimsManagementFile updateManagementFile)
        {
            var currentManagementFile = _managementFileRepository.GetById(updateManagementFile.Internal_Id);
            bool statusChanged = currentManagementFile.ManagementFileStatusTypeCode != updateManagementFile.ManagementFileStatusTypeCode;
            if (!statusChanged)
            {
                return;
            }

            var newStatus = _lookupRepository.GetAllManagementFileStatusTypes()
                .FirstOrDefault(x => x.ManagementFileStatusTypeCode == updateManagementFile.ManagementFileStatusTypeCode);

            PimsManagementFileNote fileNoteInstance = new()
            {
                ManagementFileId = updateManagementFile.Internal_Id,
                AppCreateTimestamp = DateTime.Now,
                AppCreateUserid = _user.GetUsername(),
                Note = new PimsNote()
                {
                    IsSystemGenerated = true,
                    NoteTxt = $"Management File status changed from {currentManagementFile.ManagementFileStatusTypeCodeNavigation.Description} to {newStatus.Description}",
                    AppCreateTimestamp = DateTime.Now,
                    AppCreateUserid = this._user.GetUsername(),
                },
            };

            _entityNoteRepository.AddNoteRelationship(fileNoteInstance);
        }

        private void AddPropertyActiveNote(PimsManagementFileProperty updateManagementFileProperty)
        {
            string disabledOrEnabled = updateManagementFileProperty.IsActive ? "Enabled" : "Disabled";
            PimsManagementFileNote fileNoteInstance = new()
            {
                ManagementFileId = updateManagementFileProperty.ManagementFileId,
                AppCreateTimestamp = DateTime.Now,
                AppCreateUserid = _user.GetUsername(),
                Note = new PimsNote()
                {
                    IsSystemGenerated = true,
                    NoteTxt = $"Management File property {(updateManagementFileProperty as IFilePropertyEntity).GetPropertyName()} {disabledOrEnabled}",
                    AppCreateTimestamp = DateTime.Now,
                    AppCreateUserid = this._user.GetUsername(),
                },
            };

            _entityNoteRepository.AddNoteRelationship(fileNoteInstance);
        }

        private void MatchProperties(PimsManagementFile managementFile, IEnumerable<UserOverrideCode> overrideCodes)
        {
            foreach (var managementProperty in managementFile.PimsManagementFileProperties)
            {
                if (managementProperty.Property.Pid.HasValue)
                {
                    var pid = managementProperty.Property.Pid.Value;
                    try
                    {
                        var foundProperty = _propertyRepository.GetByPid(pid, true);
                        if (foundProperty.IsRetired.HasValue && foundProperty.IsRetired.Value)
                        {
                            throw new BusinessRuleViolationException("Retired property can not be selected.");
                        }

                        managementProperty.PropertyId = foundProperty.Internal_Id;
                        _propertyService.UpdateLocation(managementProperty.Property, ref foundProperty, overrideCodes);
                        managementProperty.Property = foundProperty;
                    }
                    catch (KeyNotFoundException)
                    {
                        if (overrideCodes.Contains(UserOverrideCode.ManagingPropertyNotInventoried))
                        {
                            _logger.LogDebug("Adding new property with pid:{pid}", pid);
                            managementProperty.Property = _propertyService.PopulateNewProperty(managementProperty.Property, false, false);
                        }
                        else
                        {
                            throw new UserOverrideException(UserOverrideCode.ManagingPropertyNotInventoried, "You have added one or more properties to the management file that are not in the MOTT Inventory. To acquire these properties, add them to an acquisition file. Do you want to proceed?");
                        }
                    }
                }
                else if (managementProperty.Property.Pin.HasValue)
                {
                    var pin = managementProperty.Property.Pin.Value;
                    try
                    {
                        var foundProperty = _propertyRepository.GetByPin(pin, true);
                        if (foundProperty.IsRetired.HasValue && foundProperty.IsRetired.Value)
                        {
                            throw new BusinessRuleViolationException("Retired property can not be selected.");
                        }

                        managementProperty.PropertyId = foundProperty.Internal_Id;
                        _propertyService.UpdateLocation(managementProperty.Property, ref foundProperty, overrideCodes);
                        managementProperty.Property = foundProperty;
                    }
                    catch (KeyNotFoundException)
                    {
                        if (overrideCodes.Contains(UserOverrideCode.DisposingPropertyNotInventoried))
                        {
                            _logger.LogDebug("Adding new property with pin:{pin}", pin);
                            managementProperty.Property = _propertyService.PopulateNewProperty(managementProperty.Property, false, false);
                        }
                        else
                        {
                            throw new UserOverrideException(UserOverrideCode.DisposingPropertyNotInventoried, "You have added one or more properties to the management file that are not in the MOTT Inventory. To acquire these properties, add them to an acquisition file. Do you want to proceed?");
                        }
                    }
                }
                else
                {
                    if (overrideCodes.Contains(UserOverrideCode.DisposingPropertyNotInventoried))
                    {
                        _logger.LogDebug("Adding new property without a pid");
                        managementProperty.Property = _propertyService.PopulateNewProperty(managementProperty.Property, false, false);
                    }
                    else
                    {
                        throw new UserOverrideException(UserOverrideCode.DisposingPropertyNotInventoried, "You have added one or more properties to the management file that are not in the MOTT Inventory. To acquire these properties, add them to an acquisition file. Do you want to proceed?");
                    }
                }
            }
        }

        private void ValidateVersion(long managementFileId, long managementFileVersion)
        {
            long currentRowVersion = _managementFileRepository.GetRowVersion(managementFileId);
            if (currentRowVersion != managementFileVersion)
            {
                throw new DbUpdateConcurrencyException("You are working with an older version of this management file, please refresh the application and retry.");
            }
        }

        private ManagementFileStatusTypes? GetCurrentManagementStatus(long managementFileId)
        {
            var currentManagementFile = _managementFileRepository.GetById(managementFileId);
            ManagementFileStatusTypes currentManagementFileStatus;

            if (Enum.TryParse(currentManagementFile.ManagementFileStatusTypeCode, out currentManagementFileStatus))
            {
                return currentManagementFileStatus;
            }

            return currentManagementFileStatus;
        }

    }
}
