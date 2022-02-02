using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Dal.Services
{
    public class SecurityDepositService : ISecurityDepositService
    {
        readonly ISecurityDepositRepository _securityDepositRepository;
        readonly ISecurityDepositReturnRepository _securityDepositReturnRepository;
        readonly ILeaseRepository _leaseRepository;
        readonly ILeaseService _leaseService;
        readonly ClaimsPrincipal _user;
        public SecurityDepositService(ISecurityDepositRepository securityDepositRepository, ISecurityDepositReturnRepository securityDepositReturnRepository, ILeaseRepository leaseRepository, ILeaseService leaseService, ClaimsPrincipal user)
        {
            _securityDepositRepository = securityDepositRepository;
            _securityDepositReturnRepository = securityDepositReturnRepository;
            _leaseRepository = leaseRepository;
            _leaseService = leaseService;
            _user = user;
        }

        public PimsLease AddLeaseDeposit(long leaseId, long leaseRowVersion, PimsSecurityDeposit deposit)
        {
            _user.ThrowIfNotAuthorized(Permissions.LeaseAdd);
            ValidateServiceCall(leaseId, leaseRowVersion);
            _securityDepositRepository.Add(deposit);
            _securityDepositRepository.CommitTransaction();

            return _leaseRepository.Get(leaseId);
        }

        public PimsLease UpdateLeaseDeposit(long leaseId, long leaseRowVersion, PimsSecurityDeposit deposit)
        {
            _user.ThrowIfNotAuthorized(Permissions.LeaseEdit);
            ValidateServiceCall(leaseId, leaseRowVersion);
            _securityDepositRepository.Update(deposit);
            _securityDepositRepository.CommitTransaction();
            return _leaseRepository.Get(leaseId);
        }

        public PimsLease DeleteLeaseDeposit(long leaseId, long leaseRowVersion, PimsSecurityDeposit deposit)
        {
            _user.ThrowIfNotAuthorized(Permissions.LeaseDelete);
            ValidateServiceCall(leaseId, leaseRowVersion);
            ValidateDeletionRules(deposit);

            _securityDepositRepository.Delete(deposit.SecurityDepositId);
            _securityDepositRepository.CommitTransaction();

            return _leaseRepository.Get(leaseId);
        }

        private void ValidateServiceCall(long leaseId, long leaseRowVersion)
        {
            if (!_leaseService.IsRowVersionEqual(leaseId, leaseRowVersion))
            {
                throw new DbUpdateConcurrencyException("Lease version mismatch.");
            }
        }

        private void ValidateDeletionRules(PimsSecurityDeposit deposit)
        {
            IEnumerable<PimsSecurityDepositReturn> depositReturns = _securityDepositReturnRepository.GetByDepositId(deposit.SecurityDepositId);
            if (depositReturns.Any())
            {
                throw new InvalidOperationException("Deposits with associated returns cannot be deleted.");
            }
        }
    }
}
