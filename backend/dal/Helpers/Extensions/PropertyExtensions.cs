using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Pims.Core.Extensions;
using Pims.Dal.Security;
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
        private static IQueryable<Entity.PimsProperty> GenerateCommonPropertyQuery(this IQueryable<Entity.PimsProperty> query, ClaimsPrincipal user, Entity.Models.PropertyFilter filter)
        {
            filter.ThrowIfNull(nameof(filter));
            filter.ThrowIfNull(nameof(user));

            // Check if user has the ability to view sensitive properties.
            var viewSensitive = user.HasPermission(Permissions.SensitiveView);

            // Users are not allowed to view sensitive properties outside of their organization or sub-organizations.
            if (!viewSensitive)
            {
                query = query.Where(p => !(p.IsSensitive.HasValue && p.IsSensitive.Value));
            }

            if (!string.IsNullOrWhiteSpace(filter.PinOrPid))
            {
                var pinOrPidValue = filter.PinOrPid.Replace("-", "").Trim().TrimStart('0');
                query = query.Where(p => p != null && (EF.Functions.Like(p.Pid.ToString(), $"%{pinOrPidValue}%") || EF.Functions.Like(p.Pin.ToString(), $"%{pinOrPidValue}%")));
            }
            if (!string.IsNullOrWhiteSpace(filter.Address))
            {
                query = query.Where(p => EF.Functions.Like(p.Address.StreetAddress1, $"%{filter.Address}%") || EF.Functions.Like(p.Address.MunicipalityName, $"%{filter.Address}%"));
            }

            if (filter.Sort?.Any() == true)
            {
                query = query.OrderByProperty(filter.Sort);
            }
            else
            {
                query = query.OrderBy(p => p.PropertyTypeCode);
            }

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
        public static IQueryable<Entity.PimsProperty> GeneratePropertyQuery(this PimsContext context, ClaimsPrincipal user, Entity.Models.PropertyFilter filter)
        {
            filter.ThrowIfNull(nameof(filter));
            filter.ThrowIfNull(nameof(user));

            // Users may only view sensitive properties if they have the `sensitive-view` claim and belong to the owning organization.
            var query = context.PimsProperties
                .Include(p => p.Address)
                .ThenInclude(a => a.RegionCodeNavigation)
                .Include(p => p.Address)
                .ThenInclude(a => a.DistrictCodeNavigation)
                .Include(p => p.Address)
                .ThenInclude(a => a.ProvinceState)
                .Include(p => p.Address)
                .ThenInclude(a => a.Country)
                .AsNoTracking();

            query = query.GenerateCommonPropertyQuery(user, filter);

            return query;
        }
    }
}
