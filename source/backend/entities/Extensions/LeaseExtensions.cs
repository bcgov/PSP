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
        public static string GetProgramName(this Pims.Dal.Entities.PimsLease lease)
        {
            return lease?.LeaseProgramTypeCodeNavigation?.Description;
        }

        /// <summary>
        /// Get the active term of this lease.
        /// </summary>
        /// <param name="lease"></param>
        /// <returns></returns>
        public static PimsLeaseTerm GetCurrentTerm(this Pims.Dal.Entities.PimsLease lease)
        {
            return lease.PimsLeaseTerms.FirstOrDefault(term => term != null && DateTime.Now > term.TermStartDate && DateTime.Now <= term.TermExpiryDate);
        }

        /// <summary>
        /// Get the active term start date of this lease.
        /// </summary>
        /// <param name="lease"></param>
        /// <returns></returns>
        public static DateTime? GetCurrentTermStartDate(this Pims.Dal.Entities.PimsLease lease)
        {
            return GetCurrentTerm(lease)?.TermStartDate;
        }

        /// <summary>
        /// Get the active term end date of this lease.
        /// </summary>
        /// <param name="lease"></param>
        /// <returns></returns>
        public static DateTime? GetCurrentTermEndDate(this Pims.Dal.Entities.PimsLease lease)
        {
            return GetCurrentTerm(lease)?.TermExpiryDate;
        }

        /// <summary>
        /// Get the tenant name from either the person or the organization.
        /// </summary>
        /// <param name="lease"></param>
        /// <returns></returns>
        public static string GetTenantName(this Pims.Dal.Entities.PimsLeaseTenant lease)
        {
            return lease?.Person?.GetFullName() ?? lease?.Organization?.Name;
        }

        /// <summary>
        /// Get the calculated expiry date.
        /// </summary>
        /// <param name="lease"></param>
        /// <returns></returns>
        public static DateTime? GetExpiryDate(this Pims.Dal.Entities.PimsLease lease)
        {
            if (lease.PimsLeaseTerms != null && lease.PimsLeaseTerms.Any(t => t.TermExpiryDate == null))
            {
                return null;
            }
            if (lease.OrigExpiryDate != null)
            {
                if (lease.PimsLeaseTerms != null && lease.PimsLeaseTerms.Any(t => t.TermExpiryDate > lease.OrigExpiryDate))
                {
                    return lease.PimsLeaseTerms.OrderByDescending(t => t.TermExpiryDate).FirstOrDefault().TermExpiryDate;
                }
                return lease.OrigExpiryDate;
            }
            return lease.PimsLeaseTerms?.OrderByDescending(t => t.TermExpiryDate).FirstOrDefault()?.TermExpiryDate;
        }
    }
}
