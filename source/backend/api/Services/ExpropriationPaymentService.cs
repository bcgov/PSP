using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Api.Helpers.Extensions;
using Pims.Core.Extensions;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;

namespace Pims.Api.Services
{
    public class ExpropriationPaymentService : IExpropriationPaymentService
    {
        private readonly IExpropriationPaymentRepository _expropriationPaymentRepository;
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly IUserRepository _userRepository;
        private readonly IAcquisitionFileRepository _acqFileRepository;
        private readonly IProjectRepository _projectRepository;

        public ExpropriationPaymentService(ClaimsPrincipal user, ILogger<ExpropriationPaymentService> logger, IUserRepository userRepository, IAcquisitionFileRepository acqFileRepository, IExpropriationPaymentRepository expropriationPaymentRepository, IProjectRepository projectRepository)
        {
            _user = user;
            _logger = logger;
            _userRepository = userRepository;
            _acqFileRepository = acqFileRepository;
            _expropriationPaymentRepository = expropriationPaymentRepository;
            _projectRepository = projectRepository;
        }

        public PimsExpropriationPayment GetById(long id)
        {
            _logger.LogInformation($"Getting Expropriation Payment with id {id}");
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileView);

            return _expropriationPaymentRepository.GetById(id);
        }

        public PimsExpropriationPayment Update(PimsExpropriationPayment expropriationPayment)
        {
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileEdit);
            expropriationPayment.ThrowIfNull(nameof(expropriationPayment));
            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, _projectRepository, expropriationPayment.AcquisitionFileId);

            _logger.LogInformation($"Updating Expropriation Payment with id ${expropriationPayment.ExpropriationPaymentId}");

            PimsExpropriationPayment updatedEntity = _expropriationPaymentRepository.Update(expropriationPayment);
            _expropriationPaymentRepository.CommitTransaction();

            return updatedEntity;
        }

        public bool Delete(long id)
        {
            _logger.LogInformation("Deleting Expropriation Payment with id ...", id);
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileEdit);

            var deleteAttempt = _expropriationPaymentRepository.TryDelete(id);
            _expropriationPaymentRepository.CommitTransaction();

            return deleteAttempt;
        }
    }
}
