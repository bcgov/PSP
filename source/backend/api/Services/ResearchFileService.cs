using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;

namespace Pims.Api.Services
{
    public class ResearchFileService : IResearchFileService
    {
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly IResearchFileRepository _researchFileRepository;
        private readonly IResearchFilePropertyRepository _researchFilePropertyRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly ILookupRepository _lookupRepository;
        private readonly INoteRelationshipRepository<PimsResearchFileNote> _entityNoteRepository;
        private readonly IPropertyService _propertyService;
        private readonly IPropertyOperationService _propertyOperationService;
        private readonly IResearchStatusSolver _researchStatusSolver;

        public ResearchFileService(
            ClaimsPrincipal user,
            ILogger<ResearchFileService> logger,
            IResearchFileRepository researchFileRepository,
            IResearchFilePropertyRepository researchFilePropertyRepository,
            IPropertyRepository propertyRepository,
            ILookupRepository lookupRepository,
            INoteRelationshipRepository<PimsResearchFileNote> entityNoteRepository,
            IPropertyService propertyService,
            IPropertyOperationService propertyOperationService,
            IResearchStatusSolver researchStatusSolver)
        {
            _user = user;
            _logger = logger;
            _researchFileRepository = researchFileRepository;
            _researchFilePropertyRepository = researchFilePropertyRepository;
            _propertyRepository = propertyRepository;
            _lookupRepository = lookupRepository;
            _entityNoteRepository = entityNoteRepository;
            _propertyService = propertyService;
            _propertyOperationService = propertyOperationService;
            _researchStatusSolver = researchStatusSolver;
        }

