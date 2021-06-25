using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Pims.Dal.Services
{
    /// <summary>
    /// ParcelService class, provides a service layer to interact with parcels within the datasource.
    /// </summary>
    public class ParcelService : BaseService<Parcel>, IParcelService
    {
        #region Variables
        private readonly PimsOptions _options;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ParcelService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public ParcelService(IOptions<PimsOptions> options, PimsContext dbContext, ClaimsPrincipal user, IPimsService service, ILogger<ParcelService> logger) : base(dbContext, user, service, logger)
        {
            _options = options.Value;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get an array of parcels within the specified filter.
        /// Will not return sensitive parcels unless the user has the `sensitive-view` claim and belongs to the owning agency.
        /// </summary>
        /// <param name="neLat"></param>
        /// <param name="neLong"></param>
        /// <param name="swLat"></param>
        /// <param name="swLong"></param>
        /// <returns></returns>
        public IEnumerable<Parcel> Get(double neLat, double neLong, double swLat, double swLong)
        {
            this.User.ThrowIfNotAuthorized(Permissions.PropertyView);
            // Check if user has the ability to view sensitive properties.
            var userAgencies = this.User.GetAgenciesAsNullable();
            var viewSensitive = this.User.HasPermission(Security.Permissions.SensitiveView);
            var isAdmin = this.User.HasPermission(Permissions.AdminProperties);

            // Users may only view sensitive properties if they have the `sensitive-view` claim and belong to the owning agency.
            var query = this.Context.Parcels.AsNoTracking().Where(p =>
                (isAdmin
                    || p.IsVisibleToOtherAgencies
                    || ((!p.IsSensitive || viewSensitive)
                        && userAgencies.Contains(p.AgencyId))));

            var pfactory = new NetTopologySuite.Geometries.GeometryFactory();
            var ring = new NetTopologySuite.Geometries.LinearRing(
                new[] {
                        new NetTopologySuite.Geometries.Coordinate(neLong, neLat),
                        new NetTopologySuite.Geometries.Coordinate(swLong, neLat),
                        new NetTopologySuite.Geometries.Coordinate(swLong, swLat),
                        new NetTopologySuite.Geometries.Coordinate(neLong, swLat),
                        new NetTopologySuite.Geometries.Coordinate(neLong, neLat)
                });
            var poly = pfactory.CreatePolygon(ring);
            poly.SRID = 4326;
            query = query.Where(p => poly.Contains(p.Location));

            return query.ToArray();
        }

        /// <summary>
        /// Get an array of parcels within the specified filter.
        /// Will not return sensitive parcels unless the user has the `sensitive-view` claim and belongs to the owning agency.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<Parcel> Get(ParcelFilter filter)
        {
            this.User.ThrowIfNotAuthorized(Permissions.PropertyView);
            var query = this.Context.GenerateQuery(this.User, filter);

            return query.ToArray();
        }

        /// <summary>
        /// Get an array of parcels within the specified filter.
        /// Will not return sensitive parcels unless the user has the `sensitive-view` claim and belongs to the owning agency.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Paged<Parcel> GetPage(ParcelFilter filter)
        {
            this.User.ThrowIfNotAuthorized(Permissions.PropertyView);

            var query = this.Context.GenerateQuery(this.User, filter);
            var total = query.Count();
            var items = query.Skip((filter.Page - 1) * filter.Quantity).Take(filter.Quantity);

            return new Paged<Parcel>(items, filter.Page, filter.Quantity, total);
        }

        /// <summary>
        /// Get the parcel for the specified 'id'.
        /// Will not return sensitive buildings unless the user has the `sensitive-view` claim and belongs to the owning agency.
        /// </summary>
        /// <param name="id"></param>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <returns></returns>
        public Parcel Get(long id)
        {
            this.User.ThrowIfNotAuthorized(Permissions.PropertyView);
            // Check if user has the ability to view sensitive properties.
            var userAgencies = this.User.GetAgenciesAsNullable();
            var viewSensitive = this.User.HasPermission(Permissions.SensitiveView);
            var isAdmin = this.User.HasPermission(Permissions.AdminProperties);

            var ownsABuilding = this.Context.ParcelBuildings
                .Any(pb => pb.ParcelId == id
                    && userAgencies.Contains(pb.Building.AgencyId));

            var parcel = this.Context.Parcels
                .AsNoTracking()
                .Include(p => p.Classification)
                .Include(p => p.Address)
                .Include(p => p.Address.Province)
                .Include(p => p.Agency)
                .Include(p => p.Agency.Parent)
                .Include(p => p.Evaluations)
                .Include(p => p.Fiscals)
                .Include(p => p.Buildings)
                .Include(p => p.Buildings).ThenInclude(b => b.Address)
                .Include(p => p.Buildings).ThenInclude(b => b.Address.Province)
                .Include(p => p.Buildings).ThenInclude(b => b.BuildingConstructionType)
                .Include(p => p.Buildings).ThenInclude(b => b.BuildingPredominateUse)
                .Include(p => p.Buildings).ThenInclude(b => b.BuildingOccupantType)
                .Include(p => p.Buildings).ThenInclude(b => b.Evaluations)
                .Include(p => p.Buildings).ThenInclude(b => b.Fiscals)
                .Include(p => p.Buildings).ThenInclude(b => b.Classification)
                .Include(p => p.Parcels)
                .Include(p => p.Subdivisions)
                .FirstOrDefault(p => p.Id == id
                    && (ownsABuilding || isAdmin || p.IsVisibleToOtherAgencies || !p.IsSensitive || (viewSensitive && userAgencies.Contains(p.AgencyId)))) ?? throw new KeyNotFoundException();

            // Remove any sensitive buildings from the results if the user is not allowed to view them.
            if (!viewSensitive)
            {
                parcel?.Buildings.RemoveAll(pb => pb.IsSensitive);
            }

            // Remove any properties not owned by user's agency.
            if (!isAdmin)
            {
                parcel?.Buildings.RemoveAll(pb => !userAgencies.Contains(pb.AgencyId));
            }

            return parcel;
        }

        /// <summary>
        /// Add the specified parcel to the datasource.
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns></returns>
        public Parcel Add(Parcel parcel)
        {
            parcel.ThrowIfNull(nameof(parcel));
            parcel.PropertyTypeId = (long)(parcel.Parcels.Count > 0 || parcel.ParcelsManyToMany.Count > 0 ? PropertyTypes.Subdivision : PropertyTypes.Land);
            if (parcel.PropertyTypeId == (long)PropertyTypes.Subdivision)
            {
                var parentPid = this.Context.Parcels.Find(parcel.Parcels.FirstOrDefault()?.PID ?? parcel.ParcelsManyToMany.FirstOrDefault()?.ParcelId)?.PID;
                if (parentPid == null) throw new InvalidOperationException("Invalid parent parcel associated to subdivision, parent parcels must contain a valid PID");
                parcel.PID = parentPid.Value;
                parcel.PIN = this.Context.GetUniquePidPin(parcel.PID);
            }
            this.User.ThrowIfNotAuthorized(new[] { Permissions.PropertyAdd, Permissions.AdminProperties });

            var agency = this.User.GetAgency(this.Context) ??
                throw new NotAuthorizedException("User must belong to an agency before adding parcels.");

            if ((parcel.Parcels.Count > 0 && parcel.Subdivisions.Count > 0) || (parcel.ParcelsManyToMany.Count > 0 && parcel.SubdivisionsManyToMany.Count > 0))
                throw new InvalidOperationException("Parcel may only have associated parcels or subdivisions, not both.");

            this.Context.Parcels.ThrowIfNotUnique(parcel);
            // If the user is not an admin, and their agency is not a parent override to their user agency
            if (!this.User.HasPermission(Permissions.AdminProperties) && agency.ParentId != null)
            {
                parcel.AgencyId = agency.Id;
            }

            // To reduce issues a new parcel should only reference relationships through foreign key.
            // This will stop attempts to add duplicate entities to the db.
            parcel.Address.Province = this.Context.Provinces.Find(parcel.Address.ProvinceId);
            parcel.Agency = null;
            parcel.PropertyType = null;
            parcel.Classification = null;
            parcel.IsVisibleToOtherAgencies = false;

            parcel.ParcelsManyToMany.ForEach(pp =>
            {
                pp.Subdivision = parcel;
                pp.Parcel = this.Context.Parcels.Find(pp.ParcelId);
            });
            parcel.SubdivisionsManyToMany.ForEach(pp =>
            {
                pp.Parcel = parcel;
                pp.Subdivision = this.Context.Parcels.Find(pp.SubdivisionId);
            });

            // TODO: loops below are problematic.  They don't deal with different entity state.
            parcel.Buildings.ForEach(b =>
            {
                var exists = this.Context.Buildings.Find(b.Id);
                if (exists == null)
                {
                    b.Agency = null;
                    b.Address.AdministrativeArea = parcel.Address.AdministrativeArea;
                    b.Address.Province = this.Context.Provinces.Find(parcel.Address.ProvinceId);
                    b.Classification = null;
                    b.BuildingConstructionType = null;
                    b.BuildingOccupantType = null;
                    b.BuildingPredominateUse = null;
                }
            });
            parcel.BuildingsManyToMany.Where(pb => pb.Building != null).ForEach(pb =>
            {
                var exists = this.Context.Buildings.Find(pb.BuildingId);
                if (exists == null)
                {
                    pb.Parcel = parcel;
                    pb.Building.Agency = null;
                    pb.Building.Address.AdministrativeArea = parcel.Address.AdministrativeArea;
                    pb.Building.Address.Province = this.Context.Provinces.Find(parcel.Address.ProvinceId);
                    pb.Building.Classification = null;
                    pb.Building.BuildingConstructionType = null;
                    pb.Building.BuildingOccupantType = null;
                    pb.Building.BuildingPredominateUse = null;
                }
            });

            this.Context.Parcels.Add(parcel);
            this.Context.CommitTransaction();
            return parcel;
        }

        /// <summary>
        /// Update the specified parcel in the datasource.
        /// </summary>
        /// <param name="parcel"></param>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <returns></returns>
        public Parcel Update(Parcel parcel)
        {
            PendingUpdate(parcel);
            this.Context.SaveChanges();
            this.Context.CommitTransaction();
            return Get(parcel.Id);
        }

        /// <summary>
        /// Update the specified parcel financial values in the datasource.
        /// </summary>
        /// <param name="parcel"></param>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <returns></returns>
        public Parcel UpdateFinancials(Parcel parcel)
        {
            parcel.ThrowIfNotAllowedToEdit(nameof(parcel), this.User, new[] { Permissions.PropertyEdit, Permissions.AdminProperties });
            var isAdmin = this.User.HasPermission(Permissions.AdminProperties);

            var originalParcel = this.Context.Parcels
                .Include(p => p.Evaluations)
                .Include(p => p.Fiscals)
                .SingleOrDefault(p => p.Id == parcel.Id) ?? throw new KeyNotFoundException();

            var userAgencies = this.User.GetAgencies();
            var originalAgencyId = (long)this.Context.Entry(originalParcel).OriginalValues[nameof(Parcel.AgencyId)];
            var allowEdit = isAdmin || userAgencies.Contains(originalAgencyId);
            if (!allowEdit) throw new NotAuthorizedException("User may not edit parcels outside of their agency.");

            originalParcel.ThrowIfPropertyInSppProject(this.User);

            this.Context.UpdateParcelFinancials(originalParcel, parcel.Evaluations, parcel.Fiscals);

            this.Context.SetOriginalRowVersion(originalParcel);
            this.Context.CommitTransaction();
            return Get(parcel.Id);
        }

        /// <summary>
        /// Update the specified parcel in the datasource, but do not commit the transaction.
        /// </summary>
        /// <param name="parcel"></param>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <returns></returns>
        public Parcel PendingUpdate(Parcel parcel)
        {
            parcel.ThrowIfNotAllowedToEdit(nameof(parcel), this.User, new[] { Permissions.PropertyEdit, Permissions.AdminProperties });
            var isAdmin = this.User.HasPermission(Permissions.AdminProperties);

            var originalParcel = this.Context.Parcels
                .Include(p => p.Agency)
                .Include(p => p.Address)
                .Include(p => p.Evaluations)
                .Include(p => p.Fiscals)
                .Include(p => p.BuildingsManyToMany).ThenInclude(p => p.Building).ThenInclude(b => b.Evaluations)
                .Include(p => p.BuildingsManyToMany).ThenInclude(p => p.Building).ThenInclude(b => b.Fiscals)
                .Include(p => p.BuildingsManyToMany).ThenInclude(p => p.Building).ThenInclude(b => b.Address)
                .Include(p => p.ParcelsManyToMany).ThenInclude(p => p.Parcel)
                .Include(p => p.SubdivisionsManyToMany).ThenInclude(p => p.Subdivision)
                .SingleOrDefault(p => p.Id == parcel.Id) ?? throw new KeyNotFoundException();

            var userAgencies = this.User.GetAgencies();
            var originalAgencyId = (long)this.Context.Entry(originalParcel).OriginalValues[nameof(Parcel.AgencyId)];
            var allowEdit = isAdmin || userAgencies.Contains(originalAgencyId);
            var ownsABuilding = originalParcel.Buildings.Any(pb => userAgencies.Contains(pb.AgencyId.Value));
            if (!allowEdit && !ownsABuilding) throw new NotAuthorizedException("User may not edit parcels outside of their agency.");

            parcel.PropertyTypeId = originalParcel.PropertyTypeId; // property type cannot be changed directly.
            if (parcel.PropertyTypeId == (long)PropertyTypes.Subdivision && (parcel.Parcels.Any() || parcel.ParcelsManyToMany.Any()))
            {
                var parentPid = parcel.Parcels.FirstOrDefault()?.PID ?? parcel.ParcelsManyToMany.FirstOrDefault()?.Parcel?.PID;
                if (parentPid == null) throw new InvalidOperationException("Invalid parent parcel associated to subdivision, parent parcels must contain a valid PID");
                parcel.PID = parentPid.Value;
            }

            originalParcel.ThrowIfPropertyInSppProject(this.User);

            // Do not allow switching agencies through this method.
            if (!isAdmin && originalAgencyId != parcel.AgencyId) throw new NotAuthorizedException("Parcel cannot be transferred to the specified agency.");

            // Do not allow making property visible through this service.
            if (originalParcel.IsVisibleToOtherAgencies != parcel.IsVisibleToOtherAgencies) throw new InvalidOperationException("Parcel cannot be made visible to other agencies through this service.");

            // Only administrators can dispose a property.
            if (!isAdmin && parcel.ClassificationId == (long)ClassificationTypes.Disposed) throw new NotAuthorizedException("Parcel classification cannot be changed to disposed."); // TODO: Classification '4' should be a config settings.

            // Only administrators can set parcel to subdivided
            if (!isAdmin && parcel.ClassificationId == (long)ClassificationTypes.Subdivided) throw new NotAuthorizedException("Parcel classification cannot be changed to subdivided.");

            // Only buildings can be set to demolished
            if (parcel.ClassificationId == (long)ClassificationTypes.Demolished) throw new NotAuthorizedException("Only buildings may be set to demolished.");

            if ((parcel.Parcels.Count > 0 && parcel.Subdivisions.Count > 0)
                || (originalParcel.Parcels.Count > 0 && parcel.Subdivisions.Count > 0)
                || (originalParcel.Subdivisions.Count > 0 && parcel.Parcels.Count > 0)
                || (parcel.ParcelsManyToMany.Count > 0 && parcel.SubdivisionsManyToMany.Count > 0)
                || (originalParcel.ParcelsManyToMany.Count > 0 && parcel.SubdivisionsManyToMany.Count > 0)
                || (originalParcel.SubdivisionsManyToMany.Count > 0 && parcel.ParcelsManyToMany.Count > 0)) throw new InvalidOperationException("Parcel may only have associated parcels or subdivisions, not both.");

            // Users who don't own the parcel, but only own a building cannot update the parcel.
            if (allowEdit)
            {
                parcel.PropertyTypeId = originalParcel.PropertyTypeId;
                this.Context.Entry(originalParcel.Address).CurrentValues.SetValues(parcel.Address);
                this.Context.Entry(originalParcel).CurrentValues.SetValues(parcel);
                this.Context.SetOriginalRowVersion(originalParcel);
            }

            foreach (var building in parcel.Buildings)
            {
                // Check if the building already exists.
                var existingBuilding = originalParcel.Buildings.FirstOrDefault(b => b.Id == building.Id);

                // Reset all relationships that are not changed through this update.
                building.Address.Province = this.Context.Provinces.FirstOrDefault(p => p.Id == building.Address.ProvinceId);
                building.BuildingConstructionType = this.Context.BuildingConstructionTypes.FirstOrDefault(b => b.Id == building.BuildingConstructionTypeId);
                building.BuildingOccupantType = this.Context.BuildingOccupantTypes.FirstOrDefault(b => b.Id == building.BuildingOccupantTypeId);
                building.BuildingPredominateUse = this.Context.BuildingPredominateUses.FirstOrDefault(b => b.Id == building.BuildingPredominateUseId);
                building.Agency = this.Context.Agencies.FirstOrDefault(a => a.Id == building.AgencyId);

                if (existingBuilding == null)
                {
                    if (!allowEdit) throw new NotAuthorizedException("User may not add properties to a parcel they don't own.");

                    originalParcel.Buildings.Add(building);
                }
                else
                {
                    building.PropertyTypeId = existingBuilding.PropertyTypeId;
                    this.ThrowIfNotAllowedToUpdate(existingBuilding, _options.Project);

                    if (!allowEdit && !userAgencies.Contains(existingBuilding.AgencyId.Value)) throw new NotAuthorizedException("User may not update a property they don't own.");
                    // Do not allow switching agencies through this method.
                    if (!isAdmin && existingBuilding.AgencyId != building.AgencyId) throw new NotAuthorizedException("Building cannot be transferred to the specified agency.");

                    this.Context.Entry(existingBuilding).CurrentValues.SetValues(building);
                    this.Context.Entry(existingBuilding.Address).CurrentValues.SetValues(building.Address);
                    this.Context.UpdateBuildingFinancials(existingBuilding, building.Evaluations, building.Fiscals);
                }
            }

            // This property is a divided parcel with child subdivision parcels.
            if (parcel.Subdivisions.Count > 0 || originalParcel.Subdivisions.Count > 0
                || parcel.SubdivisionsManyToMany.Count > 0 || originalParcel.SubdivisionsManyToMany.Count > 0)
            {
                // loop through all passed in subdivisions, add any new subdivisions and remove any missing subdivisions from the current divided parcel.
                var subIds = parcel.Subdivisions.Any() ? parcel.Subdivisions.Select(pb => pb.Id) : parcel.SubdivisionsManyToMany.Select(s => s.SubdivisionId);
                foreach (var subdivisionId in subIds)
                {
                    // Check if the subdivision already exists.
                    var existingSubdivision = originalParcel.Subdivisions.FirstOrDefault(p => p.Id == subdivisionId)
                        ?? originalParcel.SubdivisionsManyToMany.FirstOrDefault(p => p.SubdivisionId == subdivisionId)?.Subdivision;

                    //Just add any new subdivisions, users cannot edit a subdivision within a parent parcel.
                    if (existingSubdivision == null)
                    {
                        if (!allowEdit) throw new NotAuthorizedException("User may not add subdivisions to a parcel they don't own.");
                        // This parcel is a divided parent parcel with one or more subdivisions. Therefore, add the current parcel id as the parent divided ParcelId with a ParcelParcel relationship to the new subdivisionId.
                        var pp = new ParcelParcel() { SubdivisionId = subdivisionId, ParcelId = parcel.Id };

                        originalParcel.SubdivisionsManyToMany.Add(pp);
                    }
                }

                var subdivisionIds = originalParcel.Subdivisions.Any() ? originalParcel.Subdivisions.Select(s => s.Id) : originalParcel.SubdivisionsManyToMany.Select(s => s.SubdivisionId);
                foreach (var subdivisionId in subdivisionIds)
                {
                    // Delete the subdivisions that have been removed from this parent divided parcel.
                    var found = parcel.Subdivisions.Any() ? !parcel.Subdivisions.Any(e => e.Id == subdivisionId) : !parcel.SubdivisionsManyToMany.Any(s => s.SubdivisionId == subdivisionId);
                    if (found)
                    {
                        var subdivision = this.Context.Parcels.Find(subdivisionId);
                        this.Context.Remove(subdivision);
                    }
                }
            }
            else
            {
                // This property is a Subdivision with parent divided parcels.
                // loop through all passed in owning divided parcels, adding any new divided parcels and removing any missing divided parcels from the current subdivision.
                var parIds = parcel.Parcels.Any() ? parcel.Parcels.Select(pb => pb.Id) : parcel.ParcelsManyToMany.Select(s => s.ParcelId);
                foreach (var dividedParcelId in parIds)
                {
                    // Check if the subdivided parcel already exists.
                    var existingDividedParcel = originalParcel.Parcels.FirstOrDefault(pb => pb.Id == dividedParcelId)
                        ?? originalParcel.ParcelsManyToMany.FirstOrDefault(p => p.ParcelId == dividedParcelId)?.Parcel;

                    //Just add any new divided parcels, users cannot edit a parcel within a subdivision.
                    if (existingDividedParcel == null)
                    {
                        if (!allowEdit) throw new NotAuthorizedException("User may not add divided parcels to a subdivision they don't own.");
                        // This parcel is a subdivision with one or more parent divided parcels. Therefore, add the current parcel id as the SubdivisionId with a ParcelParcel relationship to the new dividedParcelId.
                        var pp = new ParcelParcel() { SubdivisionId = parcel.Id, ParcelId = dividedParcelId };

                        originalParcel.ParcelsManyToMany.Add(pp);
                    }
                }

                var parentParcelIds = originalParcel.Parcels.Any() ? originalParcel.Parcels.Select(p => p.Id) : originalParcel.ParcelsManyToMany.Select(s => s.ParcelId);
                foreach (var parentParcelId in parentParcelIds)
                {
                    // Delete the divided parcels that have been removed from this subdivision parcel.
                    var remove = parcel.Parcels.Any() ? !parcel.Parcels.Any(e => e.Id == parentParcelId) : !parcel.ParcelsManyToMany.Any(s => s.ParcelId == parentParcelId);
                    if (remove)
                    {
                        var parentParcel = this.Context.ParcelParcels.FirstOrDefault(pp => pp.ParcelId == parentParcelId && pp.SubdivisionId == parcel.Id);
                        this.Context.Remove(parentParcel);
                    }
                }
            }

            if (allowEdit)
            {
                this.Context.UpdateParcelFinancials(originalParcel, parcel.Evaluations, parcel.Fiscals);

                // Go through the existing buildings and see if they have been deleted from the updated parcel.
                // If they have been removed, delete them from the datasource.
                var buildings = originalParcel.Buildings.Any() ? originalParcel.Buildings : originalParcel.BuildingsManyToMany.Select(b => b.Building);
                foreach (var building in buildings)
                {
                    var updateBuilding = parcel.Buildings.FirstOrDefault(pb => pb.Id == building.Id)
                        ?? parcel.BuildingsManyToMany.FirstOrDefault(b => b.BuildingId == building.Id)?.Building;

                    // The building may have evaluations or fiscals that need to be deleted.
                    foreach (var buildingEvaluation in building.Evaluations)
                    {
                        // Delete the evaluations that have been removed.
                        if (!updateBuilding.Evaluations.Any(e => e.BuildingId == buildingEvaluation.BuildingId && e.Date == buildingEvaluation.Date && e.Key == buildingEvaluation.Key))
                        {
                            this.Context.BuildingEvaluations.Remove(buildingEvaluation);
                        }
                    }
                    foreach (var buildingFiscal in building.Fiscals)
                    {
                        // Delete the fiscals that have been removed.
                        if (!updateBuilding.Fiscals.Any(e => e.BuildingId == buildingFiscal.BuildingId && e.FiscalYear == buildingFiscal.FiscalYear && e.Key == buildingFiscal.Key))
                        {
                            this.Context.BuildingFiscals.Remove(buildingFiscal);
                        }
                    }
                }

                foreach (var parcelEvaluation in originalParcel.Evaluations)
                {
                    // Delete the evaluations from the parcel that have been removed.
                    if (!parcel.Evaluations.Any(e => (e.ParcelId == parcelEvaluation.ParcelId && e.Date == parcelEvaluation.Date && e.Key == parcelEvaluation.Key)))
                    {
                        this.Context.ParcelEvaluations.Remove(parcelEvaluation);
                    }
                }
                foreach (var parcelFiscals in originalParcel.Fiscals)
                {
                    // Delete the fiscals from the parcel that have been removed.
                    if (!parcel.Fiscals.Any(e => (e.ParcelId == parcelFiscals.ParcelId && e.FiscalYear == parcelFiscals.FiscalYear && e.Key == parcelFiscals.Key)))
                    {
                        this.Context.ParcelFiscals.Remove(parcelFiscals);
                    }
                }
            }

            return originalParcel;
        }

        /// <summary>
        /// Remove the specified parcel from the datasource.
        /// </summary>
        /// <param name="parcel"></param>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <exception type="NotAuthorizedException">User does not have required claim to delete property.</exception>
        /// <returns></returns>
        public void Remove(Parcel parcel)
        {
            var userAgencies = this.User.GetAgenciesAsNullable();
            var viewSensitive = this.User.HasPermission(Permissions.SensitiveView);
            var isAdmin = this.User.HasPermission(Permissions.AdminProperties);
            var originalParcel = this.Context.Parcels
                .Include(p => p.Agency)
                .Include(p => p.Address)
                .Include(p => p.Evaluations)
                .Include(p => p.Fiscals)
                .Include(p => p.BuildingsManyToMany).ThenInclude(b => b.Building).ThenInclude(b => b.Evaluations)
                .Include(p => p.BuildingsManyToMany).ThenInclude(b => b.Building).ThenInclude(b => b.Fiscals)
                .Include(p => p.BuildingsManyToMany).ThenInclude(b => b.Building).ThenInclude(b => b.Address)
                .Include(p => p.SubdivisionsManyToMany).ThenInclude(b => b.Subdivision)
                .Include(p => p.ParcelsManyToMany).ThenInclude(b => b.Parcel)
                .SingleOrDefault(u => u.Id == parcel.Id) ?? throw new KeyNotFoundException();

            // Subdivisions can be deleted if the user has the edit claim.
            if (originalParcel.PropertyTypeId == (long)PropertyTypes.Land)
                parcel.ThrowIfNotAllowedToEdit(nameof(parcel), this.User, new[] { Permissions.PropertyDelete, Permissions.AdminProperties });
            else if (originalParcel.PropertyTypeId == (long)PropertyTypes.Subdivision)
                parcel.ThrowIfNotAllowedToEdit(nameof(parcel), this.User, new[] { Permissions.PropertyEdit, Permissions.AdminProperties });

            if (!isAdmin && (!userAgencies.Contains(originalParcel.AgencyId) || originalParcel.IsSensitive && !viewSensitive))
                throw new NotAuthorizedException("User does not have permission to delete.");

            originalParcel.ThrowIfPropertyInSppProject(this.User);

            this.ThrowIfNotAllowedToUpdate(originalParcel, _options.Project);
            this.Context.Entry(originalParcel).CurrentValues.SetValues(parcel);
            this.Context.SetOriginalRowVersion(originalParcel);

            originalParcel.BuildingsManyToMany.ForEach(b =>
            {
                this.ThrowIfNotAllowedToUpdate(b.Building, _options.Project);
                this.Context.ParcelBuildings.Remove(b);
                this.Context.BuildingEvaluations.RemoveRange(b.Building.Evaluations);
                this.Context.BuildingFiscals.RemoveRange(b.Building.Fiscals);
            });
            this.Context.ParcelEvaluations.RemoveRange(originalParcel.Evaluations);
            this.Context.ParcelFiscals.RemoveRange(originalParcel.Fiscals);
            this.Context.ParcelParcels.RemoveRange(originalParcel.ParcelsManyToMany);
            this.Context.ParcelParcels.RemoveRange(originalParcel.SubdivisionsManyToMany);
            this.Context.Parcels.Remove(originalParcel); // TODO: Shouldn't be allowed to permanently delete parcels entirely under certain conditions.
            if (parcel.PropertyTypeId == (long)PropertyTypes.Land)
            {
                var subdivisions = originalParcel.SubdivisionsManyToMany.Select(s => s.Subdivision);
                this.Context.Parcels.RemoveRange(subdivisions);
            }

            this.Context.CommitTransaction();
        }

        public bool IsPidAvailable(long parcelId, int PID)
        {
            return !Context.Parcels.Any(parcel => parcel.PID == PID && parcel.Id != parcelId);
        }

        public bool IsPinAvailable(long parcelId, int PIN)
        {
            return !Context.Parcels.Any(parcel => parcel.PIN == PIN && parcel.Id != parcelId);
        }

        #endregion
    }
}
