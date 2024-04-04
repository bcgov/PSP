using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Api.Constants;
using Pims.Api.Models.CodeTypes;
using Pims.Core.Exceptions;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Api.Services
{
    public class TakeService : ITakeService
    {
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly IAcquisitionFileRepository _acqFileRepository;
        private readonly ITakeRepository _takeRepository;
        private readonly IAcquisitionStatusSolver _statusSolver;

        public TakeService(
            ClaimsPrincipal user,
            ILogger<AcquisitionFileService> logger,
            IAcquisitionFileRepository acqFileRepository,
            ITakeRepository repository,
            IAcquisitionStatusSolver statusSolver)
        {
            _user = user;
            _logger = logger;
            _acqFileRepository = acqFileRepository;
            _takeRepository = repository;
            _statusSolver = statusSolver;
        }

        public IEnumerable<PimsTake> GetByFileId(long fileId)
        {
            _logger.LogInformation("Getting takes with fileId {fileId}", fileId);
            _user.ThrowIfNotAuthorized(Permissions.PropertyView, Permissions.AcquisitionFileView);
            return _takeRepository.GetAllByAcquisitionFileId(fileId);
        }

        public IEnumerable<PimsTake> GetByPropertyId(long fileId, long acquisitionFilePropertyId)
        {
            _logger.LogInformation($"Getting takes with fileId {fileId} and propertyId {acquisitionFilePropertyId}");
            _user.ThrowIfNotAuthorized(Permissions.PropertyView, Permissions.AcquisitionFileView);
            return _takeRepository.GetAllByPropertyId(fileId, acquisitionFilePropertyId);
        }

        public int GetCountByPropertyId(long propertyId)
        {
            _logger.LogInformation("Getting take count with propertyId {fileId}", propertyId);
            _user.ThrowIfNotAuthorized(Permissions.PropertyView, Permissions.AcquisitionFileView);
            return _takeRepository.GetCountByPropertyId(propertyId);
        }

        public IEnumerable<PimsTake> UpdateAcquisitionPropertyTakes(long acquisitionFilePropertyId, IEnumerable<PimsTake> takes)
        {
            _logger.LogInformation("updating takes with propertyFileId {propertyFileId}", acquisitionFilePropertyId);
            _user.ThrowIfNotAuthorized(Permissions.PropertyView, Permissions.AcquisitionFileView);

            var currentAcquistionFile = _acqFileRepository.GetByAcquisitionFilePropertyId(acquisitionFilePropertyId);
            var currentFilePropertyTakes = _takeRepository.GetAllByPropertyAcquisitionFileId(acquisitionFilePropertyId);

            var currentAcquisitionStatus = Enum.Parse<AcquisitionStatusTypes>(currentAcquistionFile.AcquisitionFileStatusTypeCode);

            // No user can update the takes when the File is not Active/Draft
            if (!_statusSolver.CanEditTakes(currentAcquisitionStatus))
            {
                throw new BusinessRuleViolationException("Retired records are referenced for historical purposes only and cannot be edited or deleted. If the take has been added in error, contact your system administrator to re-open the file, which will allow take deletion.");
            }
            else if (takes.Any(t => t.TakeStatusTypeCode == TakeStatusTypes.COMPLETE.ToString() && t.CompletionDt == null))
            {
                throw new BusinessRuleViolationException("A completed take must have a completion date.");
            }
            else
            {
                // Complete Takes can only be deleted or set to InProgress by Admins when File is Active/Draft
                if (!_user.HasPermission(Permissions.SystemAdmin))
                {
                    var currentCompleteTakes = currentFilePropertyTakes
                        .Where(x => x.TakeStatusTypeCode == AcquisitionTakeStatusTypes.COMPLETE.ToString()).ToList();

                    if (currentCompleteTakes.Count > 0)
                    {
                        foreach (var completeTake in currentCompleteTakes)
                        {
                            var updatedTake = takes.FirstOrDefault(x => x.TakeId == completeTake.TakeId);
                            if (updatedTake is null || updatedTake?.TakeStatusTypeCode != completeTake.TakeStatusTypeCode)
                            {
                                throw new BusinessRuleViolationException("Retired records are referenced for historical purposes only and cannot be edited or deleted. If the take has been added in error, contact your system administrator to re-open the file, which will allow take deletion.");
                            }
                        }
                    }
                }
            }

            _takeRepository.UpdateAcquisitionPropertyTakes(acquisitionFilePropertyId, takes);
            _takeRepository.CommitTransaction();

            return _takeRepository.GetAllByPropertyAcquisitionFileId(acquisitionFilePropertyId);
        }
    }
}
