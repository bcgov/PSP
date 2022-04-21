using System;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Entity = Pims.Dal.Entities;

namespace Pims.Dal.Helpers.Extensions
{
    /// <summary>
    /// LeaseExtensions static class, provides extension methods for leases.
    /// </summary>
    public static class LeaseExtensions
    {
        /// <summary>
        /// Generate an SQL statement for the specified 'user' and 'filter'.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        private static IQueryable<Entities.PimsLease> GenerateCommonLeaseQuery(this IQueryable<Entities.PimsLease> query, Entity.Models.LeaseFilter filter, bool loadPayments = false)
        {
            filter.ThrowIfNull(nameof(filter));

            if (!string.IsNullOrWhiteSpace(filter.TenantName))
            {
                query = query.Where(l => l.PimsLeaseTenants.Any(tenant => tenant.Person != null && EF.Functions.Like(tenant.Person.Surname + ", " + tenant.Person.FirstName + ", " + tenant.Person.MiddleNames, $"%{filter.TenantName}%"))
                || l.PimsLeaseTenants.Any(tenant => tenant.Organization != null && EF.Functions.Like(tenant.Organization.OrganizationName, $"%{filter.TenantName}%")));
            }

            if (!string.IsNullOrWhiteSpace(filter.PinOrPid))
            {
                var pinOrPidValue = filter.PinOrPid.Replace("-", "").Trim().TrimStart('0');
                query = query.Where(l => l.PimsPropertyLeases.Any(pl => pl != null && (EF.Functions.Like(pl.Property.Pid.ToString(), $"%{pinOrPidValue}%") || EF.Functions.Like(pl.Property.Pin.ToString(), $"%{pinOrPidValue}%"))));
            }

            if (!string.IsNullOrWhiteSpace(filter.LFileNo))
            {
                query = query.Where(l => EF.Functions.Like(l.LFileNo, $"%{filter.LFileNo}%"));
            }

            if (!string.IsNullOrWhiteSpace(filter.Address))
            {
                query = query.Where(l => l.PimsPropertyLeases.Any(pl => pl != null &&
                    (EF.Functions.Like(pl.Property.Address.StreetAddress1, $"%{filter.Address}%") ||
                    EF.Functions.Like(pl.Property.Address.StreetAddress2, $"%{filter.Address}%") ||
                    EF.Functions.Like(pl.Property.Address.StreetAddress3, $"%{filter.Address}%") ||
                    EF.Functions.Like(pl.Property.Address.MunicipalityName, $"%{filter.Address}%")
                    )));
            }

            if (filter.IsReceivable == true)
            {
                query = query.Where(l => l.LeasePayRvblTypeCode == "RCVBL");
            }

            if (filter.NotInStatus.Count > 0)
            {
                query = query.Where(l => !filter.NotInStatus.Contains(l.LeaseStatusTypeCode));
            }

            if (filter.ExpiryAfterDate.HasValue)
            {
                // additional terms may "extend" the original expiry date.
                query = query.Where(l => (l.OrigExpiryDate >= filter.ExpiryAfterDate.Value || l.OrigExpiryDate == null)
                    || l.PimsLeaseTerms.Any(t => t.TermExpiryDate >= filter.ExpiryAfterDate.Value || t.TermExpiryDate == null));
            }

            if (filter.StartBeforeDate.HasValue)
            {
                query = query.Where(l => l.OrigStartDate <= filter.StartBeforeDate);
            }

            if (filter.Programs.Count > 0)
            {
                query = query.Where(l => filter.Programs.Any(p => p == l.LeaseProgramTypeCode));
            }

            if (filter.LeaseStatusTypes.Count > 0)
            {
                query = query.Where(l => filter.LeaseStatusTypes.Any(p => p == l.LeaseStatusTypeCode));
            }

            if (filter.ExpiryStartDate != null && filter.ExpiryEndDate != null)
            {
                query = query.Where(l => l.OrigExpiryDate >= filter.ExpiryStartDate && l.OrigExpiryDate <= filter.ExpiryEndDate);
            }
            else if (filter.ExpiryStartDate != null)
            {
                query = query.Where(l => l.OrigExpiryDate >= filter.ExpiryStartDate);
            }
            else if (filter.ExpiryEndDate != null)
            {
                query = query.Where(l => l.OrigExpiryDate <= filter.ExpiryEndDate);
            }

            if (filter.RegionType.HasValue)
            {
                query = query.Where(l => l.RegionCode == filter.RegionType);
            }

            if (!string.IsNullOrWhiteSpace(filter.Details))
            {
                query = query.Where(l => EF.Functions.Like(l.LeaseDescription, $"%{filter.Details}%") || EF.Functions.Like(l.LeaseNotes, $"%{filter.Details}%"));
            }

            if (filter.Sort?.Any() == true)
            {
                query = query.OrderByProperty(filter.Sort);
            }
            else
            {
                query = query.OrderBy(l => l.LFileNo);
            }

            query = query.Include(l => l.PimsPropertyLeases)
                    .ThenInclude(p => p.Property)
                    .ThenInclude(p => p.Address)
                .Include(l => l.PimsPropertyLeases)
                    .ThenInclude(p => p.AreaUnitTypeCodeNavigation)
                .Include(l => l.PimsPropertyImprovements)
                .Include(l => l.LeaseProgramTypeCodeNavigation)
                .Include(l => l.LeasePurposeTypeCodeNavigation)
                .Include(l => l.LeaseStatusTypeCodeNavigation)
                .Include(l => l.LeaseLicenseTypeCodeNavigation)
                .Include(l => l.PimsLeaseTenants)
                    .ThenInclude(t => t.Person)
                .Include(l => l.PimsLeaseTenants)
                    .ThenInclude(t => t.Organization)
                .Include(p => p.RegionCodeNavigation)
                .Include(l => l.PimsLeaseTerms);


            if (loadPayments)
            {
                query = query.Include(l => l.PimsLeaseTerms)
                    .ThenInclude(l => l.PimsLeasePayments);
            }
            return query;
        }

