using System;
using System.Linq;
using MoreLinq;
using Pims.Dal.Constants;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Helpers;
using Pims.Dal.Repositories;

namespace Pims.Api.Services
{
    public class LeaseService : ILeaseService
    {
        private readonly ILeaseRepository _leaseRepository;
        private readonly ICoordinateTransformService _coordinateService;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyLeaseRepository _propertyLeaseRepository;

        public LeaseService(ILeaseRepository leaseRepository, ICoordinateTransformService coordinateTransformService, IPropertyRepository propertyRepository, IPropertyLeaseRepository propertyLeaseRepository)
        {
            _leaseRepository = leaseRepository;
            _coordinateService = coordinateTransformService;
            _propertyRepository = propertyRepository;
            _propertyLeaseRepository = propertyLeaseRepository;
        }

        public bool IsRowVersionEqual(long leaseId, long rowVersion)
        {
            long currentRowVersion = _leaseRepository.GetRowVersion(leaseId);
            return currentRowVersion == rowVersion;
        }

        public PimsLease GetById(long leaseId)
        {
            var lease = _leaseRepository.Get(leaseId);
            foreach (PimsPropertyLease propertyLease in lease.PimsPropertyLeases)
            {
                var property = propertyLease.Property;
                if (property?.Location != null)
                {
                    var newCoords = _coordinateService.TransformCoordinates(SpatialReference.BC_ALBERS, SpatialReference.WGS_84, property.Location.Coordinate);
                    property.Location = GeometryHelper.CreatePoint(newCoords, SpatialReference.WGS_84);
                }
            }
            return lease;
        }

        public PimsLease Add(PimsLease lease, bool userOverride = false)
        {
            var leasesWithProperties = AssociatePropertyLeases(lease, userOverride);
            return _leaseRepository.Add(leasesWithProperties, userOverride);
        }

        public PimsLease Update(PimsLease lease, bool userOverride = false)
        {
            _leaseRepository.Update(lease, false);
            var leaseWithProperties = AssociatePropertyLeases(lease, userOverride);
            var updatedLease = _leaseRepository.UpdatePropertyLeases(lease.Id, lease.ConcurrencyControlNumber, leaseWithProperties.PimsPropertyLeases, userOverride);
            _leaseRepository.CommitTransaction();
            return updatedLease;
        }

        /// <summary>
        /// Attempt to associate property leases with real properties in the system using the pid/pin identifiers.
        /// Do not attempt to update any preexisiting properties, simply refer to them by id.
        ///
        /// By default, do not allow a property with existing leases to be associated unless the userOverride flag is true.
        /// </summary>
        /// <param name="lease"></param>
        /// <param name="userOverride"></param>
        /// <param name="newLeaseProperties"></param>
        /// <returns></returns>
        private PimsLease AssociatePropertyLeases(PimsLease lease, bool userOverride = false)
        {
            lease.PimsPropertyLeases.ForEach(propertyLease =>
            {
                PimsProperty property = null;
                if (propertyLease.Property.Pid.HasValue && propertyLease.Property.Pid > 0)
                {
                    property = _propertyRepository.GetByPid(propertyLease.Property.Pid.Value);
                }
                else if (propertyLease.Property.Pin.HasValue && propertyLease.Property.Pin > 0)
                {
                    property = _propertyRepository.GetByPin(propertyLease.Property.Pin.Value);
                }
                else if (propertyLease?.Property?.Location != null && propertyLease?.Property?.Location.SRID != SpatialReference.BC_ALBERS)
                {
                    var coords = _coordinateService.TransformCoordinates(propertyLease.Property.Location.SRID, SpatialReference.BC_ALBERS, propertyLease.Property.Location.Coordinate);
                    var geom = GeometryHelper.CreatePoint(coords.X, coords.Y, SpatialReference.BC_ALBERS);
                    property = _propertyRepository.GetByLocation(geom);
                    if(property == null)
                    {
                        throw new InvalidOperationException($"Unable to find property for given lat/lng");
                    }
                }
                if (property?.PropertyId == null)
                {
                    if (propertyLease?.Property?.Pid != -1)
                    {
                        throw new InvalidOperationException($"Property with PID {propertyLease?.Property?.Pid.ToString() ?? string.Empty} does not exist");
                    }
                    else
                    {
                        throw new InvalidOperationException($"Property with PIN {propertyLease?.Property?.Pin.ToString() ?? string.Empty} does not exist");
                    }
                }
                var existingPropertyLeases = _propertyLeaseRepository.GetAllByPropertyId(property.PropertyId);
                var isPropertyOnOtherLease = existingPropertyLeases.Any(p => p.LeaseId != lease.Id);
                var isPropertyOnThisLease = existingPropertyLeases.Any(p => p.LeaseId == lease.Id);
                if (isPropertyOnOtherLease && !isPropertyOnThisLease && !userOverride)
                {
                    var genericOverrideErrorMsg = $"is attached to L-File # {existingPropertyLeases.FirstOrDefault().Lease.LFileNo}";
                    if (propertyLease?.Property?.Pin != null)
                    {
                        throw new UserOverrideException($"PIN {propertyLease?.Property?.Pin.ToString() ?? string.Empty} {genericOverrideErrorMsg}");
                    }
                    if (propertyLease?.Property?.Pid != null)
                    {
                        throw new UserOverrideException($"PID {propertyLease?.Property?.Pid.ToString() ?? string.Empty} {genericOverrideErrorMsg}");
                    }
                    throw new UserOverrideException($"Lng/Lat {propertyLease?.Property?.Location.Coordinate.X.ToString() ?? string.Empty}, " +
                        $"{propertyLease?.Property?.Location.Coordinate.Y.ToString() ?? string.Empty} {genericOverrideErrorMsg}");
                }
                propertyLease.PropertyId = property.PropertyId;
                propertyLease.Property = null; // Do not attempt to update the associated property, just refer to it by id.
            });
            return lease;
        }
    }
}
