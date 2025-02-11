using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;

namespace Pims.Api.Services
{
    public class SecurityDepositService : ISecurityDepositService
    {
        private readonly ISecurityDepositRepository _securityDepositRepository;
        private readonly ISecurityDepositReturnRepository _securityDepositReturnRepository;
        private readonly ILeaseService _leaseService;
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly ILeaseStatusSolver _leaseStatusSolver;

        public SecurityDepositService(ISecurityDepositRepository securityDepositRepository, ISecurityDepositReturnRepository securityDepositReturnRepository, ILeaseService leaseService, ClaimsPrincipal user, ILogger<SecurityDepositService> logger, ILeaseStatusSolver leaseStatusSolver)
        {
            _securityDepositRepository = securityDepositRepository;
            _securityDepositReturnRepository = securityDepositReturnRepository;
            _leaseService = leaseService;
            _user = user;
            _logger = logger;
            _leaseStatusSolver = leaseStatusSolver;
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

            var currentLease = _leaseService.GetById(leaseId);
            var currentLeaseStatus = _leaseStatusSolver.GetCurrentLeaseStatus(currentLease?.LeaseStatusTypeCode);
            if (!_leaseStatusSolver.CanEditDeposits(currentLeaseStatus))
            {
                throw new BusinessRuleViolationException("The file you are editing is not active, so you cannot save changes. Refresh your browser to see file state.");
            }

            var securityDeposit = _securityDepositRepository.Add(deposit);
            _securityDepositRepository.CommitTransaction();

            return securityDeposit;
        }

        public PimsSecurityDeposit UpdateLeaseDeposit(long leaseId, PimsSecurityDeposit deposit)
        {
            _logger.LogInformation("Updating lease deposit for lease id {leaseid} deposit id {depositId}", leaseId, deposit.SecurityDepositId);
            _user.ThrowIfNotAuthorized(Permissions.LeaseEdit);

            var currentLease = _leaseService.GetById(leaseId);
            var currentLeaseStatus = _leaseStatusSolver.GetCurrentLeaseStatus(currentLease?.LeaseStatusTypeCode);
            if (!_leaseStatusSolver.CanEditDeposits(currentLeaseStatus))
            {
                throw new BusinessRuleViolationException("The file you are editing is not active, so you cannot save changes. Refresh your browser to see file state.");
            }

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
            var lease = _leaseService.GetById(leaseId);

            var currentLeaseStatus = _leaseStatusSolver.GetCurrentLeaseStatus(lease?.LeaseStatusTypeCode);
            if (!_leaseStatusSolver.CanEditDeposits(currentLeaseStatus))
            {
                throw new BusinessRuleViolationException("The file you are editing is not active, so you cannot save changes. Refresh your browser to see file state.");
            }

            lease.ReturnNotes = note;
            _leaseService.Update(lease, new List<UserOverrideCode>());
            _securityDepositRepository.CommitTransaction();
        }

        public bool DeleteLeaseDeposit(PimsSecurityDeposit deposit)
        {
            _logger.LogInformation("Deleting lease deposit for lease id {leaseId}, deposit id {depositId}", deposit.LeaseId, deposit.SecurityDepositId);
            _user.ThrowIfNotAuthorized(Permissions.LeaseEdit);
            ValidateDeletionRules(deposit);

            var currentLease = _leaseService.GetById(deposit.LeaseId);
            var currentLeaseStatus = _leaseStatusSolver.GetCurrentLeaseStatus(currentLease?.LeaseStatusTypeCode);
            if (!_leaseStatusSolver.CanEditDeposits(currentLeaseStatus))
            {
                throw new BusinessRuleViolationException("The file you are editing is not active, so you cannot save changes. Refresh your browser to see file state.");
            }

            _securityDepositRepository.Update(deposit);

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
