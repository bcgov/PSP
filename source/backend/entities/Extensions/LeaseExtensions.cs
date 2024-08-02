using System;
using System.Linq;
using Pims.Dal.Entities;

namespace Pims.Dal.Helpers.Extensions
{
    /// <summary>
    /// LeaseExtensions static class, provides extension methods for leases.
    /// </summary>
    public static class LeaseExtensions
    {
        /// <summary>
        /// Get the Program name from the lease's program type description.
        /// </summary>
        /// <param name="lease"></param>
        /// <returns></returns>
        public static string GetProgramName(this PimsLease lease)
        {
            return lease?.LeaseProgramTypeCodeNavigation?.Description;
        }

        /// <summary>
        /// Get the active period of this lease.
        /// </summary>
        /// <param name="lease"></param>
        /// <returns></returns>
        public static PimsLeasePeriod GetCurrentActivePeriod(this PimsLease lease)
        {
            return lease.PimsLeasePeriods.FirstOrDefault(period => period != null && DateTime.Now > period.PeriodStartDate && DateTime.Now <= period.PeriodExpiryDate);
        }

        /// <summary>
        /// Get the active period start date of this lease.
        /// </summary>
        /// <param name="lease"></param>
        /// <returns></returns>
        public static DateTime? GetCurrentPeriodStartDate(this PimsLease lease)
        {
            return GetCurrentActivePeriod(lease)?.PeriodStartDate;
        }

        /// <summary>
        /// Get the active period end date of this lease.
        /// </summary>
        /// <param name="lease"></param>
        /// <returns></returns>
        public static DateTime? GetCurrentPeriodEndDate(this PimsLease lease)
        {
            return GetCurrentActivePeriod(lease)?.PeriodExpiryDate;
        }

        /// <summary>
        /// Get the stakeholder name from either the person or the organization.
        /// </summary>
        /// <param name="stakeholder"></param>
        /// <returns></returns>
        public static string GetStakeholderName(this PimsLeaseStakeholder stakeholder)
        {
            return stakeholder?.Person?.GetFullName() ?? stakeholder?.Organization?.Name;
        }

        /// <summary>
        /// Get the calculated expiry date.
        /// </summary>
        /// <param name="lease"></param>
        /// <returns></returns>
        public static DateTime? GetExpiryDate(this PimsLease lease)
        {
            var expiryDate = lease.PimsLeaseRenewals.Where(r => r.IsExercised == true).Select(fr => fr.ExpiryDt).Append(lease.OrigExpiryDate).Max();
            return expiryDate;
        }
    }
}
