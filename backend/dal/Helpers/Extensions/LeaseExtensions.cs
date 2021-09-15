using Microsoft.EntityFrameworkCore;
using Pims.Core.Extensions;
using System;
using System.Linq;
using System.Security.Claims;
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
        /// <param name="user"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        private static IQueryable<Entity.Lease> GenerateCommonLeaseQuery(this IQueryable<Entity.Lease> query, ClaimsPrincipal user, Entity.Models.LeaseFilter filter)
        {
            filter.ThrowIfNull(nameof(filter));
            filter.ThrowIfNull(nameof(user));

            if (!String.IsNullOrWhiteSpace(filter.TenantName)) {
                query = query.Where(l => l.Tenant != null && EF.Functions.Like(l.Tenant.Surname + ", " + l.Tenant.FirstName + ", " + l.Tenant.MiddleNames, $"%{filter.TenantName}%"));
            }

            if (!String.IsNullOrWhiteSpace(filter.PidOrPin))
            {
                var pidOrPinValue = filter.PidOrPin.Replace("-", "").Trim();
                if (Int32.TryParse(pidOrPinValue, out int pidOrPin)) {
                    query = query.Where(l => l.Properties.FirstOrDefault() != null && (l.Properties.FirstOrDefault().PID == pidOrPin || l.Properties.FirstOrDefault().PIN == pidOrPin));
                }
            }

            if (!String.IsNullOrWhiteSpace(filter.LFileNo))
                query = query.Where(l => EF.Functions.Like(l.LFileNo, $"%{filter.LFileNo}%"));

            if (filter.Sort?.Any() == true)
                query = query.OrderByProperty(filter.Sort);
            else
                query = query.OrderBy(l => l.LFileNo);

            return query.Include(l => l.Properties)
                .ThenInclude(p => p.Address)
                .Include(l => l.ProgramType)
                .Include(l => l.Tenant);
        }

        /// <summary>
        /// Generate a query for the specified 'filter'.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="user"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IQueryable<Entity.Lease> GenerateLeaseQuery(this PimsContext context, ClaimsPrincipal user, Entity.Models.LeaseFilter filter)
        {
            if (context == null) throw new ArgumentNullException("GenerateLeaseQuery context cannot be null");
            filter.ThrowIfNull(nameof(filter));
            filter.ThrowIfNull(nameof(user));

            var query = context.Leases.AsNoTracking();

            query = query.GenerateCommonLeaseQuery(user, filter);

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
        /// Get the full name from the lease's tenant (person).
        /// </summary>
        /// <param name="lease"></param>
        /// <returns></returns>
        public static string GetFullName(this Pims.Dal.Entities.Lease lease)
        {
            if(lease?.Tenant != null)
            {
                string[] names = { lease.Tenant.Surname, lease.Tenant.FirstName, lease.Tenant.MiddleNames };
                return String.Join(", ", names.Where(n => n != null && n.Trim() != String.Empty));
            }
            return null;
        }
    }
}
