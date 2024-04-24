using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
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
        /// Generate a query for the specified 'filter'.
        /// Only includes properties that belong to the user's organization or sub-organizations.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="user"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IQueryable<Entity.PimsPropertyLocationVw> GeneratePropertyQuery(this PimsContext context, ClaimsPrincipal user, Entity.Models.PropertyFilter filter)
        {
            filter.ThrowIfNull(nameof(filter));
            filter.ThrowIfNull(nameof(user));

            var query = context.PimsPropertyLocationVws
                .AsNoTracking();

            var predicate = GenerateCommonPropertyQuery(user, filter);
            query = query.Where(predicate);

            if (filter.Sort?.Any() == true)
            {
                query = query.OrderByProperty(true, filter.Sort);
            }
            else
            {
                query = query.OrderBy(p => p.PropertyTypeCode);
            }

            return query;
        }

        /// <summary>
        /// Generate an SQL statement for the specified 'user' and 'filter'.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        private static ExpressionStarter<PimsPropertyLocationVw> GenerateCommonPropertyQuery(ClaimsPrincipal user, Entity.Models.PropertyFilter filter)
        {
            filter.ThrowIfNull(nameof(filter));
            filter.ThrowIfNull(nameof(user));

            // Check if user has the ability to view sensitive properties.
            var viewSensitive = user.HasPermission(Permissions.SensitiveView);

            var predicateBuilder = PredicateBuilder.New<PimsPropertyLocationVw>(p => true);

            // Users are not allowed to view sensitive properties outside of their organization or sub-organizations.
            if (!viewSensitive)
            {
                predicateBuilder = predicateBuilder.And(p => !p.IsSensitive);
            }

            if (!string.IsNullOrWhiteSpace(filter.PinOrPid))
            {
                // note - 2 part search required. all matches found by removing leading 0's, then matches filtered in subsequent step. This is because EF core does not support an lpad method.
                Regex nonInteger = new Regex("[^\\d]");
                var formattedPidPin = Convert.ToInt32(nonInteger.Replace(filter.PinOrPid, string.Empty)).ToString();
                predicateBuilder = predicateBuilder.And(p => EF.Functions.Like(p.Pid.ToString(), $"%{formattedPidPin}%") || EF.Functions.Like(p.Pin.ToString(), $"%{formattedPidPin}%"));
            }
            if (!string.IsNullOrWhiteSpace(filter.Address))
            {
                predicateBuilder = predicateBuilder.And(p => EF.Functions.Like(p.StreetAddress1, $"%{filter.Address}%") || EF.Functions.Like(p.MunicipalityName, $"%{filter.Address}%"));
            }
            if (!string.IsNullOrWhiteSpace(filter.PlanNumber))
            {
                predicateBuilder = predicateBuilder.And(p => p.SurveyPlanNumber.Equals(filter.PlanNumber));
            }

            var isRetired = filter.Ownership.Contains("isRetired");

            ExpressionStarter<PimsPropertyLocationVw> ownershipBuilder;

            if (filter.Ownership.Count > 0)
            {
                // Property ownership filters
                ownershipBuilder = isRetired ? PredicateBuilder.New<PimsPropertyLocationVw>(p => p.IsRetired == true) : PredicateBuilder.New<PimsPropertyLocationVw>(p => false);
                if (filter.Ownership.Contains("isCoreInventory"))
                {
                    ownershipBuilder = ownershipBuilder.Or(p => p.IsOwned && p.IsRetired != true);
                }
                if (filter.Ownership.Contains("isPropertyOfInterest"))
                {
                    ownershipBuilder.Or(p => p.IsRetired != true && p.HasActiveAcquisitionFile.HasValue && p.HasActiveAcquisitionFile.Value);
                    ownershipBuilder.Or(p => p.IsRetired != true && p.HasActiveResearchFile.HasValue && p.HasActiveResearchFile.Value);
                }
                if (filter.Ownership.Contains("isOtherInterest"))
                {
                    ownershipBuilder.Or(p => p.IsRetired != true && p.IsOtherInterest.HasValue && p.IsOtherInterest.Value);
                }
                if (filter.Ownership.Contains("isDisposed"))
                {
                    ownershipBuilder.Or(p => p.IsRetired != true && p.IsDisposed.HasValue && p.IsDisposed.Value);
                }
            }
            else
            {
                // psp-7658 is retired properties should be omitted by default.
                ownershipBuilder = PredicateBuilder.New<PimsPropertyLocationVw>(p => p.IsRetired != true);
            }
            predicateBuilder = predicateBuilder.And(ownershipBuilder);

            return predicateBuilder;
        }
    }
}
