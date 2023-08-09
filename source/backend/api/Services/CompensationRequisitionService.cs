using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
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

        public CompensationRequisitionService(ClaimsPrincipal user, ILogger<CompensationRequisitionService> logger, ICompensationRequisitionRepository compensationRequisitionRepository,
         IEntityNoteRepository entityNoteRepository, IUserRepository userRepository, IAcquisitionFileRepository acqFileRepository, ICompReqFinancialService compReqFinancialService)
        {
            _user = user;
            _logger = logger;
            _compensationRequisitionRepository = compensationRequisitionRepository;
            _entityNoteRepository = entityNoteRepository;
            _userRepository = userRepository;
            _acqFileRepository = acqFileRepository;
            _compReqFinancialService = compReqFinancialService;
        }

        public PimsCompensationRequisition GetById(long compensationRequisitionId)
        {
            _logger.LogInformation($"Getting Compensation Requisition with id {compensationRequisitionId}");
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView);

            return _compensationRequisitionRepository.GetById(compensationRequisitionId);
        }

        public PimsCompensationRequisition Update(PimsCompensationRequisition compensationRequisition)
        {
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionEdit);
            compensationRequisition.ThrowIfNull(nameof(compensationRequisition));
            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, compensationRequisition.AcquisitionFileId);

            _logger.LogInformation($"Updating Compensation Requisition with id ${compensationRequisition.CompensationRequisitionId}");

            var currentCompensation = _compensationRequisitionRepository.GetById(compensationRequisition.CompensationRequisitionId);

            CheckDraftStatusUpdateAuthorized(currentCompensation.IsDraft, compensationRequisition.IsDraft);
            CheckTotalAllowableCompensation(compensationRequisition.AcquisitionFileId, compensationRequisition);
            compensationRequisition.FinalizedDate = CheckFinalizedDate(currentCompensation.IsDraft, compensationRequisition.IsDraft, currentCompensation.FinalizedDate);

            PimsCompensationRequisition updatedEntity = _compensationRequisitionRepository.Update(compensationRequisition);
            AddNoteIfStatusChanged(compensationRequisition.Internal_Id, compensationRequisition.AcquisitionFileId, currentCompensation.IsDraft, compensationRequisition.IsDraft);
            _compensationRequisitionRepository.CommitTransaction();

            return updatedEntity;

            DateTime? CheckFinalizedDate(bool? currentStatusIsDraft, bool? newStatusIsDraft, DateTime? currentValue)
            {
                if (currentStatusIsDraft.Equals(newStatusIsDraft))
                {
                    return currentValue;
                }

                if (newStatusIsDraft.HasValue)
                {
                    return newStatusIsDraft.Value ? null : DateTime.UtcNow;
                }

                return null;
            }
        }

        public bool DeleteCompensation(long compensationId)
        {
            _logger.LogInformation("Deleting compensation with id ...", compensationId);
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionDelete, Permissions.AcquisitionFileEdit);

            var fileFormToDelete = _compensationRequisitionRepository.TryDelete(compensationId);
            _compensationRequisitionRepository.CommitTransaction();

            return fileFormToDelete;
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

        private void CheckDraftStatusUpdateAuthorized(bool? currentStatus, bool? newStatus)
        {
            if (currentStatus.HasValue && currentStatus.Value.Equals(false)
                && ((newStatus.HasValue && newStatus.Value.Equals(true)) || !newStatus.HasValue)
                && !_user.HasPermission(Permissions.SystemAdmin))
            {
                throw new NotAuthorizedException();
            }
        }

        private void CheckTotalAllowableCompensation(long currentAcquisitionFileId, PimsCompensationRequisition newCompensation)
        {
            PimsAcquisitionFile acquisitionFile = _acqFileRepository.GetById(currentAcquisitionFileId);
            if (!acquisitionFile.TotalAllowableCompensation.HasValue || (newCompensation.IsDraft.HasValue && newCompensation.IsDraft.Value))
            {
                return;
            }
            IEnumerable<PimsCompReqFinancial> allFinancialsForFile = _compReqFinancialService.GetAllByAcquisitionFileId(currentAcquisitionFileId, true);
            IEnumerable<PimsCompReqFinancial> allUnchangedFinancialsForFile = allFinancialsForFile.Where(f => f.CompensationRequisitionId != newCompensation.Internal_Id);
            decimal newTotalCompensation = allUnchangedFinancialsForFile.Concat(newCompensation.PimsCompReqFinancials).Aggregate(0m, (acc, f) => acc + (f.TotalAmt ?? 0m));
            if (newTotalCompensation > acquisitionFile.TotalAllowableCompensation)
            {
                throw new BusinessRuleViolationException("Your compensation requisition cannot be saved in FINAL status, as its compensation amount exceeds total allowable compensation for this file.");
            }
        }
    }
}
