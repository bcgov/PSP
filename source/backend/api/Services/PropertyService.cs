using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using Pims.Api.Helpers.Exceptions;
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

        public PropertyService(ClaimsPrincipal user, ILogger<PropertyService> logger, IPropertyRepository propertyRepository, IPropertyContactRepository propertyContactRepository, ICoordinateTransformService coordinateService)
        {
            _user = user;
            _logger = logger;
            _propertyRepository = propertyRepository;
            _propertyContactRepository = propertyContactRepository;
            _coordinateService = coordinateService;
        }

        public PimsProperty GetById(long id)
        {
            _logger.LogInformation("Getting property with id {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.PropertyView);

            var property = _propertyRepository.GetById(id);
            if (property?.Location != null)
            {
                property.Location = TransformCoordiates(property.Location);
            }
            return property;
        }

        public List<PimsProperty> GetMultipleById(List<long> ids)
        {
            _logger.LogInformation("Getting multiple properies by id");
            _user.ThrowIfNotAuthorized(Permissions.PropertyView);

            List<PimsProperty> properties = _propertyRepository.GetAllByIds(ids);
            foreach (PimsProperty property in properties)
            {
                if (property?.Location != null)
                {
                    property.Location = TransformCoordiates(property.Location);
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

        private Point TransformCoordiates(Geometry location)
        {
            // return property spatial location in lat/long (4326)
            var newCoords = _coordinateService.TransformCoordinates(SpatialReference.BCALBERS, SpatialReference.WGS84, location.Coordinate);
            return GeometryHelper.CreatePoint(newCoords, SpatialReference.WGS84);
        }
    }
}
