using Microsoft.EntityFrameworkCore;
using Pims.Core.Extensions;
using Pims.Dal.Exceptions;
using Pims.Dal.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Entity = Pims.Dal.Entities;

namespace Pims.Dal.Helpers.Extensions
{
    /// <summary>
    /// PropertyExtensions static class, provides extension methods for propertys.
    /// </summary>
    public static class PropertyExtensions
    {
        /// <summary>
        /// Generate an SQL statement for the specified 'user' and 'filter'.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="query"></param>
        /// <param name="user"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        private static IQueryable<Entity.Views.Property> GenerateCommonQuery(this PimsContext context, IQueryable<Entity.Views.Property> query, ClaimsPrincipal user, Entity.Models.AllPropertyFilter filter)
        {
            filter.ThrowIfNull(nameof(filter));
            filter.ThrowIfNull(nameof(user));

            // Check if user has the ability to view sensitive properties.
            var userAgencies = user.GetAgenciesAsNullable();
            var viewSensitive = user.HasPermission(Permissions.SensitiveView);
            var isAdmin = user.HasPermission(Permissions.AdminProperties);

            // By default do not include the following property classifications.
            if (filter.ClassificationId.HasValue)
                query = query.Where(p => p.ClassificationId == filter.ClassificationId);
            else
                query = query.Where(p => p.ClassificationId != (long)Entities.ClassificationTypes.Disposed
                    && p.ClassificationId != (long)Entities.ClassificationTypes.Demolished
                    && p.ClassificationId != (long)Entities.ClassificationTypes.Subdivided);

            // Users are not allowed to view sensitive properties outside of their agency or sub-agencies.
            if (!viewSensitive)
                query = query.Where(p => !p.IsSensitive);

            // Display buildings or land/subdivisions
            if (filter.PropertyType == Entities.PropertyTypes.Building)
                query = query.Where(p => p.PropertyTypeId == Entities.PropertyTypes.Building);
            else if (filter.PropertyType == Entities.PropertyTypes.Land)
                query = query.Where(p => p.PropertyTypeId == Entities.PropertyTypes.Land || p.PropertyTypeId == Entities.PropertyTypes.Subdivision);

            // Where rentable area is less than or equal to the filter.
            if (filter.RentableArea.HasValue)
                query = query.Where(p => p.RentableArea <= filter.RentableArea);

            if (filter.BareLandOnly == true)
            {
                query = from p in query
                        join pa in context.Parcels on new { p.Id, PropertyTypeId = (long)p.PropertyTypeId } equals new { pa.Id, PropertyTypeId = (long)pa.PropertyTypeId }
                        where p.PropertyTypeId == Entity.PropertyTypes.Land
                            && pa.Buildings.Count() == 0
                        select p;
            }

            if (filter.NELatitude.HasValue && filter.NELongitude.HasValue && filter.SWLatitude.HasValue && filter.SWLongitude.HasValue)
            {
                var poly = new NetTopologySuite.Geometries.Envelope(filter.NELongitude.Value, filter.SWLongitude.Value, filter.NELatitude.Value, filter.SWLatitude.Value).ToPolygon();
                query = query.Where(p => poly.Contains(p.Location));
            }

            if (filter.Agencies?.Any() == true)
            {
                IEnumerable<long?> filterAgencies;
                if (!isAdmin)
                {
                    // Users can only search their own agencies.
                    filterAgencies = filter.Agencies.Intersect(userAgencies.Select(a => (long)a)).Select(a => (long?)a);
                }
                else
                {
                    // TODO: Ideally this list would be provided by the frontend, as it is expensive to do it here.
                    // Get list of sub-agencies for any agency selected in the filter.
                    filterAgencies = filter.Agencies.Select(a => (long?)a);
                }
                if (filterAgencies.Any())
                {
                    var agencies = filterAgencies.Concat(context.Agencies.AsNoTracking().Where(a => filterAgencies.Contains(a.Id)).SelectMany(a => a.Children.Select(ac => (long?)ac.Id)).ToArray()).Distinct();
                    query = query.Where(p => agencies.Contains(p.AgencyId));
                }
            }
            if (filter.ParcelId.HasValue)
                query = query.Where(p => p.ParcelId == filter.ParcelId);
            if (!String.IsNullOrWhiteSpace(filter.ProjectNumber))
                query = query.Where(p => p.ProjectNumbers.Contains(filter.ProjectNumber));
            if (filter.IgnorePropertiesInProjects == true)
                query = query.Where(p => p.ProjectNumbers == null || p.ProjectNumbers == "[]");
            if (!String.IsNullOrWhiteSpace(filter.Description))
                query = query.Where(p => EF.Functions.Like(p.Description, $"%{filter.Description}%"));
            if (!String.IsNullOrWhiteSpace(filter.Name))
                query = query.Where(p => EF.Functions.Like(p.Name, $"%{filter.Name}%"));

            if (!String.IsNullOrWhiteSpace(filter.PID))
            {
                var pidValue = filter.PID.Replace("-", "").Trim();
                if (Int32.TryParse(pidValue, out int pid))
                    query = query.Where(p => p.PID == pid || p.PIN == pid);
            }
            if (!String.IsNullOrWhiteSpace(filter.AdministrativeArea))
                query = query.Where(p => EF.Functions.Like(p.AdministrativeArea, $"%{filter.AdministrativeArea}%"));
            if (!String.IsNullOrWhiteSpace(filter.Zoning))
                query = query.Where(p => EF.Functions.Like(p.Zoning, $"%{filter.Zoning}%"));
            if (!String.IsNullOrWhiteSpace(filter.ZoningPotential))
                query = query.Where(p => EF.Functions.Like(p.ZoningPotential, $"%{filter.ZoningPotential}%"));

            if (filter.ConstructionTypeId.HasValue)
                query = query.Where(p => p.BuildingConstructionTypeId == filter.ConstructionTypeId);
            if (filter.PredominateUseId.HasValue)
                query = query.Where(p => p.BuildingPredominateUseId == filter.PredominateUseId);
            if (filter.FloorCount.HasValue)
                query = query.Where(p => p.BuildingFloorCount == filter.FloorCount);
            if (!String.IsNullOrWhiteSpace(filter.Tenancy))
                query = query.Where(p => EF.Functions.Like(p.BuildingTenancy, $"%{filter.Tenancy}%"));

            if (!String.IsNullOrWhiteSpace(filter.Address))
                query = query.Where(p => EF.Functions.Like(p.Address, $"%{filter.Address}%") || EF.Functions.Like(p.AdministrativeArea, $"%{filter.Address}%"));

            if (filter.MinLandArea.HasValue)
                query = query.Where(p => p.LandArea >= filter.MinLandArea);
            if (filter.MaxLandArea.HasValue)
                query = query.Where(b => b.LandArea <= filter.MaxLandArea);

            if (filter.MinRentableArea.HasValue)
                query = query.Where(p => p.RentableArea >= filter.MinRentableArea);
            if (filter.MaxRentableArea.HasValue)
                query = query.Where(b => b.RentableArea <= filter.MaxRentableArea);

            if (filter.MinMarketValue.HasValue)
                query = query.Where(p => p.Market >= filter.MinMarketValue);
            if (filter.MaxMarketValue.HasValue)
                query = query.Where(p => p.Market <= filter.MaxMarketValue);

            if (filter.MinAssessedValue.HasValue)
                query = query.Where(p => p.AssessedLand >= filter.MinAssessedValue || p.AssessedBuilding >= filter.MinAssessedValue);
            if (filter.MaxAssessedValue.HasValue)
                query = query.Where(p => p.AssessedLand <= filter.MaxAssessedValue || p.AssessedBuilding <= filter.MaxAssessedValue);

            if (filter.Sort?.Any() == true)
                query = query.OrderByProperty(filter.Sort);
            else
                query = query.OrderBy(p => p.AgencyCode).ThenBy(p => p.PID).ThenBy(p => p.PIN).ThenBy(p => p.PropertyTypeId);

            return query;
        }

