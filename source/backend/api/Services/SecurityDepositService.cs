using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Api.Services
{
    public class SecurityDepositService : ISecurityDepositService
    {
        private readonly ISecurityDepositRepository _securityDepositRepository;
        private readonly ISecurityDepositReturnRepository _securityDepositReturnRepository;
        private readonly ILeaseRepository _leaseRepository;
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;

        public SecurityDepositService(ISecurityDepositRepository securityDepositRepository, ISecurityDepositReturnRepository securityDepositReturnRepository, ILeaseRepository leaseRepository, ClaimsPrincipal user, ILogger<SecurityDepositService> logger)
        {
            _securityDepositRepository = securityDepositRepository;
            _securityDepositReturnRepository = securityDepositReturnRepository;
            _leaseRepository = leaseRepository;
            _user = user;
            _logger = logger;
        }

        public IEnumerable<PimsSecurityDeposit> GetLeaseDeposits(long leaseId)
        {
            _logger.LogInformation("Getting lease deposits for lease id {id}", leaseId);
            _user.ThrowIfNotAuthorized(Permissions.LeaseView);

            return _securityDepositRepository.GetAllByLeaseId(leaseId);
        }

        public PimsSecurityDeposit AddLeaseDeposit(long leaseId, PimsSecurityDeposit deposit)
        {
            _logger.LogInformation("Adding lease deposit for lease id {id}", leaseId);
            _user.ThrowIfNotAuthorized(Permissions.LeaseAdd);
            var securityDeposit = _securityDepositRepository.Add(deposit);
            _securityDepositRepository.CommitTransaction();

            return securityDeposit;
        }

        public PimsSecurityDeposit UpdateLeaseDeposit(long leaseId, PimsSecurityDeposit deposit)
        {
            _logger.LogInformation("Updating lease deposit for lease id {leaseid} deposit id {depositId}", leaseId, deposit.SecurityDepositId);
            _user.ThrowIfNotAuthorized(Permissions.LeaseEdit);
            var currentHolder = _securityDepositRepository.GetById(deposit.SecurityDepositId).PimsSecurityDepositHolder;
            if (currentHolder != null)
            {
                deposit.PimsSecurityDepositHolder.SecurityDepositHolderId = currentHolder.SecurityDepositHolderId;
                deposit.PimsSecurityDepositHolder.ConcurrencyControlNumber = currentHolder.ConcurrencyControlNumber;
            }
            var securityDeposit = _securityDepositRepository.Update(deposit);
            _securityDepositRepository.CommitTransaction();
            return securityDeposit;
        }

        public void UpdateLeaseDepositNote(long leaseId, string note)
        {
            _logger.LogInformation("Updating lease deposit note for lease id {leaseId}", leaseId);
            _user.ThrowIfNotAuthorized(Permissions.LeaseEdit);
            var lease = _leaseRepository.Get(leaseId);
            lease.ReturnNotes = note;
            _leaseRepository.Update(lease);
            _leaseRepository.CommitTransaction();
        }

        public bool DeleteLeaseDeposit(PimsSecurityDeposit deposit)
        {
            _logger.LogInformation("Deleting lease deposit for lease id {leaseId}, deposit id {depositId}", deposit.LeaseId, deposit.SecurityDepositId);
            _user.ThrowIfNotAuthorized(Permissions.LeaseEdit);
            ValidateDeletionRules(deposit);

            bool deleted = _securityDepositRepository.Delete(deposit.SecurityDepositId);
            _securityDepositRepository.CommitTransaction();
            return deleted;
        }

        private void ValidateDeletionRules(PimsSecurityDeposit deposit)
        {
            IEnumerable<PimsSecurityDepositReturn> depositReturns = _securityDepositReturnRepository.GetAllByDepositId(deposit.SecurityDepositId);
            if (depositReturns.Any())
            {
                throw new InvalidOperationException("Deposits with associated returns cannot be deleted.");
            }
        }
    }
}