        /// <summary>
        /// Generate a query for the specified 'filter'.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IQueryable<Entities.PimsLease> GenerateLeaseQuery(this PimsContext context, Entity.Models.LeaseFilter filter, bool loadPayments = false)
        {
            filter.ThrowIfNull(nameof(filter));

            var query = context.PimsLeases.AsNoTracking();

            query = query.GenerateCommonLeaseQuery(filter, loadPayments);

            return query;
        }

        /// <summary>
        /// Get the next available id from the PIMS_LEASE_ID_SEQ
        /// </summary>
        /// <param name="context"></param>
        public static Int64 GetNextLeaseSequenceValue(this PimsContext context)
        {
            SqlParameter result = new SqlParameter("@result", System.Data.SqlDbType.BigInt)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            context.Database.ExecuteSqlRaw("set @result = next value for dbo.PIMS_LEASE_ID_SEQ;", result);

            return (Int64)result.Value;
        }

        /// <summary>
        /// Generate a new L File in format L-XXX-YYY using the lease id. Add the lease id and lfileno to the passed lease.
        /// </summary>
        /// <param name="context"></param>
        public static PimsLease GenerateLFileNo(this PimsContext context, PimsLease lease)
        {
            Int64 leaseId = GetNextLeaseSequenceValue(context);
            lease.LeaseId = leaseId;
            lease.LFileNo = $"L-{lease.LeaseId.ToString().PadLeft(6, '0').Insert(3, "-")}";
            return lease;
        }

        /// <summary>
        /// Get the Program name from the lease's program type description
        /// </summary>
        /// <param name="lease"></param>
        /// <returns></returns>
        public static string GetProgramName(this Pims.Dal.Entities.PimsLease lease)
        {
            return lease?.LeaseProgramTypeCodeNavigation?.Description;
        }

        /// <summary>
        /// Get the full name from the lease's first tenant (person).
        /// </summary>
        /// <param name="lease"></param>
        /// <returns></returns>
        public static string GetFullName(this Pims.Dal.Entities.PimsLease lease)
        {
            return lease?.PimsLeaseTenants.FirstOrDefault(t => t != null && t.Person != null)?.Person?.GetFullName();
        }

        /// <summary>
        /// Get the full name from the lease's first tenant (person).
        /// </summary>
        /// <param name="lease"></param>
        /// <returns></returns>
        public static string GetMotiName(this Pims.Dal.Entities.PimsLease lease)
        {
            return lease.MotiContact;
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
        /// Get the tenant name from either the person or the organization
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
