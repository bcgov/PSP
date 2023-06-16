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

        public CompensationRequisitionService(ClaimsPrincipal user, ILogger<CompensationRequisitionService> logger, ICompensationRequisitionRepository compensationRequisitionRepository, IAcquisitionPayeeRepository acquisitionPayeeRepository)
        {
            _user = user;
            _logger = logger;
            _compensationRequisitionRepository = compensationRequisitionRepository;
            _acquisitionPayeeRepository = acquisitionPayeeRepository;
        }

        public PimsCompensationRequisition GetById(long compensationRequisitionId)
        {
            _logger.LogInformation($"Getting Compensation Requisition with id {compensationRequisitionId}");
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView);

            var compensationRequisition = _compensationRequisitionRepository.GetById(compensationRequisitionId);
            var compensationPayee = compensationRequisition.PimsAcquisitionPayees?.FirstOrDefault();
            if (compensationRequisition is not null && compensationPayee is not null)
            {
                var payeeCheque = compensationPayee.PimsAcqPayeeCheques.FirstOrDefault();
                if (payeeCheque is not null)
                {
                    payeeCheque.PretaxAmt = compensationRequisition.PayeeChequesPreTaxTotalAmount;
                    payeeCheque.TaxAmt = compensationRequisition.PayeeChequesTaxTotalAmount;
                    payeeCheque.TotalAmt = compensationRequisition.PayeeChequesTotalAmount;
                }
            }

            return compensationRequisition;
        }

        public PimsAcquisitionPayee GetPayeeByCompensationId(long compensationRequisitionId)
        {
            _logger.LogInformation($"Getting Compensation Requisition Payee with compensation id {compensationRequisitionId}");
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView);

            var compensationRequisition = _compensationRequisitionRepository.GetById(compensationRequisitionId);
            var tempPayee = compensationRequisition.PimsAcquisitionPayees.FirstOrDefault();
            if (tempPayee != null)
            {
                var compensationPayee = _acquisitionPayeeRepository.GetById(tempPayee.AcquisitionPayeeId);
                if (compensationRequisition is not null && compensationPayee is not null)
                {
                    var payeeCheque = compensationPayee.PimsAcqPayeeCheques.FirstOrDefault();
                    if (payeeCheque is not null)
                    {
                        payeeCheque.PretaxAmt = compensationRequisition.PayeeChequesPreTaxTotalAmount;
                        payeeCheque.TaxAmt = compensationRequisition.PayeeChequesTaxTotalAmount;
                        payeeCheque.TotalAmt = compensationRequisition.PayeeChequesTotalAmount;
                    }
                }
                return compensationPayee;
            }

            return null;
        }

        public PimsCompensationRequisition Update(PimsCompensationRequisition compensationRequisition)
        {
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionEdit);
            compensationRequisition.ThrowIfNull(nameof(compensationRequisition));

            _logger.LogInformation($"Updating Compensation Requisition with id ${compensationRequisition.CompensationRequisitionId}");

            var currentCompensation = _compensationRequisitionRepository.GetById(compensationRequisition.CompensationRequisitionId);
            var currentPayee = currentCompensation.PimsAcquisitionPayees.FirstOrDefault();
            var updatedPayee = compensationRequisition.PimsAcquisitionPayees.FirstOrDefault();
            var payeeCheque = updatedPayee?.PimsAcqPayeeCheques.FirstOrDefault();

            if (currentPayee != null && updatedPayee != null)
            {
                if (currentPayee.InterestHolderId != updatedPayee.InterestHolderId ||
                    currentPayee.AcquisitionOwnerId != updatedPayee.AcquisitionOwnerId ||
                    currentPayee.OwnerSolicitorId != updatedPayee.OwnerSolicitorId ||
                    currentPayee.AcquisitionFilePersonId != updatedPayee.AcquisitionFilePersonId ||
                    currentPayee.OwnerRepresentativeId != updatedPayee.OwnerRepresentativeId)
                {
                    // Given there  is only one payee per compensation, set the existing ids for the compensation requisition payee.
                    updatedPayee.CompensationRequisitionId = currentPayee.CompensationRequisitionId;
                    updatedPayee.AcquisitionPayeeId = currentPayee.AcquisitionPayeeId;
                    updatedPayee.ConcurrencyControlNumber = currentPayee.ConcurrencyControlNumber;
                    _compensationRequisitionRepository.UpdatePayee(updatedPayee);
                }
            }

            if (payeeCheque is not null)
            {
                _compensationRequisitionRepository.UpdatePayeeCheque(payeeCheque);
            }

            PimsCompensationRequisition updatedEntity = _compensationRequisitionRepository.Update(compensationRequisition);

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
    }
}
