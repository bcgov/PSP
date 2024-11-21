using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.CodeTypes;
using Pims.Core.Exceptions;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Core.Extensions;
using Pims.Dal.Repositories;
using Pims.Core.Security;

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

        public bool DeleteAcquisitionPropertyTake(long takeId, IEnumerable<UserOverrideCode> userOverrides)
        {
            _logger.LogInformation("deleting take with {takeId}", takeId);
            _user.ThrowIfNotAuthorized(Permissions.PropertyView, Permissions.AcquisitionFileView);

            var takeToDelete = _takeRepository.GetById(takeId);
            var propertyWithAssociations = _propertyRepository.GetAllAssociationsById(takeToDelete.PropertyAcquisitionFile.PropertyId);
            var currentAcquisitionFile = _acqFileRepository.GetByAcquisitionFilePropertyId(takeToDelete.PropertyAcquisitionFileId);
            var allTakesForProperty = _takeRepository.GetAllByPropertyId(takeToDelete.PropertyAcquisitionFile.PropertyId);
            if ((!_statusSolver.CanEditTakes(Enum.Parse<AcquisitionStatusTypes>(currentAcquisitionFile.AcquisitionFileStatusTypeCode)) || takeToDelete.TakeStatusTypeCode == AcquisitionTakeStatusTypes.COMPLETE.ToString())
                && !_user.HasPermission(Permissions.SystemAdmin))
            {
                throw new BusinessRuleViolationException("Retired records are referenced for historical purposes only and cannot be edited or deleted. If the take has been added in error, contact your system administrator to re-open the file, which will allow take deletion.");
            }
            else if (propertyWithAssociations?.PimsDispositionFileProperties?.Any(d => d.DispositionFile.DispositionFileStatusTypeCode == DispositionFileStatusTypes.COMPLETE.ToString()) == true)
            {
                throw new BusinessRuleViolationException("You cannot delete a take that has a completed disposition attached to the same property.");
            }
            else if (propertyWithAssociations?.IsRetired == true)
            {
                throw new BusinessRuleViolationException("You cannot delete a take from a retired property.");
            }

            // user overrides
            if (takeToDelete.TakeStatusTypeCode == AcquisitionTakeStatusTypes.COMPLETE.ToString() && _user.HasPermission(Permissions.SystemAdmin))
            {
                if (propertyWithAssociations?.PimsDispositionFileProperties?.Any(d => d.DispositionFile.DispositionFileStatusTypeCode == DispositionFileStatusTypes.ACTIVE.ToString()) == true && !userOverrides.Contains(UserOverrideCode.DeleteTakeActiveDisposition))
                {
                    throw new UserOverrideException(UserOverrideCode.DeleteTakeActiveDisposition, "You are deleting a take. Property ownership state will be recalculated based upon any remaining completed takes. It should be noted that one or more related dispositions are in progress that should also be reviewed. \n\nDo you want to acknowledge and proceed?");
                }
                else if (allTakesForProperty.Count(t => !IsTakeExpired(t) && t.TakeStatusTypeCode == AcquisitionTakeStatusTypes.COMPLETE.ToString()) == 1 && allTakesForProperty.FirstOrDefault().TakeId == takeId && !userOverrides.Contains(UserOverrideCode.DeleteLastTake))
                {
                    throw new UserOverrideException(UserOverrideCode.DeleteLastTake, "You are deleting the last non-expired completed take on this property. This property will become a property of interest.\n\nDo you want to acknowledge and proceed?");
                }
                else if (!userOverrides.Contains(UserOverrideCode.DeleteCompletedTake) && !userOverrides.Contains(UserOverrideCode.DeleteLastTake) && !userOverrides.Contains(UserOverrideCode.DeleteTakeActiveDisposition))
                {
                    throw new UserOverrideException(UserOverrideCode.DeleteCompletedTake, "You are deleting a completed take. Property ownership state will be recalculated based upon any remaining completed takes.\n\nDo you want to acknowledge and proceed?");
                }
            }

            var wasTakeDeleted = _takeRepository.TryDeleteTake(takeId);

            if (wasTakeDeleted)
            {
                // Evaluate if the property needs to be updated
                var currentProperty = _acqFileRepository.GetProperty(takeToDelete.PropertyAcquisitionFileId);
                var currentTakes = _takeRepository.GetAllByPropertyId(currentProperty.PropertyId);

                var completedTakes = currentTakes
                    .Where(x => x.TakeStatusTypeCode == AcquisitionTakeStatusTypes.COMPLETE.ToString() && x.TakeId != takeId).ToList();

                if (completedTakes.Count > 0 || takeToDelete.TakeStatusTypeCode == AcquisitionTakeStatusTypes.COMPLETE.ToString())
                {
                    if (_takeInteractionSolver.ResultsInOwnedProperty(completedTakes))
                    {
                        _propertyRepository.TransferFileProperty(currentProperty, true);
                    }
                    else
                    {
                        _propertyRepository.TransferFileProperty(currentProperty, false);
                    }
                }
            }

            _takeRepository.CommitTransaction();
            return wasTakeDeleted;
        }

        private static bool IsTakeExpired(PimsTake take)
        {
            return (take.IsActiveLease && take.ActiveLeaseEndDt > DateOnly.FromDateTime(DateTime.Now))
                || (take.IsNewLandAct && take.LandActEndDt > DateOnly.FromDateTime(DateTime.Now))
                || (take.IsNewLicenseToConstruct && take.LtcEndDt > DateOnly.FromDateTime(DateTime.Now))
                || (take.IsNewInterestInSrw && take.SrwEndDt > DateOnly.FromDateTime(DateTime.Now));
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

            // If the take is being modified, update the one in the list to use the incoming one.
            var existingTake = allTakes.FirstOrDefault(t => t.TakeId == take.TakeId);
            if (existingTake != null)
            {
                allTakes = allTakes.Select(t => t.TakeId == take.TakeId ? take : t);
            }
            else if (take != null)
            {
                allTakes = allTakes.Append(take);
            }

            var completedTakes = allTakes
                .Where(x => x.TakeStatusTypeCode == AcquisitionTakeStatusTypes.COMPLETE.ToString()).ToList();

            if (completedTakes.Count > 0)
            {
                if (_takeInteractionSolver.ResultsInOwnedProperty(completedTakes))
                {
                    _propertyRepository.TransferFileProperty(currentProperty, true);
                }
                else
                {
                    _propertyRepository.TransferFileProperty(currentProperty, false);
                }
            }
        }
    }
}
