using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Concepts.Property;
using Pims.Api.Repositories.PropertyLookup;
using Pims.Core.Extensions;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;

namespace Pims.Api.Services
{
    /// <summary>
    /// Orchestrates property lookups across PIMS and PMBC.
    /// </summary>
    public class PropertyLookupService : IPropertyLookupService
    {
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyService _propertyService;
        private readonly IPropertyPmbcRepository _propertyPmbcRepository;
        private readonly IMapper _mapper;

        public PropertyLookupService(
            ClaimsPrincipal user,
            IPropertyRepository propertyRepository,
            IPropertyService propertyService,
            IPropertyPmbcRepository propertyPmbcRepository,
            IMapper mapper,
            ILogger<PropertyLookupService> logger)
        {
            _user = user;
            _propertyRepository = propertyRepository;
            _propertyService = propertyService;
            _propertyPmbcRepository = propertyPmbcRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PropertyPidLookupResultModel> LookupByPidAsync(string pid)
        {
            var result = new PropertyPidLookupResultModel();

            PimsProperty pimsProperty = null;
            _logger.LogInformation("Getting property lookup with id {pid} in PIMS", pid);
            _user.ThrowIfNotAuthorized(Permissions.PropertyView);
            try
            {
                pimsProperty = _propertyRepository.GetByPid(pid);
            }
            catch (KeyNotFoundException)
            {
                _logger.LogInformation("PID {Pid} not found in PIMS inventory", pid);
            }

            if (pimsProperty != null)
            {
                result.FoundInPims = true;
                pimsProperty = _propertyService.TransformPropertyToLatLong(pimsProperty);
                result.Property = _mapper.Map<PropertyModel>(pimsProperty);
                return result;
            }
            _logger.LogInformation("Getting property lookup with id {pid} in PMBC", pid);
            result.Property = await _propertyPmbcRepository.GetByPidAsync(pid);

            return result;
        }
    }
}
