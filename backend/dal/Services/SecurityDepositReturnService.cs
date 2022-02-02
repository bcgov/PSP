using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Dal.Services
{
    public class SecurityDepositReturnService : ISecurityDepositReturnService
    {
        readonly ISecurityDepositReturnRepository _securityDepositReturnRepository;
        readonly ILeaseRepository _leaseRepository;
        readonly ILeaseService _leaseService;
        readonly ClaimsPrincipal _user;
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
            _securityDepositReturnRepository.Update(deposit);
            _securityDepositReturnRepository.CommitTransaction();

            return _leaseRepository.Get(leaseId);
        }

        public PimsLease DeleteLeaseDepositReturn(long leaseId, long leaseRowVersion, PimsSecurityDepositReturn deposit)
        {
            _user.ThrowIfNotAuthorized(Permissions.LeaseDelete);
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
