using Pims.Api.Models.CodeTypes;
using Pims.Core.Extensions;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using static Pims.Dal.Entities.PimsLeaseStatusType;

namespace Pims.Api.Services
{
    public class LeaseReportsService : ILeaseReportsService
    {
        private readonly ILeaseRepository _leaseRepository;
        private readonly IUserRepository _userRepository;
        private readonly ClaimsPrincipal _user;
        private readonly ILeasePaymentRepository _leasePaymentRepository;
        private readonly ILeaseService _leaseService;

        public LeaseReportsService(ILeaseRepository leaseRepository, IUserRepository userRepository, ClaimsPrincipal user, ILeasePaymentRepository leasePaymentRepository, ILeaseService leaseService)
        {
            _leaseRepository = leaseRepository;
            _userRepository = userRepository;
            _user = user;
            _leasePaymentRepository = leasePaymentRepository;
            _leaseService = leaseService;
        }

        public IEnumerable<PimsLease> GetAggregatedLeaseReport(int fiscalYearStart)
        {
            _user.ThrowIfNotAuthorized(Permissions.LeaseView);
            DateTime fiscalYearStartDate = fiscalYearStart.ToFiscalYearDate();
            var pimsUser = _userRepository.GetByKeycloakUserId(_user.GetUserKey());
            long? contractorPersonId = pimsUser.IsContractor ? pimsUser.PersonId : null;

            // fiscal defined as April 01 to March 31 of following year
            return _leaseRepository.GetAllByFilter(
                new Dal.Entities.Models.LeaseFilter()
                {
                    ExpiryAfterDate = fiscalYearStartDate,
                    StartBeforeDate = fiscalYearStartDate.AddYears(1).AddDays(-1),
                    NotInStatus = new List<string>() { LeaseStatusTypes.DRAFT.ToString(), LeaseStatusTypes.DISCARD.ToString(), LeaseStatusTypes.DUPLICATE.ToString() },
                    IsReceivable = true,
                },
                true,
                contractorPersonId);
        }

        public IEnumerable<PimsLeasePayment> GetLeasePaymentsReport(int fiscalYearStart)
        {
            _user.ThrowIfNotAuthorized(Permissions.LeaseView);

            var pimsUser = _userRepository.GetByKeycloakUserId(_user.GetUserKey());
            long? contractorPersonId = pimsUser.IsContractor ? pimsUser.PersonId : null;

            // fiscal defined as April 01 to March 31 of following year
            DateTime fiscalYearStartDate = fiscalYearStart.ToFiscalYearDate();
            DateTime fiscalYearEndDate = fiscalYearStartDate.AddYears(1).AddDays(-1);

            var allPayments = _leasePaymentRepository.GetAllByDateRange(fiscalYearStartDate, fiscalYearEndDate, contractorPersonId).ToList();
            var leaseIds = allPayments.Select(payment => payment.LeasePeriod.LeaseId);
            var activeLeases = _leaseService.GetAllByIds(leaseIds).Where(l => l.LeaseStatusTypeCode != LeaseStatusTypes.DUPLICATE.ToString() && l.LeaseStatusTypeCode != LeaseStatusTypes.DRAFT.ToString() && l.LeaseStatusTypeCode != LeaseStatusTypes.DISCARD.ToString()).ToList();
            var activePayments = allPayments.Where(payment => activeLeases.Any(lease => lease.LeaseId == payment.LeasePeriod.LeaseId)).ToList();

            // Required to display the latest payment on the lease, which may not be part of the current date range filter of payments. This ensures that all payments for a lease associated to one of the payments in the date range are included.
            activePayments.ForEach(payment =>
            {
                payment.LeasePeriod.Lease = activeLeases.FirstOrDefault(lease => lease.LeaseId == payment.LeasePeriod.LeaseId);
            });

            return allPayments;
        }
    }
}
