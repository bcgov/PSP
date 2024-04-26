using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.CodeTypes;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Helpers;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Api.Services
{
    public class LeaseService : BaseService, ILeaseService
    {
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly ILeaseRepository _leaseRepository;
        private readonly IPropertyImprovementRepository _propertyImprovementRepository;
        private readonly ICoordinateTransformService _coordinateService;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyLeaseRepository _propertyLeaseRepository;
        private readonly IEntityNoteRepository _entityNoteRepository;
        private readonly IInsuranceRepository _insuranceRepository;
        private readonly ILeaseTenantRepository _tenantRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPropertyService _propertyService;

        public LeaseService(
            ClaimsPrincipal user,
            ILogger<LeaseService> logger,
            ILeaseRepository leaseRepository,
            ICoordinateTransformService coordinateTransformService,
            IPropertyRepository propertyRepository,
            IPropertyLeaseRepository propertyLeaseRepository,
            IPropertyImprovementRepository propertyImprovementRepository,
            IEntityNoteRepository entityNoteRepository,
            IInsuranceRepository insuranceRepository,
            ILeaseTenantRepository tenantRepository,
            IUserRepository userRepository,
            IPropertyService propertyService)
            : base(user, logger)
        {
            _logger = logger;
            _user = user;
            _leaseRepository = leaseRepository;
            _coordinateService = coordinateTransformService;
            _propertyRepository = propertyRepository;
            _propertyLeaseRepository = propertyLeaseRepository;
            _entityNoteRepository = entityNoteRepository;
            _propertyImprovementRepository = propertyImprovementRepository;
            _insuranceRepository = insuranceRepository;
            _tenantRepository = tenantRepository;
            _userRepository = userRepository;
            _propertyService = propertyService;
        }

        public bool IsRowVersionEqual(long leaseId, long rowVersion)
        {
            long currentRowVersion = _leaseRepository.GetRowVersion(leaseId);
            return currentRowVersion == rowVersion;
        }

        public PimsLease GetById(long leaseId)
        {
            _logger.LogInformation("Getting lease {leaseId}", leaseId);
            _user.ThrowIfNotAuthorized(Permissions.LeaseView);
            var pimsUser = _userRepository.GetByKeycloakUserId(_user.GetUserKey());
            pimsUser.ThrowInvalidAccessToLeaseFile(_leaseRepository.GetNoTracking(leaseId).RegionCode);

            var lease = _leaseRepository.Get(leaseId);
            foreach (PimsPropertyLease propertyLease in lease.PimsPropertyLeases)
            {
                var property = propertyLease.Property;
                if (property?.Location != null)
                {
                    var newCoords = _coordinateService.TransformCoordinates(SpatialReference.BCALBERS, SpatialReference.WGS84, property.Location.Coordinate);
                    property.Location = GeometryHelper.CreatePoint(newCoords, SpatialReference.WGS84);
                }
            }
            return lease;
        }

        public LastUpdatedByModel GetLastUpdateInformation(long leaseId)
        {
            _logger.LogInformation("Retrieving last updated-by information...");
            _user.ThrowIfNotAuthorized(Permissions.LeaseView);

            return _leaseRepository.GetLastUpdateBy(leaseId);
        }

        public Paged<PimsLease> GetPage(LeaseFilter filter, bool? all = false)
        {
            _logger.LogInformation("Getting lease page {filter}", filter);
            _user.ThrowIfNotAuthorized(Permissions.LeaseView);
            filter.Quantity = all.HasValue && all.Value ? _leaseRepository.Count() : filter.Quantity;
            var user = _userRepository.GetByKeycloakUserId(this.User.GetUserKey());

            var leases = _leaseRepository.GetPage(filter, user.PimsRegionUsers.Select(u => u.RegionCode).ToHashSet());
            return leases;
        }

        public IEnumerable<PimsInsurance> GetInsuranceByLeaseId(long leaseId)
        {
            _logger.LogInformation("Getting insurance on lease {leaseId}", leaseId);
            _user.ThrowIfNotAuthorized(Permissions.LeaseView);
            var pimsUser = _userRepository.GetByKeycloakUserId(_user.GetUserKey());
            pimsUser.ThrowInvalidAccessToLeaseFile(_leaseRepository.GetNoTracking(leaseId).RegionCode);

            return _insuranceRepository.GetByLeaseId(leaseId);
        }

        public IEnumerable<PimsInsurance> UpdateInsuranceByLeaseId(long leaseId, IEnumerable<PimsInsurance> pimsInsurances)
        {
            _logger.LogInformation("Updating insurance on lease {leaseId}", leaseId);
            _user.ThrowIfNotAuthorized(Permissions.LeaseEdit);
            var pimsUser = _userRepository.GetByKeycloakUserId(_user.GetUserKey());
            pimsUser.ThrowInvalidAccessToLeaseFile(_leaseRepository.GetNoTracking(leaseId).RegionCode);

            _insuranceRepository.UpdateLeaseInsurance(leaseId, pimsInsurances);
            _insuranceRepository.CommitTransaction();

            return _insuranceRepository.GetByLeaseId(leaseId);
        }

        public IEnumerable<PimsPropertyImprovement> GetImprovementsByLeaseId(long leaseId)
        {
            _logger.LogInformation("Getting property improvements on lease {leaseId}", leaseId);
            _user.ThrowIfNotAuthorized(Permissions.LeaseView);
            var pimsUser = _userRepository.GetByKeycloakUserId(_user.GetUserKey());
            pimsUser.ThrowInvalidAccessToLeaseFile(_leaseRepository.GetNoTracking(leaseId).RegionCode);

            return _propertyImprovementRepository.GetByLeaseId(leaseId);
        }

        public IEnumerable<PimsPropertyImprovement> UpdateImprovementsByLeaseId(long leaseId, IEnumerable<PimsPropertyImprovement> pimsPropertyImprovements)
        {
            _logger.LogInformation("Updating property improvements on lease {leaseId}", leaseId);
            _user.ThrowIfNotAuthorized(Permissions.LeaseEdit);
            var pimsUser = _userRepository.GetByKeycloakUserId(_user.GetUserKey());
            pimsUser.ThrowInvalidAccessToLeaseFile(_leaseRepository.GetNoTracking(leaseId).RegionCode);

            _propertyImprovementRepository.Update(leaseId, pimsPropertyImprovements);
            _propertyImprovementRepository.CommitTransaction();

            return _propertyImprovementRepository.GetByLeaseId(leaseId);
        }

        public IEnumerable<PimsLeaseTenant> GetTenantsByLeaseId(long leaseId)
        {
            _logger.LogInformation("Getting tenants on lease {leaseId}", leaseId);
            _user.ThrowIfNotAuthorized(Permissions.LeaseView);
            var pimsUser = _userRepository.GetByKeycloakUserId(_user.GetUserKey());
            pimsUser.ThrowInvalidAccessToLeaseFile(_leaseRepository.GetNoTracking(leaseId).RegionCode);

            return _tenantRepository.GetByLeaseId(leaseId);
        }

        public IEnumerable<PimsLeaseTenant> UpdateTenantsByLeaseId(long leaseId, IEnumerable<PimsLeaseTenant> pimsLeaseTenants)
        {
            _logger.LogInformation("Updating tenants on lease {leaseId}", leaseId);
            _user.ThrowIfNotAuthorized(Permissions.LeaseEdit);
            var pimsUser = _userRepository.GetByKeycloakUserId(_user.GetUserKey());
            pimsUser.ThrowInvalidAccessToLeaseFile(_leaseRepository.GetNoTracking(leaseId).RegionCode);

            _tenantRepository.Update(leaseId, pimsLeaseTenants);
            _tenantRepository.CommitTransaction();

            return _tenantRepository.GetByLeaseId(leaseId);
        }

        public PimsLease Add(PimsLease lease, IEnumerable<UserOverrideCode> userOverrides)
        {
            _logger.LogInformation("Adding lease");
            _user.ThrowIfNotAuthorized(Permissions.LeaseAdd);
            var pimsUser = _userRepository.GetByKeycloakUserId(_user.GetUserKey());
            pimsUser.ThrowInvalidAccessToLeaseFile(lease.RegionCode);

            var leasesWithProperties = AssociatePropertyLeases(lease, userOverrides);

            return _leaseRepository.Add(leasesWithProperties);
        }

        public IEnumerable<PimsPropertyLease> GetPropertiesByLeaseId(long leaseId)
        {
            _logger.LogInformation("Getting properties on lease {leaseId}", leaseId);
            _user.ThrowIfNotAuthorized(Permissions.LeaseView);
            var pimsUser = _userRepository.GetByKeycloakUserId(_user.GetUserKey());
            pimsUser.ThrowInvalidAccessToLeaseFile(_leaseRepository.GetNoTracking(leaseId).RegionCode);

            var propertyLeases = _propertyLeaseRepository.GetAllByLeaseId(leaseId);
            return ReprojectPropertyLocationsToWgs84(propertyLeases);
        }

        public PimsLease Update(PimsLease lease, IEnumerable<UserOverrideCode> userOverrides)
        {
            _logger.LogInformation("Updating lease {leaseId}", lease.LeaseId);
            _user.ThrowIfNotAuthorized(Permissions.LeaseEdit);

            var pimsUser = _userRepository.GetByKeycloakUserId(_user.GetUserKey());
            var currentLease = _leaseRepository.GetNoTracking(lease.LeaseId);

            pimsUser.ThrowInvalidAccessToLeaseFile(currentLease.RegionCode); // need to check that the user is able to access the current lease as well as has the region for the updated lease.
            pimsUser.ThrowInvalidAccessToLeaseFile(lease.RegionCode);

            var currentProperties = _propertyLeaseRepository.GetAllByLeaseId(lease.LeaseId);
            var newPropertiesAdded = lease.PimsPropertyLeases.Where(x => !currentProperties.Any(y => y.Internal_Id == x.Internal_Id)).ToList();

            if (newPropertiesAdded.Any(x => x.Property.IsRetired.HasValue && x.Property.IsRetired.Value))
            {
                throw new BusinessRuleViolationException("Retired property can not be selected.");
            }

            if (currentLease.LeaseStatusTypeCode != lease.LeaseStatusTypeCode)
            {
                _entityNoteRepository.Add(
                    new PimsLeaseNote()
                    {
                        LeaseId = currentLease.LeaseId,
                        AppCreateTimestamp = System.DateTime.Now,
                        AppCreateUserid = this.User.GetUsername(),
                        Note = new PimsNote()
                        {
                            IsSystemGenerated = true,
                            NoteTxt = $"Lease status changed from {currentLease.LeaseStatusTypeCode} to {lease.LeaseStatusTypeCode}",
                            AppCreateTimestamp = System.DateTime.Now,
                            AppCreateUserid = this.User.GetUsername(),
                        },
                    });
            }

            _leaseRepository.Update(lease, false);
            var leaseWithProperties = AssociatePropertyLeases(lease, userOverrides);
            _propertyLeaseRepository.UpdatePropertyLeases(lease.Internal_Id, leaseWithProperties.PimsPropertyLeases);

            _leaseRepository.UpdateLeaseConsultations(lease.Internal_Id, lease.ConcurrencyControlNumber, lease.PimsLeaseConsultations);

            List<PimsPropertyLease> differenceSet = currentProperties.Where(x => !lease.PimsPropertyLeases.Any(y => y.Internal_Id == x.Internal_Id)).ToList();
            foreach (var deletedProperty in differenceSet)
            {
                var totalAssociationCount = _propertyRepository.GetAllAssociationsCountById(deletedProperty.PropertyId);
                if (totalAssociationCount <= 1)
                {
                    _leaseRepository.CommitTransaction(); // TODO: this can only be removed if cascade deletes are implemented. EF executes deletes in alphabetic order.
                    _propertyRepository.Delete(deletedProperty.Property);
                }
            }

            _leaseRepository.CommitTransaction();
            return _leaseRepository.GetNoTracking(lease.LeaseId);
        }

        private IEnumerable<PimsPropertyLease> ReprojectPropertyLocationsToWgs84(IEnumerable<PimsPropertyLease> propertyLeases)
        {
            List<PimsPropertyLease> reprojectedProperties = new List<PimsPropertyLease>();
            foreach (var leaseProperty in propertyLeases)
            {
                if (leaseProperty.Property.Location != null)
                {
                    var oldCoords = leaseProperty.Property.Location.Coordinate;
                    var newCoords = _coordinateService.TransformCoordinates(SpatialReference.BCALBERS, SpatialReference.WGS84, oldCoords);
                    leaseProperty.Property.Location = GeometryHelper.CreatePoint(newCoords, SpatialReference.WGS84);
                    reprojectedProperties.Add(leaseProperty);
                }
            }
            return reprojectedProperties;
        }

        /// <summary>
        /// Attempt to associate property leases with real properties in the system using the pid/pin identifiers.
        /// Do not attempt to update any preexisiting properties, simply refer to them by id.
        ///
        /// By default, do not allow a property with existing leases to be associated unless the userOverride flag is true.
        /// </summary>
        /// <param name="lease"></param>
        /// <param name="userOverrides"></param>
        /// <returns></returns>
        private PimsLease AssociatePropertyLeases(PimsLease lease, IEnumerable<UserOverrideCode> userOverrides)
        {
            MatchProperties(lease, userOverrides);

            foreach (var propertyLease in lease.PimsPropertyLeases)
            {
                PimsProperty property = propertyLease.Property;
                var existingPropertyLeases = _propertyLeaseRepository.GetAllByPropertyId(property.PropertyId);
                var isPropertyOnOtherLease = existingPropertyLeases.Any(p => p.LeaseId != lease.Internal_Id);
                var isPropertyOnThisLease = existingPropertyLeases.Any(p => p.LeaseId == lease.Internal_Id);

                if (isPropertyOnOtherLease && !isPropertyOnThisLease && !userOverrides.Contains(UserOverrideCode.AddPropertyToInventory))
                {
                    var genericOverrideErrorMsg = $"is attached to L-File # {existingPropertyLeases.FirstOrDefault().Lease.LFileNo}";
                    if (propertyLease?.Property?.Pin != null)
                    {
                        throw new UserOverrideException(UserOverrideCode.AddPropertyToInventory, $"PIN {propertyLease?.Property?.Pin.ToString() ?? string.Empty} {genericOverrideErrorMsg}");
                    }
                    if (propertyLease?.Property?.Pid != null)
                    {
                        throw new UserOverrideException(UserOverrideCode.AddPropertyToInventory, $"PID {propertyLease?.Property?.Pid.ToString() ?? string.Empty} {genericOverrideErrorMsg}");
                    }
                    string overrideError = $"Lng/Lat {propertyLease?.Property?.Location.Coordinate.X.ToString(CultureInfo.CurrentCulture) ?? string.Empty}, " +
                        $"{propertyLease?.Property?.Location.Coordinate.Y.ToString(CultureInfo.CurrentCulture) ?? string.Empty} {genericOverrideErrorMsg}";

                    throw new UserOverrideException(UserOverrideCode.AddPropertyToInventory, overrideError);
                }

                // If the property exist dont update it, just refer to it by id.
                if (property.Internal_Id != 0)
                {
                    propertyLease.PropertyId = property.PropertyId;
                    propertyLease.Property = null;
                }
            }

            return lease;
        }

        private void UpdateLocation(PimsProperty leaseProperty, ref PimsProperty propertyToUpdate, IEnumerable<UserOverrideCode> userOverrides)
        {
            if (propertyToUpdate.Location == null)
            {
                if (userOverrides.Contains(UserOverrideCode.AddLocationToProperty))
                {
                    // convert spatial location from lat/long (4326) to BC Albers (3005) for database storage
                    var geom = leaseProperty.Location;
                    if (geom.SRID != SpatialReference.BCALBERS)
                    {
                        var newCoords = _coordinateService.TransformCoordinates(geom.SRID, SpatialReference.BCALBERS, geom.Coordinate);
                        propertyToUpdate.Location = GeometryHelper.CreatePoint(newCoords, SpatialReference.BCALBERS);
                        _propertyRepository.Update(propertyToUpdate, overrideLocation: true);
                    }

                    var boundaryGeom = leaseProperty.Boundary;
                    if (boundaryGeom != null && boundaryGeom.SRID != SpatialReference.BCALBERS)
                    {
                        var newCoords = boundaryGeom.Coordinates.Select(coord => _coordinateService.TransformCoordinates(boundaryGeom.SRID, SpatialReference.BCALBERS, coord));
                        var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(SpatialReference.BCALBERS);
                        leaseProperty.Boundary = gf.CreatePolygon(newCoords.ToArray());
                    }
                }
                else
                {
                    throw new UserOverrideException(UserOverrideCode.AddLocationToProperty, "The selected property already exists in the system's inventory. However, the record is missing spatial details.\n\n To add the property, the spatial details for this property will need to be updated. The system will attempt to update the property record with spatial information from the current selection.");
                }
            }
        }

        private void MatchProperties(PimsLease lease, IEnumerable<UserOverrideCode> userOverrides)
        {
            foreach (var leaseProperty in lease.PimsPropertyLeases)
            {
                if (leaseProperty.Property.Pid.HasValue)
                {
                    var pid = leaseProperty.Property.Pid.Value;
                    try
                    {
                        var foundProperty = _propertyRepository.GetByPid(pid, true);
                        if (foundProperty.IsRetired.HasValue && foundProperty.IsRetired.Value)
                        {
                            throw new BusinessRuleViolationException("Retired property can not be selected.");
                        }

                        leaseProperty.PropertyId = foundProperty.Internal_Id;
                        UpdateLocation(leaseProperty.Property, ref foundProperty, userOverrides);
                        leaseProperty.Property = foundProperty;
                    }
                    catch (KeyNotFoundException)
                    {
                        _logger.LogDebug("Adding new property with pid:{pid}", pid);
                        _propertyService.PopulateNewProperty(leaseProperty.Property, true, false);
                    }
                }
                else if (leaseProperty.Property.Pin.HasValue)
                {
                    var pin = leaseProperty.Property.Pin.Value;
                    try
                    {
                        var foundProperty = _propertyRepository.GetByPin(pin, true);
                        if (foundProperty.IsRetired.HasValue && foundProperty.IsRetired.Value)
                        {
                            throw new BusinessRuleViolationException("Retired property can not be selected.");
                        }

                        leaseProperty.PropertyId = foundProperty.Internal_Id;
                        UpdateLocation(leaseProperty.Property, ref foundProperty, userOverrides);
                        leaseProperty.Property = foundProperty;
                    }
                    catch (KeyNotFoundException)
                    {
                        _logger.LogDebug("Adding new property with pin:{pin}", pin);
                        _propertyService.PopulateNewProperty(leaseProperty.Property, true, false);
                    }
                }
                else
                {
                    _logger.LogDebug("Adding new property without a pid or pin");
                    _propertyService.PopulateNewProperty(leaseProperty.Property, true, false);
                }
            }
        }
    }
}
