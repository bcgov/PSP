using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Api.Services
{
    public class CompensationService : BaseService, ICompensationService
    {
        private readonly ILogger _logger;
        private readonly ICompensationRepository _compensationRepository;

        public CompensationService(
            ClaimsPrincipal user,
            ILogger<CompensationService> logger,
            ICompensationRepository compensationRepository)
            : base(user, logger)
        {
            _logger = logger;
            _compensationRepository = compensationRepository;
        }

        public IList<PimsCompensationRequisition> GetAcquisitionCompensations(long acquisitionFileId)
        {
            _logger.LogInformation("Getting compensations for acquisition file id ...", acquisitionFileId);
            this.User.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView, Permissions.AcquisitionFileView);

            return _compensationRepository.GetAllByAcquisitionFileId(acquisitionFileId);
        }

        public bool DeleteCompensation(long compensationId)
        {
            _logger.LogInformation("Deleting compensation with id ...", compensationId);
            this.User.ThrowIfNotAuthorized(Permissions.CompensationRequisitionDelete, Permissions.AcquisitionFileEdit);

            var fileFormToDelete = _compensationRepository.TryDelete(compensationId);
            _compensationRepository.CommitTransaction();
            return fileFormToDelete;
        }
    }
}
