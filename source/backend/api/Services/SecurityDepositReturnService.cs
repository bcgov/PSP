using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Api.Services
{
    public class SecurityDepositReturnService : ISecurityDepositReturnService
    {
        private readonly ISecurityDepositReturnRepository _securityDepositReturnRepository;
        private readonly ILeaseRepository _leaseRepository;
        private readonly ILeaseService _leaseService;
        private readonly ClaimsPrincipal _user;

        public SecurityDepositReturnService(ISecurityDepositReturnRepository depositReturnRepository, ILeaseRepository leaseRepository, ILeaseService leaseService, ClaimsPrincipal user)
        {
            _securityDepositReturnRepository = depositReturnRepository;
            _leaseRepository = leaseRepository;
            _leaseService = leaseService;
            _user = user;
        }

        public PimsLease AddLeaseDepositReturn(long leaseId, long leaseRowVersion, PimsSecurityDepositReturn deposit)
        {
            _user.ThrowIfNotAuthorized(Permissions.LeaseAdd);
            ValidateServiceCall(leaseId, leaseRowVersion);
            _securityDepositReturnRepository.Add(deposit);
            _securityDepositReturnRepository.CommitTransaction();

            return _leaseRepository.Get(leaseId);
        }

        public PimsLease UpdateLeaseDepositReturn(long leaseId, long leaseRowVersion, PimsSecurityDepositReturn deposit)
        {
            _user.ThrowIfNotAuthorized(Permissions.LeaseEdit);
            ValidateServiceCall(leaseId, leaseRowVersion);
            var currentHolder = _securityDepositReturnRepository.GetById(deposit.SecurityDepositReturnId).PimsSecurityDepositReturnHolder;
            if (currentHolder != null)
            {
                deposit.PimsSecurityDepositReturnHolder.SecurityDepositReturnHolderId = currentHolder.SecurityDepositReturnHolderId;
                deposit.PimsSecurityDepositReturnHolder.ConcurrencyControlNumber = currentHolder.ConcurrencyControlNumber;
            }
            _securityDepositReturnRepository.Update(deposit);
            _securityDepositReturnRepository.CommitTransaction();

            return _leaseRepository.Get(leaseId);
        }

        public PimsLease DeleteLeaseDepositReturn(long leaseId, long leaseRowVersion, PimsSecurityDepositReturn deposit)
        {
            _user.ThrowIfNotAuthorized(Permissions.LeaseEdit);
            ValidateServiceCall(leaseId, leaseRowVersion);
            _securityDepositReturnRepository.Delete(deposit.SecurityDepositReturnId);
            _securityDepositReturnRepository.CommitTransaction();

            return _leaseRepository.Get(leaseId);
        }

        /// <summary>
        /// For a deposit service call to be valid, the user must have the lease edit claim and the lease being edited must be up to date.
        /// </summary>
        /// <param name="leaseId"></param>
        /// <param name="leaseRowVersion"></param>
        private void ValidateServiceCall(long leaseId, long leaseRowVersion)
        {
            if (!_leaseService.IsRowVersionEqual(leaseId, leaseRowVersion))
            {
                throw new DbUpdateConcurrencyException("You are working with an older version of this lease, please refresh the application and retry.");
            }
        }
    }
}
