using System.Security.Claims;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;

namespace Pims.Api.Services
{
    public class SecurityDepositReturnService : ISecurityDepositReturnService
    {
        private readonly ISecurityDepositReturnRepository _securityDepositReturnRepository;
        private readonly ILeaseService _leaseService;
        private readonly ILeaseStatusSolver _leaseStatusSolver;
        private readonly ClaimsPrincipal _user;

        public SecurityDepositReturnService(ISecurityDepositReturnRepository depositReturnRepository, ILeaseService leaseService, ILeaseStatusSolver leaseStatusSolver, ClaimsPrincipal user)
        {
            _securityDepositReturnRepository = depositReturnRepository;
            _leaseService = leaseService;
            _leaseStatusSolver = leaseStatusSolver;
            _user = user;
        }

        public PimsSecurityDepositReturn AddLeaseDepositReturn(long leaseId, PimsSecurityDepositReturn deposit)
        {
            _user.ThrowIfNotAuthorized(Permissions.LeaseAdd);

            var currentLease = _leaseService.GetById(leaseId);
            var currentLeaseStatus = _leaseStatusSolver.GetCurrentLeaseStatus(currentLease?.LeaseStatusTypeCode);
            if (!_leaseStatusSolver.CanEditDeposits(currentLeaseStatus))
            {
                throw new BusinessRuleViolationException("The file you are editing is not active, so you cannot save changes. Refresh your browser to see file state.");
            }

            var addedDeposit = _securityDepositReturnRepository.Add(deposit);
            _securityDepositReturnRepository.CommitTransaction();

            return addedDeposit;
        }

        public PimsSecurityDepositReturn UpdateLeaseDepositReturn(long leaseId, PimsSecurityDepositReturn deposit)
        {
            _user.ThrowIfNotAuthorized(Permissions.LeaseEdit);

            var currentLease = _leaseService.GetById(leaseId);
            var currentLeaseStatus = _leaseStatusSolver.GetCurrentLeaseStatus(currentLease?.LeaseStatusTypeCode);
            if (!_leaseStatusSolver.CanEditDeposits(currentLeaseStatus))
            {
                throw new BusinessRuleViolationException("The file you are editing is not active, so you cannot save changes. Refresh your browser to see file state.");
            }

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

            var currentLease = _leaseService.GetById(leaseId);
            var currentLeaseStatus = _leaseStatusSolver.GetCurrentLeaseStatus(currentLease?.LeaseStatusTypeCode);
            if (!_leaseStatusSolver.CanEditDeposits(currentLeaseStatus))
            {
                throw new BusinessRuleViolationException("The file you are editing is not active, so you cannot save changes. Refresh your browser to see file state.");
            }

            _securityDepositReturnRepository.Delete(deposit.SecurityDepositReturnId);
            _securityDepositReturnRepository.CommitTransaction();

            return true;
        }
    }
}
