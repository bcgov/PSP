using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using LinqKit;
using Microsoft.Extensions.Logging;
using Pims.Core.Exceptions;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

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

            CommonPropertyOperationValidation(operations);
            if (dbSourceProperty.IsRetired == true)
            {
                throw new BusinessRuleViolationException("Retired properties cannot be subdivided.");
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
                operation.SourceProperty = null; // do not allow the property operation to modify the source in the add range operation.
            }
            var completedOperations = _repository.AddRange(operations);
            _repository.CommitTransaction();

            return completedOperations;
        }

        public IEnumerable<PimsPropertyOperation> ConsolidateProperty(IEnumerable<PimsPropertyOperation> operations)
        {
            var destinationProperty = operations.FirstOrDefault()?.DestinationProperty;
            _user.ThrowIfNotAuthorized(Permissions.PropertyEdit);

            // validate
            IEnumerable<PimsProperty> sourceProperties = operations.Select(p => p.SourceProperty);
            IEnumerable<PimsProperty> dbSourceProperties = _propertyService.GetMultipleById(sourceProperties.Select(sp => sp.PropertyId).ToList());

            CommonPropertyOperationValidation(operations);
            if(destinationProperty?.Pid == null)
            {
                throw new BusinessRuleViolationException("Consolidation child must have a property with a valid PID.");
            }

            if (dbSourceProperties.Any(sp => sp.IsRetired == true))
            {
                throw new BusinessRuleViolationException("Retired properties cannot be consolidated.");
            }

            if (operations.Any(op => op.DestinationProperty.Pid != destinationProperty?.Pid))
            {
                throw new BusinessRuleViolationException("All property operations must have the same child property with the same PID.");
            }

            if (operations.Select(o => o.SourceProperty).GroupBy(s => s.PropertyId).Count() < 2)
            {
                throw new BusinessRuleViolationException("Consolidations must contain at least two parent properties.");
            }

            // either the property exists in pims, and is present in the source properties list, or the property does not have a match in PIMS at all (neither pid nor property_id).
            if (destinationProperty?.PropertyId > 0)
            {
                if (!dbSourceProperties.Any(sp => sp.PropertyId == destinationProperty?.PropertyId))
                {
                    throw new BusinessRuleViolationException("Consolidated child property may not be in the PIMS inventory unless also in the parent property list.");
                }
            }
            else
            {
                try
                {
                    _propertyService.GetByPid(destinationProperty?.Pid?.ToString());
                    throw new BusinessRuleViolationException("Consolidated child may not already be in the PIMS inventory.");
                }
                catch (KeyNotFoundException)
                {
                    // ignore exception, the pid should not exist.
                }
            }

            // retire the source properties
            foreach (var sp in dbSourceProperties)
            {
                sp.IsRetired = true;
                _propertyService.Update(sp, false);
            }

            destinationProperty.PropertyId = 0; // in the case this property already exists, this will force it to be recreated.
            var newProperty = _propertyService.PopulateNewProperty(destinationProperty, isOwned: true, isPropertyOfInterest: false);
            operations.ForEach(op => {
                op.DestinationProperty = newProperty;
                op.DestinationPropertyId = newProperty.PropertyId;
                op.SourceProperty = null; // do not allow the property operation to modify the source in the add range operation.
            });

            var completedOperations = _repository.AddRange(operations);
            _repository.CommitTransaction();

            return completedOperations;
        }

        private static void CommonPropertyOperationValidation(IEnumerable<PimsPropertyOperation> operations)
        {
            if (operations.Any(op => op.PropertyOperationNo != operations.FirstOrDefault().PropertyOperationNo))
            {
                throw new BusinessRuleViolationException("All property operations must have matching operation numbers.");
            }

            if (operations.Any(op => op.PropertyOperationTypeCode != operations.FirstOrDefault().PropertyOperationTypeCode))
            {
                throw new BusinessRuleViolationException("All property operations must have matching type codes.");
            }
        }
    }
}
