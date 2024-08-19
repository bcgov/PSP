using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using static Pims.Dal.Entities.PimsLeasePeriodStatusType;

namespace Pims.Api.Services
{
    public class LeasePeriodService : ILeasePeriodService
    {
        private readonly ILeasePeriodRepository _leasePeriodRepository;
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;

        public LeasePeriodService(ILeasePeriodRepository leasePeriodRepository, ClaimsPrincipal user, ILogger<LeasePeriodService> logger)
        {
            _leasePeriodRepository = leasePeriodRepository;
            _user = user;
            _logger = logger;
        }

        public IEnumerable<PimsLeasePeriod> GetPeriods(long leaseId)
        {
            _logger.LogInformation("Getting periods from lease with id: {id}", leaseId);
            _user.ThrowIfNotAuthorized(Permissions.LeaseView);
            return _leasePeriodRepository.GetAllByLeaseId(leaseId);
        }

        public bool DeletePeriod(long leaseId, PimsLeasePeriod period)
        {
            _logger.LogInformation("Deleting period to lease with id: {id}", leaseId);
            ValidateDeletionRules(period);

            _leasePeriodRepository.Delete(period.Internal_Id);
            _leasePeriodRepository.CommitTransaction();

            return true;
        }

        public PimsLeasePeriod UpdatePeriod(long leaseId, long periodId, PimsLeasePeriod period)
        {
            _logger.LogInformation("Updating period to lease with id: {id}", leaseId);
            ValidateUpdateRules(period, periodId);

            _leasePeriodRepository.Update(period);
            _leasePeriodRepository.CommitTransaction();

            return period;
        }

        public PimsLeasePeriod AddPeriod(long leaseId, PimsLeasePeriod period)
        {
            _logger.LogInformation("Adding period to lease with id: {id}", leaseId);
            ValidateAddRules(period);

            _leasePeriodRepository.Add(period);
            _leasePeriodRepository.CommitTransaction();

            return period;
        }

        private void ValidateDeletionRules(PimsLeasePeriod period)
        {
            PimsLeasePeriod leasePeriodToDelete = _leasePeriodRepository.GetById(period.Internal_Id, true);
            if (leasePeriodToDelete.PimsLeasePayments.Count > 0)
            {
                throw new InvalidOperationException("A period with payments attached can not be deleted. If you intend to delete this period, you must delete each of the corresponding payments first.");
            }
            if (leasePeriodToDelete.LeasePeriodStatusTypeCode == LeasePeriodStatusTypes.EXER)
            {
                throw new InvalidOperationException("Exercised periods cannot be deleted. Remove all payments from this period and set this period to 'Not Exercised' to delete this period.");
            }
            IEnumerable<PimsLeasePeriod> periodsForLease = _leasePeriodRepository.GetAllByLeaseId(period.LeaseId).OrderBy(t => t.PeriodStartDate).ThenBy(t => t.LeasePeriodId);
            if (period.Internal_Id == periodsForLease.FirstOrDefault()?.Internal_Id && periodsForLease.Count() > 1)
            {
                throw new InvalidOperationException("You must delete all renewals before deleting the initial period.");
            }
        }

        /// <summary>
        /// Validate if the incoming period date range overlaps any existing date ranges, or if the existing lease period to be updated has any payments but is not being set to EXER.
        /// </summary>
        /// <param name="period"></param>
        /// <param name="periodId"></param>
        private void ValidateUpdateRules(PimsLeasePeriod period, long periodId)
        {
            ValidateOverlappingPeriod(period);

            PimsLeasePeriod leasePeriodToUpdate = _leasePeriodRepository.GetById(periodId, true);
            if (leasePeriodToUpdate.PimsLeasePayments.Count > 0 && period.LeasePeriodStatusTypeCode != LeasePeriodStatusTypes.EXER)
            {
                throw new InvalidOperationException("Period must be 'exercised' if payments have been made.");
            }
            if(leasePeriodToUpdate.IsVariablePayment != period.IsVariablePayment)
            {
                throw new InvalidOperationException("Period payment variability may not be changed after period creation.");
            }
        }

        /// <summary>
        /// Validate if the new incoming period date range overlaps any existing date ranges, or if the new incoming period has any payments but is not being set to exercised.
        /// </summary>
        /// <param name="period"></param>
        private void ValidateAddRules(PimsLeasePeriod period)
        {
            ValidateOverlappingPeriod(period);

            if (period.PimsLeasePayments.Count > 0 && period.LeasePeriodStatusTypeCode != LeasePeriodStatusTypes.EXER)
            {
                throw new InvalidOperationException("Period must be 'exercised' if payments have been made.");
            }
        }

        private void ValidateOverlappingPeriod(PimsLeasePeriod period)
        {
            if (IsPeriodOverlapping(period))
            {
                throw new InvalidOperationException("A new period start and end date must not conflict with any existing periods.");
            }
        }

        /// <summary>
        /// Does the date range of this period overlap any existing periods on this lease, assuming a null end date is logically equivalent to no fixed end date.
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        private bool IsPeriodOverlapping(PimsLeasePeriod period)
        {
            IEnumerable<PimsLeasePeriod> periods = _leasePeriodRepository.GetAllByLeaseId(period.LeaseId);

            return periods.Any(t => t.Internal_Id != period.Internal_Id && ((t.PeriodExpiryDate >= period.PeriodStartDate && t.PeriodStartDate <= period.PeriodStartDate)
                || (t.PeriodExpiryDate == null && t.PeriodStartDate <= period.PeriodStartDate)
                || (period.PeriodExpiryDate == null && t.PeriodStartDate >= period.PeriodStartDate)));
        }
    }
}
