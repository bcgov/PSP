using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models.CodeTypes;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Api.Services
{
    public class CompensationRequisitionService : ICompensationRequisitionService
    {
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly ICompensationRequisitionRepository _compensationRequisitionRepository;
        private readonly IEntityNoteRepository _entityNoteRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAcquisitionFileRepository _acqFileRepository;
        private readonly ICompReqFinancialService _compReqFinancialService;
        private readonly IAcquisitionStatusSolver _acquisitionStatusSolver;
        private readonly ILeaseRepository _leaseRepository;

        public CompensationRequisitionService(
            ClaimsPrincipal user,
            ILogger<CompensationRequisitionService> logger,
            ICompensationRequisitionRepository compensationRequisitionRepository,
            IEntityNoteRepository entityNoteRepository,
            IUserRepository userRepository,
            IAcquisitionFileRepository acqFileRepository,
            ICompReqFinancialService compReqFinancialService,
            IAcquisitionStatusSolver statusSolver,
            ILeaseRepository leaseRepository)
        {
            _user = user;
            _logger = logger;
            _compensationRequisitionRepository = compensationRequisitionRepository;
            _entityNoteRepository = entityNoteRepository;
            _userRepository = userRepository;
            _acqFileRepository = acqFileRepository;
            _compReqFinancialService = compReqFinancialService;
            _acquisitionStatusSolver = statusSolver;
            _leaseRepository = leaseRepository;
        }

        public PimsCompensationRequisition GetById(long compensationRequisitionId)
        {
            _logger.LogInformation($"Getting Compensation Requisition with id {compensationRequisitionId}");
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView);

            return _compensationRequisitionRepository.GetById(compensationRequisitionId);
        }

        public PimsCompensationRequisition AddCompensationRequisition(FileTypes fileType, PimsCompensationRequisition compensationRequisition)
        {
            PimsCompensationRequisition newCompensationRequisition = fileType switch
            {
                FileTypes.Acquisition => AddAcquisitionFileCompReq(compensationRequisition),
                FileTypes.Lease => AddLeaseFileCompReq(compensationRequisition),
                FileTypes.Disposition => throw new BadRequestException("Relationship type not valid."),
                FileTypes.Research => throw new BadRequestException("Relationship type not valid."),
                _ => throw new BadRequestException("Relationship type not valid."),
            };

            return newCompensationRequisition;
        }

        public PimsCompensationRequisition Update(PimsCompensationRequisition compensationRequisition)
        {
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionEdit);
            compensationRequisition.ThrowIfNull(nameof(compensationRequisition));

            if (compensationRequisition.AcquisitionFileId is not null)
            {
                _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, (long)compensationRequisition.AcquisitionFileId);
                _logger.LogInformation($"Updating Compensation Requisition with id ${compensationRequisition.CompensationRequisitionId}");

                return UpdateAcquisitionFileCompensation(compensationRequisition);
            }

            return UpdateLeaseFileCompensation(compensationRequisition);
        }

        public bool DeleteCompensation(long compensationId)
        {
            _logger.LogInformation("Deleting compensation with id: {compensationId}", compensationId);
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionDelete, Permissions.AcquisitionFileEdit);

            var currentCompensation = _compensationRequisitionRepository.GetById(compensationId);

            if (currentCompensation.AcquisitionFileId is not null)
            {
                var currentAcquisitionStatus = GetCurrentAcquisitionStatus((long)currentCompensation.AcquisitionFileId);

                if (!_acquisitionStatusSolver.CanEditOrDeleteCompensation(currentAcquisitionStatus, currentCompensation.IsDraft) && !_user.HasPermission(Permissions.SystemAdmin))
                {
                    throw new BusinessRuleViolationException("The file you are editing is not active or hold, so you cannot save changes. Refresh your browser to see file state.");
                }
            }

            var fileFormToDelete = _compensationRequisitionRepository.TryDelete(compensationId);
            _compensationRequisitionRepository.CommitTransaction();

            return fileFormToDelete;
        }

        public IEnumerable<PimsPropertyAcquisitionFile> GetAcquisitionProperties(long id)
        {
            _logger.LogInformation("Getting properties for Compensation Requisition with id {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView);

            return _compensationRequisitionRepository.GetAcquisitionCompReqPropertiesById(id);
        }

        public IEnumerable<PimsPropertyLease> GetLeaseProperties(long id)
        {
            _logger.LogInformation("Getting properties for Compensation Requisition with id {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView);

            return _compensationRequisitionRepository.GetLeaseCompReqPropertiesById(id);
        }

        public IEnumerable<PimsCompensationRequisition> GetFileCompensationRequisitions(FileTypes fileType, long fileId)
        {
            List<PimsCompensationRequisition> compReqs = fileType switch
            {
                FileTypes.Acquisition => GetAcquisitionFileCompReqs(fileId),
                FileTypes.Lease => GetLeaseFileCompReqs(fileId),
                FileTypes.Research => throw new BadRequestException("Relationship type not valid."),
                FileTypes.Disposition => throw new BadRequestException("Relationship type not valid."),
                _ => throw new BadRequestException("Relationship type not valid."),
            };

            return compReqs;
        }

        private static string GetCompensationRequisitionStatusText(bool? isDraft)
        {
            if (isDraft.HasValue)
            {
                return isDraft.Value ? "'Draft'" : "'Final'";
            }
            else
            {
                return "'No Status'";
            }
        }

        private static DateOnly? CheckFinalizedDate(bool? currentStatusIsDraft, bool? newStatusIsDraft, DateOnly? currentValue)
        {
            if (currentStatusIsDraft.Equals(newStatusIsDraft))
            {
                return currentValue;
            }

            if (newStatusIsDraft.HasValue)
            {
                return newStatusIsDraft.Value ? null : DateOnly.FromDateTime(DateTime.UtcNow);
            }

            return null;
        }

        private PimsCompensationRequisition AddAcquisitionFileCompReq(PimsCompensationRequisition compensationRequisition)
        {
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionAdd);

            compensationRequisition.ThrowIfNull(nameof(compensationRequisition));
            if (compensationRequisition.AcquisitionFileId is null)
            {
                throw new BadRequestException("Invalid acquisitionFileId.");
            }

            _logger.LogInformation("Adding compensation requisition for acquisition file id: {acquisitionFileId}", compensationRequisition.AcquisitionFileId);

            if (compensationRequisition.LeaseId is not null)
            {
                throw new BadRequestException("Compensation Requisition should have only one parent Id");
            }

            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, (long)compensationRequisition.AcquisitionFileId);

            _ = _acqFileRepository.GetById((long)compensationRequisition.AcquisitionFileId) ?? throw new BadRequestException("Invalid acquisitionFileId.");

            compensationRequisition.IsDraft ??= true;
            var newCompensationRequisition = _compensationRequisitionRepository.Add(compensationRequisition);
            _compensationRequisitionRepository.CommitTransaction();

            return newCompensationRequisition;
        }

        private PimsCompensationRequisition AddLeaseFileCompReq(PimsCompensationRequisition compensationRequisition)
        {
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionAdd);

            compensationRequisition.ThrowIfNull(nameof(compensationRequisition));
            if (compensationRequisition.LeaseId is null)
            {
                throw new BadRequestException("Invalid LeaseId.");
            }

            _logger.LogInformation("Adding compensation requisition for lease file id: {leaseId}", compensationRequisition.LeaseId);

            if (compensationRequisition.AcquisitionFileId is not null)
            {
                throw new BadRequestException("Compensation Requisition should have only one parent Id");
            }

            var pimsUser = _userRepository.GetByKeycloakUserId(_user.GetUserKey());
            var currentLease = _leaseRepository.GetNoTracking((long)compensationRequisition.LeaseId) ?? throw new BadRequestException("Invalid LeaseId.");
            pimsUser.ThrowInvalidAccessToLeaseFile(currentLease.RegionCode);

            compensationRequisition.IsDraft ??= true;
            var newCompensationRequisition = _compensationRequisitionRepository.Add(compensationRequisition);
            _compensationRequisitionRepository.CommitTransaction();

            return newCompensationRequisition;
        }

        private List<PimsCompensationRequisition> GetAcquisitionFileCompReqs(long fileId)
        {
            _logger.LogInformation("Getting compensations for acquisition file id: {acquisitionFileId}", fileId);
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView, Permissions.AcquisitionFileView);
            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, fileId);

            return _compensationRequisitionRepository.GetAllByAcquisitionFileId(fileId).ToList();
        }

        private List<PimsCompensationRequisition> GetLeaseFileCompReqs(long fileId)
        {
            _logger.LogInformation("Getting compensations for Lease file id: {LeaseId}", fileId);
            _user.ThrowIfNotAuthorized(Permissions.LeaseView);

            var pimsUser = _userRepository.GetByKeycloakUserId(_user.GetUserKey());
            pimsUser.ThrowInvalidAccessToLeaseFile(_leaseRepository.GetNoTracking(fileId).RegionCode);

            return _compensationRequisitionRepository.GetAllByLeaseFileId(fileId).ToList();
        }

        private PimsCompensationRequisition UpdateAcquisitionFileCompensation(PimsCompensationRequisition compensationRequisition)
        {
            var currentCompensation = _compensationRequisitionRepository.GetById(compensationRequisition.CompensationRequisitionId);

            var currentAcquisitionFile = _acqFileRepository.GetById((long)currentCompensation.AcquisitionFileId);
            var currentAcquisitionStatus = Enum.Parse<AcquisitionStatusTypes>(currentAcquisitionFile.AcquisitionFileStatusTypeCode);

            if (!_acquisitionStatusSolver.CanEditOrDeleteCompensation(currentAcquisitionStatus, currentCompensation.IsDraft) && !_user.HasPermission(Permissions.SystemAdmin))
            {
                throw new BusinessRuleViolationException("The file you are editing is not active or hold, so you cannot save changes. Refresh your browser to see file state.");
            }

            CheckTotalAllowableCompensation(currentAcquisitionFile, compensationRequisition);
            compensationRequisition.FinalizedDate = CheckFinalizedDate(currentCompensation.IsDraft, compensationRequisition.IsDraft, currentCompensation.FinalizedDate);

            PimsCompensationRequisition updatedEntity = _compensationRequisitionRepository.Update(compensationRequisition);
            AddNoteIfStatusChanged(compensationRequisition.Internal_Id, (long)compensationRequisition.AcquisitionFileId, currentCompensation.IsDraft, compensationRequisition.IsDraft);
            _compensationRequisitionRepository.CommitTransaction();

            return updatedEntity;
        }

        private PimsCompensationRequisition UpdateLeaseFileCompensation(PimsCompensationRequisition compensationRequisition)
        {
            var currentCompensation = _compensationRequisitionRepository.GetById(compensationRequisition.CompensationRequisitionId);

            PimsCompensationRequisition updatedEntity = _compensationRequisitionRepository.Update(compensationRequisition);
            AddNoteIfStatusChanged(compensationRequisition.Internal_Id, (long)compensationRequisition.AcquisitionFileId, currentCompensation.IsDraft, compensationRequisition.IsDraft);
            _compensationRequisitionRepository.CommitTransaction();

            return updatedEntity;
        }

        private void AddNoteIfStatusChanged(long compensationRequisitionId, long acquisitionFileId, bool? currentStatus, bool? newStatus)
        {
            if (currentStatus.Equals(newStatus))
            {
                return;
            }

            var curentStatusText = GetCompensationRequisitionStatusText(currentStatus);
            var newStatusText = GetCompensationRequisitionStatusText(newStatus);

            PimsAcquisitionFileNote fileNoteInstance = new()
            {
                AcquisitionFileId = acquisitionFileId,
                AppCreateTimestamp = DateTime.Now,
                AppCreateUserid = _user.GetUsername(),
                Note = new PimsNote()
                {
                    IsSystemGenerated = true,
                    NoteTxt = $"Compensation Requisition with # {compensationRequisitionId}, changed status from {curentStatusText} to {newStatusText}",
                    AppCreateTimestamp = DateTime.Now,
                    AppCreateUserid = this._user.GetUsername(),
                },
            };

            _entityNoteRepository.Add(fileNoteInstance);
        }

        private void CheckTotalAllowableCompensation(PimsAcquisitionFile currentAcquisitionFile, PimsCompensationRequisition newCompensation)
        {
            if (!currentAcquisitionFile.TotalAllowableCompensation.HasValue || (newCompensation.IsDraft.HasValue && newCompensation.IsDraft.Value))
            {
                return;
            }
            IEnumerable<PimsCompReqFinancial> allFinancialsForFile = _compReqFinancialService.GetAllByAcquisitionFileId(currentAcquisitionFile.AcquisitionFileId, true);
            IEnumerable<PimsCompReqFinancial> allUnchangedFinancialsForFile = allFinancialsForFile.Where(f => f.CompensationRequisitionId != newCompensation.Internal_Id);
            decimal newTotalCompensation = allUnchangedFinancialsForFile.Concat(newCompensation.PimsCompReqFinancials).Aggregate(0m, (acc, f) => acc + (f.TotalAmt ?? 0m));
            if (newTotalCompensation > currentAcquisitionFile.TotalAllowableCompensation)
            {
                throw new BusinessRuleViolationException("Your compensation requisition cannot be saved in FINAL status, as its compensation amount exceeds total allowable compensation for this file.");
            }
        }

        private AcquisitionStatusTypes GetCurrentAcquisitionStatus(long acquisitionFileId)
        {
            var currentAcquisitionFile = _acqFileRepository.GetById(acquisitionFileId);
            return Enum.Parse<AcquisitionStatusTypes>(currentAcquisitionFile.AcquisitionFileStatusTypeCode);
        }
    }
}
