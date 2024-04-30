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
        private readonly ITakeInteractionSolver _takeInteractionSolver;
        private readonly IPropertyRepository _propertyRepository;

        public TakeService(
            ClaimsPrincipal user,
            ILogger<AcquisitionFileService> logger,
            IAcquisitionFileRepository acqFileRepository,
            ITakeRepository repository,
            IAcquisitionStatusSolver statusSolver,
            ITakeInteractionSolver takeInteractionSolver,
            IPropertyRepository propertyRepository)
        {
            _user = user;
            _logger = logger;
            _acqFileRepository = acqFileRepository;
            _takeRepository = repository;
            _statusSolver = statusSolver;
            _takeInteractionSolver = takeInteractionSolver;
            _propertyRepository = propertyRepository;
        }

        public PimsTake GetById(long takeId)
        {
            _logger.LogInformation("Getting take with takeId {takeId}", takeId);
            _user.ThrowIfNotAuthorized(Permissions.PropertyView, Permissions.AcquisitionFileView);
            return _takeRepository.GetById(takeId);
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
            return _takeRepository.GetAllByAcqPropertyId(fileId, acquisitionFilePropertyId);
        }

        public int GetCountByPropertyId(long propertyId)
        {
            _logger.LogInformation("Getting take count with propertyId {fileId}", propertyId);
            _user.ThrowIfNotAuthorized(Permissions.PropertyView, Permissions.AcquisitionFileView);
            return _takeRepository.GetCountByPropertyId(propertyId);
        }

        public PimsTake AddAcquisitionPropertyTake(long acquisitionFilePropertyId, PimsTake take)
        {
            _logger.LogInformation("adding take with propertyFileId {propertyFileId}", acquisitionFilePropertyId);
            _user.ThrowIfNotAuthorized(Permissions.PropertyView, Permissions.AcquisitionFileView);

            ValidateTakeRules(acquisitionFilePropertyId, take);

            // Add take
            var addedTake = _takeRepository.AddTake(take);

            RecalculateOwnership(acquisitionFilePropertyId, take);

            _takeRepository.CommitTransaction();
            return addedTake;
        }

        public PimsTake UpdateAcquisitionPropertyTake(long acquisitionFilePropertyId, PimsTake take)
        {
            _logger.LogInformation("updating take with propertyFileId {propertyFileId}", acquisitionFilePropertyId);
            _user.ThrowIfNotAuthorized(Permissions.PropertyView, Permissions.AcquisitionFileView);

            ValidateTakeRules(acquisitionFilePropertyId, take);

            // Update take
            var updatedTake = _takeRepository.UpdateTake(take);

            RecalculateOwnership(acquisitionFilePropertyId, take);

            _takeRepository.CommitTransaction();
            return updatedTake;
        }

        public bool DeleteAcquisitionPropertyTake(long takeId)
        {
            _logger.LogInformation("deleting take with {takeId}", takeId);
            _user.ThrowIfNotAuthorized(Permissions.PropertyView, Permissions.AcquisitionFileView);

            var takeToDelete = _takeRepository.GetById(takeId);
            var currentAcquisitionFile = _acqFileRepository.GetByAcquisitionFilePropertyId(takeToDelete.PropertyAcquisitionFileId);
            if ((!_statusSolver.CanEditTakes(Enum.Parse<AcquisitionStatusTypes>(currentAcquisitionFile.AcquisitionFileStatusTypeCode)) || takeToDelete.TakeStatusTypeCode == AcquisitionTakeStatusTypes.COMPLETE.ToString())
                && !_user.HasPermission(Permissions.SystemAdmin))
            {
                throw new BusinessRuleViolationException("Retired records are referenced for historical purposes only and cannot be edited or deleted. If the take has been added in error, contact your system administrator to re-open the file, which will allow take deletion.");
            }
            var wasTakeDeleted = _takeRepository.TryDeleteTake(takeId);

            if (wasTakeDeleted)
            {
                RecalculateOwnership(takeToDelete.PropertyAcquisitionFileId, null);
            }

            _takeRepository.CommitTransaction();
            return wasTakeDeleted;
        }

        private void ValidateTakeRules(long acquisitionFilePropertyId, PimsTake take)
        {
            var currentFilePropertyTakes = _takeRepository.GetAllByPropertyAcquisitionFileId(acquisitionFilePropertyId);

            var currentAcquistionFile = _acqFileRepository.GetByAcquisitionFilePropertyId(acquisitionFilePropertyId);
            var currentAcquisitionStatus = Enum.Parse<AcquisitionStatusTypes>(currentAcquistionFile.AcquisitionFileStatusTypeCode);

            if (!_statusSolver.CanEditTakes(currentAcquisitionStatus) && !_user.HasPermission(Permissions.SystemAdmin))
            {
                throw new BusinessRuleViolationException("Retired records are referenced for historical purposes only and cannot be edited or deleted. If the take has been added in error, contact your system administrator to re-open the file, which will allow take deletion.");
            }
            else if (take.TakeStatusTypeCode == AcquisitionTakeStatusTypes.COMPLETE.ToString() && take.CompletionDt == null)
            {
                throw new BusinessRuleViolationException("A completed take must have a completion date.");
            }
            else if (take.IsNewLandAct && take.LandActEndDt != null && (take.LandActTypeCode == LandActTypes.CROWN_GRANT.ToString() || take.LandActTypeCode == LandActTypes.TRANSFER_OF_ADMIN_AND_CONTROL.ToString()))
            {
                throw new BusinessRuleViolationException("'Crown Grant' and 'Transfer' Land Acts cannot have an end date.");
            }
            else
            {
                // Complete Takes can only be set to InProgress by Admins when File is Active/Draft
                var currentCompleteTakes = currentFilePropertyTakes
                    .Where(x => x.TakeStatusTypeCode == AcquisitionTakeStatusTypes.COMPLETE.ToString()).ToList();

                if (currentCompleteTakes.Count > 0)
                {
                    var updatedTake = currentCompleteTakes.FirstOrDefault(x => x.TakeId == take.TakeId);
                    if (!_user.HasPermission(Permissions.SystemAdmin) && (updatedTake is not null && updatedTake.TakeStatusTypeCode != take.TakeStatusTypeCode))
                    {
                        throw new BusinessRuleViolationException("Retired records are referenced for historical purposes only and cannot be edited or deleted. If the take has been added in error, contact your system administrator to re-open the file, which will allow take deletion.");
                    }
                }
            }
        }

        private void RecalculateOwnership(long acquisitionFilePropertyId, PimsTake take)
        {
            // Evaluate if the property needs to be updated
            var currentProperty = _acqFileRepository.GetProperty(acquisitionFilePropertyId);
            var currentTakes = _takeRepository.GetAllByPropertyId(currentProperty.PropertyId);

            var allTakes = currentTakes;
            if (take != null)
            {
                allTakes = allTakes.Append(take);
            }

            var completedTakes = allTakes
                .Where(x => x.TakeStatusTypeCode == AcquisitionTakeStatusTypes.COMPLETE.ToString()).ToList();

            if (completedTakes.Count > 0 && _takeInteractionSolver.ResultsInOwnedProperty(completedTakes))
            {
                _propertyRepository.TransferFileProperty(currentProperty, true);
            }
        }
    }
}
