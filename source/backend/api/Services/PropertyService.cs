using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using Pims.Api.Constants;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models.Concepts;
using Pims.Core.Extensions;
using Pims.Dal.Constants;
using Pims.Dal.Entities;
using Pims.Dal.Helpers;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Api.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyContactRepository _propertyContactRepository;
        private readonly ICoordinateTransformService _coordinateService;
        private readonly IPropertyLeaseRepository _propertyLeaseRepository;
        private readonly IMapper _mapper;

        public PropertyService(ClaimsPrincipal user, ILogger<PropertyService> logger, IPropertyRepository propertyRepository, IPropertyContactRepository propertyContactRepository, ICoordinateTransformService coordinateService, IPropertyLeaseRepository propertyLeaseRepository, IMapper mapper)
        {
            _user = user;
            _logger = logger;
            _propertyRepository = propertyRepository;
            _propertyContactRepository = propertyContactRepository;
            _coordinateService = coordinateService;
            _propertyLeaseRepository = propertyLeaseRepository;
            _mapper = mapper;
        }

        public PimsProperty GetById(long id)
        {
            _logger.LogInformation("Getting property with id {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.PropertyView);

            var property = _propertyRepository.GetById(id);
            if (property?.Location != null)
            {
                property.Location = TransformCoordinates(property.Location);
            }
            return property;
        }

        public List<PimsProperty> GetMultipleById(List<long> ids)
        {
            _logger.LogInformation("Getting multiple properties by id");
            _user.ThrowIfNotAuthorized(Permissions.PropertyView);

            List<PimsProperty> properties = _propertyRepository.GetAllByIds(ids);
            foreach (PimsProperty property in properties)
            {
                if (property?.Location != null)
                {
                    property.Location = TransformCoordinates(property.Location);
                }
            }

            return properties;
        }

        public PimsProperty GetByPid(string pid)
        {
            _logger.LogInformation("Getting property with pid {pid}", pid);
            _user.ThrowIfNotAuthorized(Permissions.PropertyView);

            // return property spatial location in lat/long (4326)
            var property = _propertyRepository.GetByPid(pid);
            if (property?.Location != null)
            {
                var newCoords = _coordinateService.TransformCoordinates(SpatialReference.BCALBERS, SpatialReference.WGS84, property.Location.Coordinate);
                property.Location = GeometryHelper.CreatePoint(newCoords, SpatialReference.WGS84);
            }
            return property;
        }

        public PimsProperty Update(PimsProperty property)
        {
            _logger.LogInformation("Updating property...");
            _user.ThrowIfNotAuthorized(Permissions.PropertyEdit);

            // convert spatial location from lat/long (4326) to BC Albers (3005) for database storage
            var geom = property.Location;
            if (geom.SRID != SpatialReference.BCALBERS)
            {
                var newCoords = _coordinateService.TransformCoordinates(geom.SRID, SpatialReference.BCALBERS, geom.Coordinate);
                property.Location = GeometryHelper.CreatePoint(newCoords, SpatialReference.BCALBERS);
            }

            var newProperty = _propertyRepository.Update(property);
            _propertyRepository.CommitTransaction();

            return GetById(newProperty.Internal_Id);
        }

        public IList<PimsPropertyContact> GetContacts(long propertyId)
        {
            _logger.LogInformation("Retrieving property contacts...");
            _user.ThrowIfNotAuthorized(Permissions.PropertyEdit);

            return _propertyContactRepository.GetContactsByProperty(propertyId);
        }

        public PimsPropertyContact GetContact(long propertyId, long contactId)
        {
            _logger.LogInformation("Retrieving single property contact...");
            _user.ThrowIfNotAuthorized(Permissions.PropertyEdit);

            var propertyContact = _propertyContactRepository.GetContact(contactId);

            if (propertyContact.PropertyId != propertyId)
            {
                throw new BadRequestException("Contact with the given id does not match the property id");
            }

            return propertyContact;
        }

        public PimsPropertyContact CreateContact(PimsPropertyContact propertyContact)
        {
            _logger.LogInformation("Creating property contact...");
            _user.ThrowIfNotAuthorized(Permissions.PropertyEdit);

            var propertyContactResult = _propertyContactRepository.Create(propertyContact);
            _propertyContactRepository.CommitTransaction();

            return propertyContactResult;
        }

        public PimsPropertyContact UpdateContact(PimsPropertyContact propertyContact)
        {
            _logger.LogInformation("Updating property contact...");
            _user.ThrowIfNotAuthorized(Permissions.PropertyEdit);

            var propertyContactResult = _propertyContactRepository.Update(propertyContact);
            _propertyContactRepository.CommitTransaction();

            return propertyContactResult;
        }

        public bool DeleteContact(long propertyContactId)
        {
            _logger.LogInformation("Deleting property contact...");
            _user.ThrowIfNotAuthorized(Permissions.PropertyEdit);

            _propertyContactRepository.Delete(propertyContactId);

            _propertyContactRepository.CommitTransaction();

            return true;
        }

        public PropertyManagementModel GetPropertyManagement(long propertyId)
        {
            _logger.LogInformation("Getting property management information for property with id {propertyId}", propertyId);
            _user.ThrowIfNotAuthorized(Permissions.PropertyView);
            _user.ThrowIfNotAuthorized(Permissions.ManagementView);

            var property = GetById(propertyId);

            var propertyLeases = _propertyLeaseRepository.GetAllByPropertyId(propertyId);
            var activeLease = propertyLeases.FirstOrDefault(pl => pl.Lease.LeaseStatusTypeCode == "ACTIVE")?.Lease;
            var leaseExpiryDate = activeLease?.GetExpiryDate()?.FilterSqlMinDate() ?? null;

            var propertyManagement = _mapper.Map<PropertyManagementModel>(property);
            propertyManagement.IsLeaseActive = activeLease is not null;
            propertyManagement.IsLeaseExpired = leaseExpiryDate is not null && (leaseExpiryDate < DateTime.UtcNow);
            propertyManagement.LeaseExpiryDate = leaseExpiryDate;

            return propertyManagement;
        }

        public PropertyManagementModel UpdatePropertyManagement(PimsProperty property)
        {
            _logger.LogInformation("Updating property management information...");
            _user.ThrowIfNotAuthorized(Permissions.PropertyEdit);
            _user.ThrowIfNotAuthorized(Permissions.ManagementEdit);

            var newProperty = _propertyRepository.UpdatePropertyManagement(property);
            _propertyRepository.CommitTransaction();

            return GetPropertyManagement(newProperty.Internal_Id);
        }

        public IList<PimsPropPropActivity> GetManagementActivities(long propertyId)
        {
            _logger.LogInformation("Getting property management activities for property with id {propertyId}", propertyId);
            _user.ThrowIfNotAuthorized(Permissions.ManagementView, Permissions.PropertyView);

            return _propertyRepository.GetManagementActivitiesByProperty(propertyId);
        }

        public bool DeleteManagementPropPropActivity(long propertyId, long managementActivityId)
        {
            _logger.LogInformation("Deleting Management Activity with id {managementActivityId} from property with Id {propertyId}", managementActivityId, propertyId);
            _user.ThrowIfNotAuthorized(Permissions.ManagementDelete);

            var propertyManagementActivity = _propertyRepository.GetManagementActivityById(managementActivityId);
            if(propertyManagementActivity.PropertyId != propertyId)
            {
                throw new BadRequestException($"PropertyManagementActivity with Id: {managementActivityId} and PropertyId {propertyId} doesn't exist");
            }

            if (!propertyManagementActivity.PimsPropertyActivity.PropMgmtActivityStatusTypeCode.Equals(PropertyActivityStatusTypeCode.NOTSTARTED.ToString()))
            {
                throw new BadRequestException($"PropertyManagementActivity can not be deleted since it has already started");
            }

            var success = _propertyRepository.TryDeletePropPropActivity(managementActivityId);
            _propertyRepository.CommitTransaction();

            return success;
        }

        private Point TransformCoordinates(Geometry location)
        {
            // return property spatial location in lat/long (4326)
            var newCoords = _coordinateService.TransformCoordinates(SpatialReference.BCALBERS, SpatialReference.WGS84, location.Coordinate);
            return GeometryHelper.CreatePoint(newCoords, SpatialReference.WGS84);
        }
    }
}
