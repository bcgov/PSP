using System.Collections.Generic;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Api.Services
{
    public class PropertyOperationService : BaseService, IPropertyOperationService
    {
        private readonly IPropertyOperationRepository _propertyOperationRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Creates a new instance of a PropertyOperationService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        /// <param name="propertyOperationRepository"></param>
        public PropertyOperationService(ClaimsPrincipal user, ILogger<PropertyOperationService> logger, IPropertyOperationRepository propertyOperationRepository)
            : base(user, logger)
        {
            _propertyOperationRepository = propertyOperationRepository;
        }

        public IList<PimsPropertyOperation> GetOperationsForProperty(long propertyId)
        {
            this.Logger.LogInformation("Getting operations for property with id {propertyId}", propertyId);
            this.User.ThrowIfNotAuthorized(Permissions.PropertyView);

            var propertyOperations = _propertyOperationRepository.GetByPropertyId(propertyId);
            return propertyOperations;
        }
    }
}
