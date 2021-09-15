using Microsoft.EntityFrameworkCore;
using Pims.Core.Extensions;
using Pims.Dal.Security;
using System;
using System.Linq;
using System.Security.Claims;
using Entity = Pims.Dal.Entities;

namespace Pims.Dal.Helpers.Extensions
{
    /// <summary>
    /// PropertyExtensions static class, provides extension methods for properties.
    /// </summary>
    public static class PropertyExtensions
    {
        /// <summary>
        /// Generate an SQL statement for the specified 'user' and 'filter'.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="user"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        private static IQueryable<Entity.Property> GenerateCommonPropertyQuery(this IQueryable<Entity.Property> query, ClaimsPrincipal user, Entity.Models.PropertyFilter filter)
        {
            filter.ThrowIfNull(nameof(filter));
            filter.ThrowIfNull(nameof(user));

            // Check if user has the ability to view sensitive properties.
            var userOrganizations = user.GetOrganizationsAsNullable();
            var viewSensitive = user.HasPermission(Permissions.SensitiveView);
            var isAdmin = user.HasPermission(Permissions.AdminProperties);

            // Users are not allowed to view sensitive properties outside of their organization or sub-organizations.
            if (!viewSensitive)
                query = query.Where(p => !p.IsSensitive);

            if (!String.IsNullOrWhiteSpace(filter.ClassificationId))
                query = query.Where(p => p.ClassificationId == filter.ClassificationId);
            if (!String.IsNullOrWhiteSpace(filter.PropertyTypeId))
                query = query.Where(p => p.PropertyTypeId == filter.PropertyTypeId);
            if (!String.IsNullOrWhiteSpace(filter.TenureId))
                query = query.Where(p => p.TenureId == filter.TenureId);
            if (filter.NELatitude.HasValue && filter.NELongitude.HasValue && filter.SWLatitude.HasValue && filter.SWLongitude.HasValue)
            {
                var poly = new NetTopologySuite.Geometries.Envelope(filter.NELongitude.Value, filter.SWLongitude.Value, filter.NELatitude.Value, filter.SWLatitude.Value).ToPolygon();
                query = query.Where(p => poly.Contains(p.Location));
            }
            if (filter.Organizations?.Any() == true)
                query = query.Where(p => p.OrganizationsManyToMany.Any(o => filter.Organizations.Contains(o.OrganizationId)));
            if (!String.IsNullOrWhiteSpace(filter.Name))
                query = query.Where(p => EF.Functions.Like(p.Name, $"%{filter.Name}%"));

            if (!String.IsNullOrWhiteSpace(filter.PID))
            {
                var pidValue = filter.PID.Replace("-", "").Trim();
                if (Int32.TryParse(pidValue, out int pid))
                    query = query.Where(p => p.PID == pid || p.PIN == pid);
            }
            if (filter.PIN.HasValue)
                query = query.Where(p => p.PIN == filter.PIN);

            if (!String.IsNullOrWhiteSpace(filter.Address))
                query = query.Where(p => EF.Functions.Like(p.Address.StreetAddress1, $"%{filter.Address}%") || EF.Functions.Like(p.Address.Municipality, $"%{filter.Address}%"));

            if (filter.Sort?.Any() == true)
                query = query.OrderByProperty(filter.Sort);
            else
                query = query.OrderBy(p => p.PropertyTypeId);

            return query;
        }

        /// <summary>
        /// Generate a query for the specified 'filter'.
        /// Only includes properties that belong to the user's organization or sub-organizations.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="user"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IQueryable<Entity.Property> GeneratePropertyQuery(this PimsContext context, ClaimsPrincipal user, Entity.Models.PropertyFilter filter)
        {
            filter.ThrowIfNull(nameof(filter));
            filter.ThrowIfNull(nameof(user));

            // Users may only view sensitive properties if they have the `sensitive-view` claim and belong to the owning organization.
            var query = context.Properties.AsNoTracking();

            query = query.GenerateCommonPropertyQuery(user, filter);

            return query;
        }

        /// <summary>
        /// Return the pid (if valued) or pin of the property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static int? GetPidOrPin(this Pims.Dal.Entities.Property property)
        {
            return property.PID != 0 ? property.PID : property.PIN;
        }

        /// <summary>
        /// Get the Tenant Name of the first associated lease to this property.
        /// </summary>
        /// <param name="lease"></param>
        /// <returns></returns>
        public static string GetTenantName(this Pims.Dal.Entities.Property property)
        {
            return property.Leases.FirstOrDefault()?.GetFullName();
        }

        /// <summary>
        /// Get the Tenant Name of the first associated lease to this property.
        /// </summary>
        /// <param name="lease"></param>
        /// <returns></returns>
        public static DateTime? GetExpiryDate(this Pims.Dal.Entities.Property property)
        {
            return property.Leases.FirstOrDefault()?.ExpiryDate;
        }
    }
}
