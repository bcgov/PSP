using System.Security.Claims;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Api.Services
{
    public class SecurityDepositReturnService : ISecurityDepositReturnService
    {
        private readonly ISecurityDepositReturnRepository _securityDepositReturnRepository;
        private readonly ClaimsPrincipal _user;

        public SecurityDepositReturnService(ISecurityDepositReturnRepository depositReturnRepository, ILeaseRepository leaseRepository, ILeaseService leaseService, ClaimsPrincipal user)
        {
            _securityDepositReturnRepository = depositReturnRepository;
            _user = user;
        }

        public PimsSecurityDepositReturn AddLeaseDepositReturn(long leaseId, PimsSecurityDepositReturn deposit)
        {
            _user.ThrowIfNotAuthorized(Permissions.LeaseAdd);
            var addedDeposit = _securityDepositReturnRepository.Add(deposit);
            _securityDepositReturnRepository.CommitTransaction();

            return addedDeposit;
        }

        public PimsSecurityDepositReturn UpdateLeaseDepositReturn(long leaseId, PimsSecurityDepositReturn deposit)
        {
            _user.ThrowIfNotAuthorized(Permissions.LeaseEdit);
            var currentHolder = _securityDepositReturnRepository.GetById(deposit.SecurityDepositReturnId).PimsSecurityDepositReturnHolder;
            if (currentHolder != null)
            {
                deposit.PimsSecurityDepositReturnHolder.SecurityDepositReturnHolderId = currentHolder.SecurityDepositReturnHolderId;
                deposit.PimsSecurityDepositReturnHolder.ConcurrencyControlNumber = currentHolder.ConcurrencyControlNumber;
            }
            var updatedDeposit = _securityDepositReturnRepository.Update(deposit);
            _securityDepositReturnRepository.CommitTransaction();

            return updatedDeposit;
        }

        public bool DeleteLeaseDepositReturn(long leaseId, PimsSecurityDepositReturn deposit)
        {
            _user.ThrowIfNotAuthorized(Permissions.LeaseEdit);
            _securityDepositReturnRepository.Delete(deposit.SecurityDepositReturnId);
            _securityDepositReturnRepository.CommitTransaction();

            return true;
        }
    }
}
