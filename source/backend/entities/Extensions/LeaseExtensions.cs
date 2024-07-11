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
        /// Get the tenant name from either the person or the organization.
        /// </summary>
        /// <param name="lease"></param>
        /// <returns></returns>
        public static string GetTenantName(this PimsLeaseTenant lease)
        {
            return lease?.Person?.GetFullName() ?? lease?.Organization?.Name;
        }

        /// <summary>
        /// Get the calculated expiry date.
        /// </summary>
        /// <param name="lease"></param>
        /// <returns></returns>
        public static DateTime? GetExpiryDate(this PimsLease lease)
        {
            if (lease.PimsLeasePeriods != null && lease.PimsLeasePeriods.Any(p => p.PeriodExpiryDate == null && !p.IsFlexibleDuration))
            {
                return null;
            }
            if (lease.OrigExpiryDate != null)
            {
                if (lease.PimsLeasePeriods != null && lease.PimsLeasePeriods.Any(p => p.PeriodExpiryDate > lease.OrigExpiryDate && !p.IsFlexibleDuration))
                {
                    return lease.PimsLeasePeriods.OrderByDescending(p => p.PeriodExpiryDate).FirstOrDefault().PeriodExpiryDate;
                }
                return lease.OrigExpiryDate;
            }
            return lease.PimsLeasePeriods?.OrderByDescending(p => p.PeriodExpiryDate).FirstOrDefault(p => !p.IsFlexibleDuration)?.PeriodExpiryDate;
        }
    }
}
