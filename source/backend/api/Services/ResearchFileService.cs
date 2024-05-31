using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
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
        private readonly ILookupRepository _lookupRepository;
        private readonly IEntityNoteRepository _entityNoteRepository;
        private readonly IPropertyService _propertyService;

        public ResearchFileService(
            ClaimsPrincipal user,
            ILogger<ResearchFileService> logger,
            IResearchFileRepository researchFileRepository,
            IResearchFilePropertyRepository researchFilePropertyRepository,
            IPropertyRepository propertyRepository,
            ICoordinateTransformService coordinateService,
            ILookupRepository lookupRepository,
            IEntityNoteRepository entityNoteRepository,
            IPropertyService propertyService)
        {
            _user = user;
            _logger = logger;
            _researchFileRepository = researchFileRepository;
            _researchFilePropertyRepository = researchFilePropertyRepository;
            _propertyRepository = propertyRepository;
            _lookupRepository = lookupRepository;
            _entityNoteRepository = entityNoteRepository;
            _propertyService = propertyService;
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

            var newResearchFile = _researchFileRepository.Update(researchFile);
            AddNoteIfStatusChanged(newResearchFile);

            _researchFileRepository.CommitTransaction();
            return newResearchFile;
        }

        public PimsResearchFile UpdateProperties(PimsResearchFile researchFile, IEnumerable<UserOverrideCode> userOverrideCodes)
        {
            _logger.LogInformation("Updating research file properties...");
            _user.ThrowIfNotAuthorized(Permissions.ResearchFileEdit);
            ValidateVersion(researchFile.Internal_Id, researchFile.ConcurrencyControlNumber);

            MatchProperties(researchFile, userOverrideCodes);

            // Get the current properties in the research file
            var currentProperties = _researchFilePropertyRepository.GetAllByResearchFileId(researchFile.Internal_Id);

            // Check if the property is new or if it is being updated
            foreach (var incomingResearchProperty in researchFile.PimsPropertyResearchFiles)
            {
                // If the property is not new, check if the name has been updated.
                if (incomingResearchProperty.Internal_Id != 0)
                {
                    PimsPropertyResearchFile existingProperty = currentProperties.FirstOrDefault(x => x.Internal_Id == incomingResearchProperty.Internal_Id);
                    if (existingProperty.PropertyName != incomingResearchProperty.PropertyName)
                    {
                        existingProperty.PropertyName = incomingResearchProperty.PropertyName;
                        _researchFilePropertyRepository.Update(existingProperty);
                    }
                }
                else
                {
                    // New property needs to be added
                    _researchFilePropertyRepository.Add(incomingResearchProperty);
                }
            }

            // The ones not on the new set should be deleted
            List<PimsPropertyResearchFile> differenceSet = currentProperties.Where(x => !researchFile.PimsPropertyResearchFiles.Any(y => y.Internal_Id == x.Internal_Id)).ToList();
            foreach (var deletedProperty in differenceSet)
            {
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

            _logger.LogDebug("Research file search with filter", filter);
            _user.ThrowIfNotAuthorized(Permissions.ResearchFileView);

            return _researchFileRepository.GetPage(filter);
        }

        public PimsResearchFile UpdateProperty(long researchFileId, long? researchFileVersion, PimsPropertyResearchFile propertyResearchFile)
        {
            _logger.LogInformation("Updating property research file...");
            _user.ThrowIfNotAuthorized(Permissions.ResearchFileEdit);
            ValidateVersion(researchFileId, researchFileVersion);

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
                        _propertyService.UpdateLocation(researchProperty.Property, ref foundProperty, userOverrideCodes);
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
                        _propertyService.UpdateLocation(researchProperty.Property, ref foundProperty, userOverrideCodes);
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

            _entityNoteRepository.Add(fileNoteInstance);
        }
    }
}
