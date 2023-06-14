using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
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
        private readonly IAcquisitionPayeeRepository _acquisitionPayeeRepository;
        private readonly IEntityNoteRepository _entityNoteRepository;

        public CompensationRequisitionService(ClaimsPrincipal user, ILogger<CompensationRequisitionService> logger, ICompensationRequisitionRepository compensationRequisitionRepository, IAcquisitionPayeeRepository acquisitionPayeeRepository, IEntityNoteRepository entityNoteRepository)
        {
            _user = user;
            _logger = logger;
            _compensationRequisitionRepository = compensationRequisitionRepository;
            _acquisitionPayeeRepository = acquisitionPayeeRepository;
            _entityNoteRepository = entityNoteRepository;
        }

        public PimsCompensationRequisition GetById(long compensationRequisitionId)
        {
            _logger.LogInformation($"Getting Compensation Requisition with id {compensationRequisitionId}");
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView);

            var compensationRequisition = _compensationRequisitionRepository.GetById(compensationRequisitionId);
            var compensationPayee = compensationRequisition.PimsAcquisitionPayees?.FirstOrDefault();
            if (compensationRequisition is not null && compensationPayee is not null)
            {
                // TODO fix this
                /*var payeeCheque = compensationPayee.PimsAcqPayeeCheques.FirstOrDefault();
                if (payeeCheque is not null)
                {
                    payeeCheque.PretaxAmt = compensationRequisition.PayeeChequesPreTaxTotalAmount;
                    payeeCheque.TaxAmt = compensationRequisition.PayeeChequesTaxTotalAmount;
                    payeeCheque.TotalAmt = compensationRequisition.PayeeChequesTotalAmount;
                }*/
            }

            return compensationRequisition;
        }

        public PimsAcquisitionPayee GetPayeeByCompensationId(long compensationRequisitionId)
        {
            _logger.LogInformation($"Getting Compensation Requisition Payee with compensation id {compensationRequisitionId}");
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView);

            var compensationRequisition = _compensationRequisitionRepository.GetById(compensationRequisitionId);
            if(compensationRequisition.PimsAcquisitionPayees.FirstOrDefault() is null)
            {
                throw new KeyNotFoundException();
            }

            return _acquisitionPayeeRepository.GetById(compensationRequisition.PimsAcquisitionPayees.FirstOrDefault().AcquisitionPayeeId);
        }

        public PimsCompensationRequisition Update(PimsCompensationRequisition compensationRequisition)
        {
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionEdit);
            compensationRequisition.ThrowIfNull(nameof(compensationRequisition));
            _logger.LogInformation($"Updating Compensation Requisition with id ${compensationRequisition.CompensationRequisitionId}");

            var currentCompensation = _compensationRequisitionRepository.GetById(compensationRequisition.CompensationRequisitionId);
            (bool? currentStatus, bool? newStatus) compReqStatusComparable = (currentStatus: currentCompensation.IsDraft, newStatus: compensationRequisition.IsDraft);

            PimsCompensationRequisition updatedEntity = _compensationRequisitionRepository.Update(compensationRequisition);
            AddNoteIfStatusChanged(compensationRequisition.Internal_Id, compensationRequisition.AcquisitionFileId, compReqStatusComparable);
            _compensationRequisitionRepository.CommitTransaction();

            return updatedEntity;
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

        private void AddNoteIfStatusChanged(long compensationRequisitionId, long acquisitionFileId, (bool? currentStatus, bool? newStatus) statusComparable)
        {
            if (statusComparable.currentStatus.Equals(statusComparable.newStatus))
            {
                return;
            }

            var curentStatusText = GetCompensationRequisitionStatusText(statusComparable.currentStatus);
            var newStatusText = GetCompensationRequisitionStatusText(statusComparable.newStatus);

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
    }
}
