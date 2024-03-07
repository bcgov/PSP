using System;
using System.Collections.Generic;
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

            var currentAcquisitionStatus = Enum.Parse<AcquisitionStatusTypes>(currentAcquistionFile.AcquisitionFileStatusTypeCode);
            if (!_statusSolver.CanEditTakes(currentAcquisitionStatus) && !_user.HasPermission(Permissions.SystemAdmin))
            {
                throw new BusinessRuleViolationException("The file you are editing is not active or draft, so you cannot save changes. Refresh your browser to see file state.");
            }

            _takeRepository.UpdateAcquisitionPropertyTakes(acquisitionFilePropertyId, takes);
            _takeRepository.CommitTransaction();

            return _takeRepository.GetAllByPropertyAcquisitionFileId(acquisitionFilePropertyId);
        }
    }
}
