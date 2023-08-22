using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Api.Services
{
    public class ExpropriationPaymentService : IExpropriationPaymentService
    {
        private readonly IExpropriationPaymentRepository _expropriationPaymentRepository;
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly IUserRepository _userRepository;
        private readonly IAcquisitionFileRepository _acqFileRepository;

        public ExpropriationPaymentService(ClaimsPrincipal user, ILogger<ExpropriationPaymentService> logger, IUserRepository userRepository, IAcquisitionFileRepository acqFileRepository, IExpropriationPaymentRepository expropriationPaymentRepository)
        {
            _user = user;
            _logger = logger;
            _userRepository = userRepository;
            _acqFileRepository = acqFileRepository;
            _expropriationPaymentRepository = expropriationPaymentRepository;
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
            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, expropriationPayment.AcquisitionFileId);

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
