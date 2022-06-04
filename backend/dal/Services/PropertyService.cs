using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Dal.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly IPropertyRepository _propertyRepository;
        private readonly ICoordinateTransformService _coordinateService;

        public PropertyService(ClaimsPrincipal user, ILogger<PropertyService> logger, IPropertyRepository propertyRepository, ICoordinateTransformService coordinateService)
        {
            _user = user;
            _logger = logger;
            _propertyRepository = propertyRepository;
            _coordinateService = coordinateService;
        }

        public PimsProperty GetById(long id)
        {
            _logger.LogInformation("Getting property with id {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.PropertyView);
            // return property spatial location in lat/long (4326)
            var property = _propertyRepository.Get(id);
            property.Location = _coordinateService.TransformCoordinates(3005, 4326, property.Location);
            return property;
        }

        public PimsProperty GetByPid(string pid)
        {
            _logger.LogInformation("Getting property with pid {pid}", pid);
            _user.ThrowIfNotAuthorized(Permissions.PropertyView);
            // return property spatial location in lat/long (4326)
            var property = _propertyRepository.GetByPid(pid);
            property.Location = _coordinateService.TransformCoordinates(3005, 4326, property.Location);
            return property;
        }

        public PimsProperty Update(PimsProperty property)
        {
            _logger.LogInformation("Updating property...");
            _user.ThrowIfNotAuthorized(Permissions.PropertyEdit);

            // convert spatial location from lat/long (4326) to BC Albers (3005) for database storage
            property.Location = _coordinateService.TransformCoordinates(4326, 3005, property.Location);

            var newProperty = _propertyRepository.Update(property);
            _propertyRepository.CommitTransaction();

            return GetById(newProperty.Id);
        }
    }
}