        public PimsResearchFile GetById(long id)
        {
            _logger.LogInformation("Getting research file with id {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.ResearchFileView);

            var researchFile = _researchFileRepository.GetById(id);

            return researchFile;
        }

        public IEnumerable<PimsPropertyResearchFile> GetProperties(long researchFileId)
        {
            _logger.LogInformation("Getting research file properties for file with id {id}", researchFileId);
            _user.ThrowIfNotAuthorized(Permissions.ResearchFileView);
            _user.ThrowIfNotAuthorized(Permissions.PropertyView);

            var propertyResearchFiles = _researchFilePropertyRepository.GetAllByResearchFileId(researchFileId);
            return _propertyService.TransformAllPropertiesToLatLong(propertyResearchFiles);
        }

        public PimsResearchFile Add(PimsResearchFile researchFile, IEnumerable<UserOverrideCode> userOverrideCodes)
        {
            _logger.LogInformation("Adding research file...");
            _user.ThrowIfNotAuthorized(Permissions.ResearchFileAdd);
            researchFile.ResearchFileStatusTypeCode = "ACTIVE";

            MatchProperties(researchFile, userOverrideCodes);

            // Update marker locations in the context of this file
            foreach (var incomingResearchProperty in researchFile.PimsPropertyResearchFiles)
            {
                _propertyService.PopulateNewFileProperty(incomingResearchProperty);
            }

            var newResearchFile = _researchFileRepository.Add(researchFile);
            _researchFileRepository.CommitTransaction();
            return newResearchFile;
        }

        public PimsResearchFile Update(PimsResearchFile researchFile)
        {
            researchFile.ThrowIfNull(nameof(researchFile));

            _logger.LogInformation("Updating research file with id {id}", researchFile.Internal_Id);
            _user.ThrowIfNotAuthorized(Permissions.ResearchFileEdit);
            ValidateVersion(researchFile.Internal_Id, researchFile.ConcurrencyControlNumber);

            var currentResearchFile = _researchFileRepository.GetById(researchFile.ResearchFileId);
            var currentResearchStatus = _researchStatusSolver.GetCurrentResearchStatus(currentResearchFile?.ResearchFileStatusTypeCode);
            if (!_researchStatusSolver.CanEditDetails(currentResearchStatus) && !_user.HasPermission(Permissions.SystemAdmin))
            {
                throw new BusinessRuleViolationException("The file you are editing is not active, so you cannot save changes. Refresh your browser to see file state.");
            }

            var newResearchFile = _researchFileRepository.Update(researchFile);
            AddNoteIfStatusChanged(newResearchFile);

            _researchFileRepository.CommitTransaction();
            return newResearchFile;
        }

        public PimsResearchFile UpdateProperties(PimsResearchFile researchFile, IEnumerable<UserOverrideCode> userOverrideCodes)
        {
            _logger.LogInformation("Updating research file properties with ResearchFile id: {id}", researchFile.Internal_Id);
            _user.ThrowIfNotAllAuthorized(Permissions.ResearchFileEdit, Permissions.PropertyView, Permissions.PropertyAdd);

            ValidateVersion(researchFile.Internal_Id, researchFile.ConcurrencyControlNumber);

            var currentResearchFile = _researchFileRepository.GetById(researchFile.ResearchFileId);
            var currentResearchStatus = _researchStatusSolver.GetCurrentResearchStatus(currentResearchFile?.ResearchFileStatusTypeCode);
            if (!_researchStatusSolver.CanEditProperties(currentResearchStatus))
            {
                throw new BusinessRuleViolationException("The file you are editing is not active, so you cannot save changes. Refresh your browser to see file state.");
            }

            MatchProperties(researchFile, userOverrideCodes);

            // Get the current properties in the research file
            var currentFileProperties = _researchFilePropertyRepository.GetAllByResearchFileId(researchFile.Internal_Id);

            // Check if the property is new or if it is being updated
            foreach (var incomingResearchProperty in researchFile.PimsPropertyResearchFiles)
            {
                var matchingProperty = currentFileProperties.FirstOrDefault(c => c.PropertyId == incomingResearchProperty.PropertyId);
                if (matchingProperty is not null && incomingResearchProperty.Internal_Id == 0)
                {
                    incomingResearchProperty.Internal_Id = matchingProperty.Internal_Id;
                }

                // If the property is not new, check if the name has been updated.
                if (incomingResearchProperty.Internal_Id != 0)
                {
                    var needsUpdate = false;
                    PimsPropertyResearchFile existingProperty = currentFileProperties.FirstOrDefault(x => x.Internal_Id == incomingResearchProperty.Internal_Id);
                    if (existingProperty.PropertyName != incomingResearchProperty.PropertyName)
                    {
                        existingProperty.PropertyName = incomingResearchProperty.PropertyName;
                        needsUpdate = true;
                    }
                    if (existingProperty.DisplayOrder != incomingResearchProperty.DisplayOrder)
                    {
                        existingProperty.DisplayOrder = incomingResearchProperty.DisplayOrder;
                        needsUpdate = true;
                    }

                    var incomingGeom = incomingResearchProperty.Location;
                    var existingGeom = existingProperty.Location;
                    if (existingGeom is null || (incomingGeom is not null && !existingGeom.EqualsExact(incomingGeom)))
                    {
                        _propertyService.UpdateFilePropertyLocation(incomingResearchProperty, existingProperty);
                        needsUpdate = true;
                    }

                    if (needsUpdate)
                    {
                        _researchFilePropertyRepository.Update(existingProperty);
                    }
                }
                else
                {
                    // New property needs to be added
                    var newFileProperty = _propertyService.PopulateNewFileProperty(incomingResearchProperty);
                    _researchFilePropertyRepository.Add(newFileProperty);
                }
            }

            // The ones not on the new set should be deleted
            List<PimsPropertyResearchFile> differenceSet = currentFileProperties.Where(x => !researchFile.PimsPropertyResearchFiles.Any(y => y.Internal_Id == x.Internal_Id)).ToList();
            foreach (var deletedProperty in differenceSet)
            {
                if (_propertyOperationService.GetOperationsForProperty(deletedProperty.PropertyId).Count > 0)
                {
                    throw new BusinessRuleViolationException("This property cannot be deleted because it is part of a subdivision or consolidation");
                }

                _researchFilePropertyRepository.Delete(deletedProperty);
                var totalAssociationCount = _propertyRepository.GetAllAssociationsCountById(deletedProperty.PropertyId);
                if (totalAssociationCount <= 1)
                {
                    _researchFilePropertyRepository.CommitTransaction(); // TODO: this can only be removed if cascade deletes are implemented. EF executes deletes in alphabetic order.
                    _propertyRepository.Delete(deletedProperty.Property);
                }
            }

            _researchFilePropertyRepository.CommitTransaction();
            return _researchFileRepository.GetById(researchFile.Internal_Id);
        }

        public Paged<PimsResearchFile> GetPage(ResearchFilter filter)
        {
            _logger.LogInformation("Searching for research files...");

            _logger.LogDebug("Research file search with filter {filter}", filter.Serialize());
            _user.ThrowIfNotAuthorized(Permissions.ResearchFileView);

            return _researchFileRepository.GetPage(filter);
        }

        public PimsResearchFile UpdateProperty(long researchFileId, long? researchFileVersion, PimsPropertyResearchFile propertyResearchFile)
        {
            _logger.LogInformation("Updating property research file...");
            _user.ThrowIfNotAuthorized(Permissions.ResearchFileEdit);
            ValidateVersion(researchFileId, researchFileVersion);

            var currentResearchFile = _researchFileRepository.GetById(researchFileId);
            var currentResearchStatus = _researchStatusSolver.GetCurrentResearchStatus(currentResearchFile?.ResearchFileStatusTypeCode);
            if (!_researchStatusSolver.CanEditProperties(currentResearchStatus))
            {
                throw new BusinessRuleViolationException("The file you are editing is not active, so you cannot save changes. Refresh your browser to see file state.");
            }

            _researchFilePropertyRepository.Update(propertyResearchFile);
            _researchFilePropertyRepository.CommitTransaction();
            return _researchFileRepository.GetById(researchFileId);
        }

        public LastUpdatedByModel GetLastUpdateInformation(long researchFileId)
        {
            _logger.LogInformation("Retrieving last updated-by information...");
            _user.ThrowIfNotAuthorized(Permissions.ResearchFileView);

            return _researchFileRepository.GetLastUpdateBy(researchFileId);
        }

        private void MatchProperties(PimsResearchFile researchFile, IEnumerable<UserOverrideCode> userOverrideCodes)
        {
            foreach (var researchProperty in researchFile.PimsPropertyResearchFiles)
            {
                if (researchProperty.Property.Pid.HasValue)
                {
                    var pid = researchProperty.Property.Pid.Value;
                    try
                    {
                        var foundProperty = _propertyRepository.GetByPid(pid, true);
                        researchProperty.PropertyId = foundProperty.Internal_Id;
                        _propertyService.UpdateLocation(researchProperty.Property, ref foundProperty, userOverrideCodes, allowRetired: true);
                        researchProperty.Property = foundProperty;
                    }
                    catch (KeyNotFoundException)
                    {
                        _logger.LogDebug("Adding new property with pid:{pid}", pid);
                        researchProperty.Property = _propertyService.PopulateNewProperty(researchProperty.Property);
                    }
                }
                else if (researchProperty.Property.Pin.HasValue)
                {
                    var pin = researchProperty.Property.Pin.Value;
                    try
                    {
                        var foundProperty = _propertyRepository.GetByPin(pin, true);
                        researchProperty.PropertyId = foundProperty.Internal_Id;
                        _propertyService.UpdateLocation(researchProperty.Property, ref foundProperty, userOverrideCodes, allowRetired: true);
                        researchProperty.Property = foundProperty;
                    }
                    catch (KeyNotFoundException)
                    {
                        _logger.LogDebug("Adding new property with pin:{pin}", pin);
                        researchProperty.Property = _propertyService.PopulateNewProperty(researchProperty.Property);
                    }
                }
                else
                {
                    _logger.LogDebug("Adding new property without a pid");
                    researchProperty.Property = _propertyService.PopulateNewProperty(researchProperty.Property);
                }
            }
        }

        private void ValidateVersion(long researchFileId, long? researchFileVersion)
        {
            long currentRowVersion = _researchFileRepository.GetRowVersion(researchFileId);
            if (currentRowVersion != researchFileVersion)
            {
                throw new DbUpdateConcurrencyException("You are working with an older version of this research file, please refresh the application and retry.");
            }
        }

        private void AddNoteIfStatusChanged(PimsResearchFile updateResearchFile)
        {
            var currentResearchFile = _researchFileRepository.GetById(updateResearchFile.Internal_Id);
            bool statusChanged = currentResearchFile.ResearchFileStatusTypeCode != updateResearchFile.ResearchFileStatusTypeCode;
            if (!statusChanged)
            {
                return;
            }

            var newStatus = _lookupRepository.GetAllResearchFileStatusTypes()
                .FirstOrDefault(x => x.ResearchFileStatusTypeCode == updateResearchFile.ResearchFileStatusTypeCode);

            PimsResearchFileNote fileNoteInstance = new()
            {
                ResearchFileId = updateResearchFile.Internal_Id,
                AppCreateTimestamp = DateTime.Now,
                AppCreateUserid = _user.GetUsername(),
                Note = new PimsNote()
                {
                    IsSystemGenerated = true,
                    NoteTxt = $"Research File status changed from {currentResearchFile.ResearchFileStatusTypeCodeNavigation.Description} to {newStatus.Description}",
                    AppCreateTimestamp = DateTime.Now,
                    AppCreateUserid = this._user.GetUsername(),
                },
            };

            _entityNoteRepository.AddNoteRelationship(fileNoteInstance);
        }
    }
}
