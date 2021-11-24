using System;
using System.Linq;
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
        private static IQueryable<Entity.Lease> GenerateCommonLeaseQuery(this IQueryable<Entity.Lease> query, Entity.Models.LeaseFilter filter)
        {
            filter.ThrowIfNull(nameof(filter));

            if (!string.IsNullOrWhiteSpace(filter.TenantName))
            {
                query = query.Where(l => l.Persons.Any(person => person != null && EF.Functions.Like(person.Surname + ", " + person.FirstName + ", " + person.MiddleNames, $"%{filter.TenantName}%"))
                || l.Organizations.Any(organization => organization != null && EF.Functions.Like(organization.Name, $"%{filter.TenantName}%")));
            }

            if (!string.IsNullOrWhiteSpace(filter.PidOrPin))
            {
                var pidOrPinValue = filter.PidOrPin.Replace("-", "").Trim();
                query = query.Where(l => l.Properties.Any(p => p != null && (p.PID.ToString().Contains(pidOrPinValue) || p.PIN.ToString().Contains(pidOrPinValue))));
            }

            if (!string.IsNullOrWhiteSpace(filter.LFileNo))
            {
                query = query.Where(l => EF.Functions.Like(l.LFileNo, $"%{filter.LFileNo}%"));
            }

            if (filter.Programs.Count > 0)
            {
                query = query.Where(l => filter.Programs.Any(p => p == l.ProgramTypeId));
            }

            if (filter.Sort?.Any() == true)
                query = query.OrderByProperty(filter.Sort);
            else
                query = query.OrderBy(l => l.LFileNo);

            return query.Include(l => l.Properties)
                .ThenInclude(p => p.Address)
                .Include(l => l.ProgramType)
                .Include(l => l.TenantsManyToMany)
                .ThenInclude(t => t.Person)
                .Include(l => l.TenantsManyToMany)
                .ThenInclude(t => t.Organization)
                .Include(l => l.Improvements);
        }

        /// <summary>
        /// Generate a query for the specified 'filter'.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IQueryable<Entity.Lease> GenerateLeaseQuery(this PimsContext context, Entity.Models.LeaseFilter filter)
        {
            filter.ThrowIfNull(nameof(filter));

            var query = context.Leases.AsNoTracking();

            query = query.GenerateCommonLeaseQuery(filter);

            return query;
        }

        /// <summary>
        /// Get the street address from the lease's first associated property.
        /// </summary>
        /// <param name="lease"></param>
        /// <returns></returns>
        public static string GetAddress(this Pims.Dal.Entities.Lease lease)
        {
            return lease?.Properties?.FirstOrDefault()?.Address?.StreetAddress1;
        }

        /// <summary>
        /// Get the pid or pin from the lease's first associated property.
        /// </summary>
        /// <param name="lease"></param>
        /// <returns></returns>
        public static int? GetPidOrPin(this Pims.Dal.Entities.Lease lease)
        {
            return lease?.Properties?.FirstOrDefault()?.PID ?? lease?.Properties.FirstOrDefault()?.PIN;
        }

        /// <summary>
        /// Get the Program name from the lease's program type description
        /// </summary>
        /// <param name="lease"></param>
        /// <returns></returns>
        public static string GetProgramName(this Pims.Dal.Entities.Lease lease)
        {
            return lease?.ProgramType?.Description;
        }

        /// <summary>
        /// Get the full name from the lease's first tenant (person).
        /// </summary>
        /// <param name="lease"></param>
        /// <returns></returns>
        public static string GetFullName(this Pims.Dal.Entities.Lease lease)
        {
            return lease?.TenantsManyToMany.FirstOrDefault(t => t.Person != null)?.Person?.GetFullName();
        }

        /// <summary>
        /// Get the full name from the lease's first tenant (person).
        /// </summary>
        /// <param name="lease"></param>
        /// <returns></returns>
        public static string GetMotiName(this Pims.Dal.Entities.Lease lease)
        {
            return lease.MotiName?.GetFullName();
        }

        /// <summary>
        /// Get the calculated expiry date.
        /// </summary>
        /// <param name="lease"></param>
        /// <returns></returns>
        public static DateTime? GetExpiryDate(this Pims.Dal.Entities.Lease lease)
        {
            if (lease.OrigExpiryDate != null)
            {
                if (lease.TermExpiryDate != null)
                {
                    return lease.OrigExpiryDate > lease.TermExpiryDate ? lease.OrigExpiryDate : lease.TermExpiryDate;
                }
                return lease.OrigExpiryDate;
            }
            return lease.TermExpiryDate;
        }
    }
}
