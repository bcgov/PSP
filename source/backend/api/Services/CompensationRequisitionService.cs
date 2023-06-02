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

        public CompensationRequisitionService(ClaimsPrincipal user, ILogger<CompensationRequisitionService> logger, ICompensationRequisitionRepository compensationRequisitionRepository)
        {
            _user = user;
            _logger = logger;
            _compensationRequisitionRepository = compensationRequisitionRepository;
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

            _logger.LogInformation($"Updating Compensation Requisition with id ${compensationRequisition.CompensationRequisitionId}");

            var currentCompensation = _compensationRequisitionRepository.GetById(compensationRequisition.CompensationRequisitionId);
            var currentPayee = currentCompensation.PimsAcquisitionPayees.FirstOrDefault();
            var updatedPayee = compensationRequisition.PimsAcquisitionPayees.FirstOrDefault();
            if (currentPayee != null && updatedPayee != null)
            {
                if (currentPayee.InterestHolderId == updatedPayee.InterestHolderId &&
                    currentPayee.AcquisitionOwnerId == updatedPayee.AcquisitionOwnerId &&
                    currentPayee.OwnerSolicitorId == updatedPayee.OwnerSolicitorId &&
                    currentPayee.AcquisitionFilePersonId == updatedPayee.AcquisitionFilePersonId &&
                    currentPayee.OwnerRepresentativeId == updatedPayee.OwnerRepresentativeId)
                {
                    compensationRequisition.PimsAcquisitionPayees.FirstOrDefault().AcquisitionPayeeId = currentPayee.AcquisitionPayeeId;
                }
            }

            var updatedEntity = _compensationRequisitionRepository.Update(compensationRequisition);
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
