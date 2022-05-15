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

        public PropertyService(ClaimsPrincipal user, ILogger<PropertyService> logger, IPropertyRepository propertyRepository)
        {
            _user = user;
            _logger = logger;
            _propertyRepository = propertyRepository;
        }

        public PimsProperty GetByPid(string pid)
        {
            _logger.LogInformation("Getting property with pid {pid}", pid);
            _user.ThrowIfNotAuthorized(Permissions.PropertyView);
            return _propertyRepository.GetByPid(pid);
        }

        public PimsProperty Update(PimsProperty property)
        {
            _logger.LogInformation("Updating property...");
            _user.ThrowIfNotAuthorized(Permissions.PropertyEdit);

            var newProperty = _propertyRepository.Update(property);
            _propertyRepository.CommitTransaction();
            return newProperty;
        }
    }
}