        /// <summary>
        /// Generate a query for the specified 'filter'.
        /// Only includes properties that belong to the user's agency or sub-agencies.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="user"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IQueryable<Entity.Views.Property> GenerateQuery(this PimsContext context, ClaimsPrincipal user, Entity.Models.AllPropertyFilter filter)
        {
            filter.ThrowIfNull(nameof(filter));
            filter.ThrowIfNull(nameof(user));

            // Users may only view sensitive properties if they have the `sensitive-view` claim and belong to the owning agency.
            var query = context.Properties.AsNoTracking();

            // Users can only view their agency or sub-agency properties.
            var isAdmin = user.HasPermission(Permissions.AdminProperties);
            if (!isAdmin)
            {
                var userAgencies = user.GetAgenciesAsNullable();
                query = query.Where(p => userAgencies.Contains(p.AgencyId));
            }

            query = context.GenerateCommonQuery(query, user, filter);

            return query;
        }

        /// <summary>
        /// Generate a query for the specified 'filter'.
        /// Returns all properties, even properties that don't belong to the user's agency or sub-agencies.
        /// The results of this query must be 'cleaned' so that only appropriate data is returned to API.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="user"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IQueryable<Entity.Views.Property> GenerateAllPropertyQuery(this PimsContext context, ClaimsPrincipal user, Entity.Models.AllPropertyFilter filter)
        {
            filter.ThrowIfNull(nameof(filter));
            filter.ThrowIfNull(nameof(user));

            var query = context.Properties.AsNoTracking();

            // Only return properties owned by user's agency or sub-agencies.
            if (!filter.IncludeAllProperties)
            {
                var userAgencies = user.GetAgenciesAsNullable();
                query = query.Where(p => userAgencies.Contains(p.AgencyId));
            }

            query = context.GenerateCommonQuery(query, user, filter);

            return query;
        }

