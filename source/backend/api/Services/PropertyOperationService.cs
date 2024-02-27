using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Pims.Core.Exceptions;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Pims.Dal.Helpers.Extensions;

namespace Pims.Api.Services
{
    public class PropertyOperationService : IPropertyOperationService
    {
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly IPropertyService _propertyService;
        private readonly IPropertyOperationRepository _repository;

        public PropertyOperationService(
            ClaimsPrincipal user,
            ILogger<PropertyService> logger,
            IPropertyService propertyService,
            IPropertyOperationRepository repository)
        {
            _user = user;
            _logger = logger;
            _propertyService = propertyService;
            _repository = repository;
        }

        public IList<PimsPropertyOperation> GetOperationsForProperty(long propertyId)
        {
            this._logger.LogInformation("Getting operations for property with id {propertyId}", propertyId);
            this._user.ThrowIfNotAuthorized(Permissions.PropertyView);

            var propertyOperations = _repository.GetByPropertyId(propertyId);
            return propertyOperations;
        }

        public IEnumerable<PimsPropertyOperation> SubdivideProperty(IEnumerable<PimsPropertyOperation> operations)
        {
            _logger.LogInformation("Subdividing property with id {id}", operations.FirstOrDefault()?.SourcePropertyId);
            _user.ThrowIfNotAuthorized(Permissions.PropertyEdit);

            // validate
            long sourcePropertyId = operations.FirstOrDefault().SourcePropertyId;
            PimsProperty dbSourceProperty = _propertyService.GetById(sourcePropertyId);
            if (dbSourceProperty.IsRetired == true)
            {
                throw new BusinessRuleViolationException("Retired properties cannot be subdivided.");
            }

            if (operations.Any(op => op.PropertyOperationNo != operations.FirstOrDefault().PropertyOperationNo))
            {
                 throw new BusinessRuleViolationException("All property operations must have matching operation numbers.");
            }

            if (operations.Any(op => op.PropertyOperationTypeCode != operations.FirstOrDefault().PropertyOperationTypeCode))
            {
                 throw new BusinessRuleViolationException("All property operations must have matching type codes.");
            }

            if (operations.Any(op => op.SourcePropertyId != operations.FirstOrDefault().SourcePropertyId))
            {
                 throw new BusinessRuleViolationException("All property operations must have the same PIMS parent property.");
            }

            if (operations.Select(o => o.DestinationProperty).Count() < 2)
            {
                 throw new BusinessRuleViolationException("Subdivisions must contain at least two child properties.");
            }

            foreach (var operation in operations)
            {
                if (dbSourceProperty.Pid == operation.DestinationProperty.Pid)
                {
                    continue; // the user is allowed to add a child property that exists in pims if it has the same pid as the destination property.
                }
                try
                {
                    _propertyService.GetByPid(operation.DestinationProperty.Pid.ToString());
                    throw new BusinessRuleViolationException("Subdivision children may not already be in the PIMS inventory.");

                } catch (KeyNotFoundException)
                {
                    // ignore exception, the pid should not exist.
                }
            }

            // retire the source property
            dbSourceProperty.IsRetired = true;
            _propertyService.Update(dbSourceProperty, false);

            foreach (var operation in operations)
            {
                operation.DestinationProperty.PropertyId = 0; // in the case this property already exists, this will force it to be recreated.
                var newProperty = _propertyService.PopulateNewProperty(operation.DestinationProperty, isOwned: true, isPropertyOfInterest: false);
                operation.DestinationProperty = newProperty;
                operation.DestinationPropertyId = newProperty.PropertyId;
            }
            var completedOperations = _repository.AddRange(operations);
            _repository.CommitTransaction();

            return completedOperations;
        }
    }
}