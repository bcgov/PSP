using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Api.Helpers.Exceptions;
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
            _logger.LogInformation($"Getting Compensatio Requisition with id {compensationRequisitionId}");
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView);

            return _compensationRequisitionRepository.GetById(compensationRequisitionId);
        }

        public PimsCompensationRequisition Update(long compensationRequisitionId, PimsCompensationRequisition compensationRequisition)
        {
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionEdit);
            compensationRequisition.ThrowIfNull(nameof(compensationRequisition));

            if(compensationRequisitionId != compensationRequisition.CompensationRequisitionId)
            {
                throw new BadRequestException("Invalid compensationRequisitionId.");
            }

            _logger.LogInformation($"Updating Compensation Requisition with id ${compensationRequisition.Internal_Id}");

            var updatedEntity = _compensationRequisitionRepository.Update(compensationRequisitionId, compensationRequisition);
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