        /// <summary>
        /// Throw an exception if the passed property is not in the same agency or sub agency as this project.
        /// </summary>
        /// <param name="parcel"></param>
        /// <param name="projectAgencyIds"></param>
        /// <returns></returns>
        public static void ThrowIfPropertyNotInProjectAgency(this Entity.Property parcel, IEnumerable<long> projectAgencyIds)
        {
            // properties may be in the same agency or sub-agency of a project. A parcel in a parent agency may not be added to a sub-agency project.
            if (!parcel.AgencyId.HasValue || !projectAgencyIds.Contains(parcel.AgencyId.Value))
                throw new InvalidOperationException("Properties may not be added to Projects with a different agency.");
        }

        /// <summary>
        /// Throw an exception if the passed property is in an SPP project that is in a non-draft status.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static void ThrowIfPropertyInSppProject(this Entity.Property property, ClaimsPrincipal user)
        {
            var isAdmin = user.HasPermission(Permissions.AdminProperties);
            if (!isAdmin && property?.ProjectNumbers?.Contains("SPP") == true) throw new NotAuthorizedException("User may not remove buildings that are in a SPP Project.");
        }

        /// <summary>
        /// The user may create an evaluation older then the most recent evaluation stored in PIMS. To support this, remove any evaluations that are within one year of the passed date.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="updatedProperty"></param>
        /// <param name="disposedOn"></param>
        public static void RemoveEvaluationsWithinOneYear(this Entity.Property property, Entity.Property updatedProperty, DateTime? disposedOn = null)
        {
            foreach (Entity.EvaluationKeys key in Enum.GetValues(typeof(Entity.EvaluationKeys)))
            {
                DateTime? mostRecentDate = null;
                DateTime? date = null;
                if (property is Entity.Parcel parcel)
                {
                    mostRecentDate = parcel.Evaluations.Where(d => d.Key == key).OrderByDescending(d => d.Date).FirstOrDefault()?.Date;
                    date = ((Entity.Parcel)updatedProperty).Evaluations.FirstOrDefault(e => e.Key == key)?.Date;
                }
                else if (property is Entity.Building building)
                {
                    mostRecentDate = building.Evaluations.Where(d => d.Key == key).OrderByDescending(d => d.Date).FirstOrDefault()?.Date;
                    date = ((Entity.Building)updatedProperty).Evaluations.FirstOrDefault(e => e.Key == key)?.Date;
                }
                //If the date passed in is the most recent, we don't need to do any removal logic.
                if (mostRecentDate == null || date == null || mostRecentDate == date)
                {
                    continue;
                }
                var maxDate = (disposedOn ?? date)?.AddYears(1);
                if (property is Entity.Parcel parcelProperty)
                {
                    parcelProperty.Evaluations.RemoveAll(e => e.Date > date && e.Date < maxDate && key == e.Key);
                }
                else if (property is Entity.Building building)
                {
                    building.Evaluations.RemoveAll(e => e.Date > date && e.Date < maxDate && key == e.Key);
                }
            }
        }
    }
}
