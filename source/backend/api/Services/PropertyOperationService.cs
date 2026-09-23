using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Core.Api.Exceptions;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;

namespace Pims.Api.Services
{
    public class PropertyOperationService : IPropertyOperationService
    {
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly ILookupRepository _lookupRepository;
        private readonly IPropertyService _propertyService;
        private readonly IPropertyOperationRepository _repository;

        public PropertyOperationService(
            ClaimsPrincipal user,
            ILogger<PropertyService> logger,
            ILookupRepository lookupRepository,
            IPropertyService propertyService,
            IPropertyOperationRepository repository)
        {
            _user = user;
            _logger = logger;
            _lookupRepository = lookupRepository;
            _propertyService = propertyService;
            _repository = repository;
        }

        public IList<PimsPropertyOperation> GetOperationsForProperty(long propertyId)
        {
            _logger.LogInformation("Getting operations for property with id {PropertyId}", propertyId);
            _user.ThrowIfNotAuthorized(Permissions.PropertyView);

            return _repository.GetByPropertyId(propertyId);
        }

        public IEnumerable<PimsPropertyOperation> SubdivideProperty(IEnumerable<PimsPropertyOperation> operations)
        {
            var propertyOperations = operations?.ToList() ?? new List<PimsPropertyOperation>();
            _logger.LogInformation("Subdividing property with id {id}", propertyOperations.FirstOrDefault()?.SourcePropertyId);
            _user.ThrowIfNotAuthorized(Permissions.PropertyEdit);

            ValidateInput(propertyOperations);
            ValidateSourcePropertySubdivision(propertyOperations);

            var dbSourceProperty = ResolveOrCreateSourceProperty(propertyOperations.First(), "subdivision");
            NormalizeSubdivideSource(propertyOperations, dbSourceProperty);
            ValidateSubdivideState(propertyOperations, dbSourceProperty);
            ValidateChildren(propertyOperations, dbSourceProperty);
            RetireResolvedSourceProperty(dbSourceProperty);
            PrepareSubdivideDestinations(propertyOperations);

            var completedOperations = _repository.AddRange(propertyOperations);
            _repository.CommitTransaction();

            return completedOperations;
        }

        public IEnumerable<PimsPropertyOperation> ConsolidateProperty(IEnumerable<PimsPropertyOperation> operations)
        {
            var propertyOperations = operations?.ToList() ?? new List<PimsPropertyOperation>();
            _user.ThrowIfNotAuthorized(Permissions.PropertyEdit);

            ValidateConsolidateInput(propertyOperations);

            var destinationProperty = propertyOperations.First().DestinationProperty;
            var dbSourceProperties = ResolveAndNormalizeConsolidateSources(propertyOperations);

            ValidateConsolidateState(propertyOperations, destinationProperty, dbSourceProperties);
            ValidateConsolidateDestinationInventory(destinationProperty, dbSourceProperties);
            RetireConsolidateSources(dbSourceProperties);
            PrepareConsolidateDestination(propertyOperations, destinationProperty);

            var completedOperations = _repository.AddRange(propertyOperations);
            _repository.CommitTransaction();

            return completedOperations;
        }

        private static void ValidateInput(List<PimsPropertyOperation> propertyOperations)
        {
            if (propertyOperations.Count == 0)
            {
                throw new BadRequestException("No property operations were sent.");
            }
        }

        private static void ValidateSourcePropertySubdivision(List<PimsPropertyOperation> propertyOperations)
        {
            if (propertyOperations.Any(op => op.SourcePropertyId <= 0 && op.SourceProperty?.Pid == null))
            {
                throw new BadRequestException("A valid source property with PID is required.");
            }

            var incomingSourceKeys = propertyOperations
                .Select(op => op.SourcePropertyId > 0
                    ? $"id:{op.SourcePropertyId}"
                    : $"pid:{op.SourceProperty.Pid}")
                .Distinct()
                .ToList();

            if (incomingSourceKeys.Count != 1)
            {
                throw new BusinessRuleViolationException("All property operations must have the same PIMS source parcel.");
            }
        }

        private static void NormalizeSubdivideSource(List<PimsPropertyOperation> propertyOperations, PimsProperty dbSourceProperty)
        {
            propertyOperations.ForEach(op =>
            {
                if (dbSourceProperty.PropertyId > 0)
                {
                    op.SourcePropertyId = dbSourceProperty.PropertyId;
                }
                op.SourceProperty = dbSourceProperty;
            });
        }

        private static void CommonPropertyOperationValidation(IEnumerable<PimsPropertyOperation> operations, IEnumerable<PimsProperty> dbSourceProperties)
        {
            var operationList = operations?.ToList() ?? new List<PimsPropertyOperation>();
            if (operationList.Count == 0)
            {
                throw new BadRequestException("No property operations were sent.");
            }

            var firstOperation = operationList.First();

            foreach (var sourceProperty in dbSourceProperties)
            {
                var operation = operationList.FirstOrDefault(p => p.SourcePropertyId == sourceProperty.PropertyId);
                if (operation == null)
                {
                    throw new BadRequestException("All source properties must exist in the system.");
                }
                if (sourceProperty.IsOwned != operation.SourceProperty.IsOwned)
                {
                    throw new BusinessRuleViolationException("All source properties must match existing properties in the system.");
                }
            }

            if (operationList.Any(op => op.PropertyOperationNo != firstOperation.PropertyOperationNo))
            {
                throw new BusinessRuleViolationException("All property operations must have matching operation numbers.");
            }

            if (operationList.Any(op => op.PropertyOperationTypeCode != firstOperation.PropertyOperationTypeCode))
            {
                throw new BusinessRuleViolationException("All property operations must have matching type codes.");
            }
        }

        private static void ValidateSubdivideState(List<PimsPropertyOperation> propertyOperations, PimsProperty dbSourceProperty)
        {
            ValidateInput(propertyOperations);
            CommonPropertyOperationValidation(propertyOperations, new List<PimsProperty>() { dbSourceProperty });
            var firstSourcePropertyId = propertyOperations.First().SourcePropertyId;

            if (dbSourceProperty.IsRetired == true)
            {
                throw new BusinessRuleViolationException("Retired properties cannot be subdivided.");
            }

            if (propertyOperations.Any(op => op.SourcePropertyId != firstSourcePropertyId))
            {
                throw new BusinessRuleViolationException("All property operations must have the same PIMS source parcel.");
            }

            if (propertyOperations.Select(o => o.DestinationProperty).Count() < 2)
            {
                throw new BusinessRuleViolationException("Subdivisions must contain at least two child properties.");
            }
        }

        private static void ValidateConsolidateInput(List<PimsPropertyOperation> propertyOperations)
        {
            if (propertyOperations.Count == 0)
            {
                throw new BadRequestException("No property operations were sent.");
            }
        }

        private static void ValidateConsolidateState(List<PimsPropertyOperation> propertyOperations, PimsProperty destinationProperty, List<PimsProperty> dbSourceProperties)
        {
            CommonPropertyOperationValidation(propertyOperations, dbSourceProperties);

            if (destinationProperty?.Pid == null)
            {
                throw new BusinessRuleViolationException("Consolidation child must have a property with a valid PID.");
            }

            if (dbSourceProperties.Any(sp => sp.IsRetired == true))
            {
                throw new BusinessRuleViolationException("Retired properties cannot be consolidated.");
            }

            if (propertyOperations.Any(op => op.DestinationProperty.Pid != destinationProperty.Pid))
            {
                throw new BusinessRuleViolationException("All property operations must have the same consolidated parcel with the same PID.");
            }

            var distinctSourceCount = propertyOperations
                .Select(op => op.SourceProperty)
                .Where(sp => sp != null)
                .Select(sp => sp.PropertyId > 0 ? $"id:{sp.PropertyId}" : $"pid:{sp.Pid}")
                .Distinct()
                .Count();

            if (distinctSourceCount < 2)
            {
                throw new BusinessRuleViolationException("Consolidations must contain at least two source parcels.");
            }
        }

        private void ValidateChildren(List<PimsPropertyOperation> propertyOperations, PimsProperty dbSourceProperty)
        {
            foreach (var operation in propertyOperations)
            {
                if (dbSourceProperty.Pid == operation.DestinationProperty.Pid)
                {
                    continue; // the user is allowed to add a child property that exists in pims if it has the same pid as the destination property.
                }
                try
                {
                    _propertyService.GetByPid(operation.DestinationProperty.Pid.ToString());
                    throw new BusinessRuleViolationException("Subdivision children may not already be in the PIMS inventory.");
                }
                catch (KeyNotFoundException)
                {
                    // ignore exception, the pid should not exist.
                }
            }
        }

        private void PrepareSubdivideDestinations(List<PimsPropertyOperation> propertyOperations)
        {
            foreach (var operation in propertyOperations)
            {
                var newProperty = PrepareDestinationProperty(operation.DestinationProperty);
                operation.DestinationProperty = newProperty;
                operation.DestinationPropertyId = newProperty.PropertyId;

                // Keep the source navigation for newly-created source properties so EF can resolve SOURCE_PROPERTY_ID.
                if (operation.SourcePropertyId > 0)
                {
                    operation.SourceProperty = null; // Use FK-only for existing source properties so they are not modified in add-range.
                }
            }
        }

        private List<PimsProperty> ResolveAndNormalizeConsolidateSources(List<PimsPropertyOperation> propertyOperations)
        {
            var dbSourceProperties = propertyOperations
                .Select(op => ResolveOrCreateSourceProperty(op, "consolidation"))
                .ToList();

            for (var i = 0; i < propertyOperations.Count; i++)
            {
                if (dbSourceProperties[i].PropertyId > 0)
                {
                    propertyOperations[i].SourcePropertyId = dbSourceProperties[i].PropertyId;
                }
                propertyOperations[i].SourceProperty = dbSourceProperties[i];
            }

            return dbSourceProperties;
        }

        private void ValidateConsolidateDestinationInventory(PimsProperty destinationProperty, List<PimsProperty> dbSourceProperties)
        {
            try
            {
                var dbDestinationProperty = _propertyService.GetByPid(destinationProperty?.Pid?.ToString());

                // if the property exists in pims, it must also be present in the source properties list.
                if (!dbSourceProperties.Any(sp => sp.PropertyId == dbDestinationProperty?.PropertyId))
                {
                    throw new BusinessRuleViolationException("Consolidated parcel may not be in the PIMS inventory unless also in the source parcel list.");
                }
            }
            catch (KeyNotFoundException)
            {
                // ignore exception, the pid should not exist.
            }
        }

        private void RetireConsolidateSources(List<PimsProperty> dbSourceProperties)
        {
            foreach (var sp in dbSourceProperties)
            {
                RetireResolvedSourceProperty(sp);
            }
        }

        private void PrepareConsolidateDestination(List<PimsPropertyOperation> propertyOperations, PimsProperty destinationProperty)
        {
            var newProperty = PrepareDestinationProperty(destinationProperty);

            propertyOperations.ForEach(op =>
            {
                op.DestinationProperty = newProperty;
                op.DestinationPropertyId = newProperty.PropertyId;

                // Keep the source navigation for newly-created source properties so EF can resolve SOURCE_PROPERTY_ID.
                if (op.SourcePropertyId > 0)
                {
                    op.SourceProperty = null; // Use FK-only for existing source properties so they are not modified in add-range.
                }
            });
        }

        private PimsProperty PrepareDestinationProperty(PimsProperty destinationProperty)
        {
            destinationProperty.PropertyId = 0; // in the case this property already exists, this will force it to be recreated.
            ValidateRegionDistrict(destinationProperty, "Destination property");
            return _propertyService.PopulateNewProperty(destinationProperty, isOwned: true, isPropertyOfInterest: false);
        }

        private void RetireResolvedSourceProperty(PimsProperty sourceProperty)
        {
            // New source properties may be staged (not yet persisted) and have no database id.
            // Mark them retired in-memory so the final repository commit persists the retired state.
            if (sourceProperty.PropertyId > 0)
            {
                _propertyService.RetireProperty(sourceProperty, false);
            }
            else
            {
                sourceProperty.IsRetired = true;
            }
        }

        private PimsProperty ResolveOrCreateSourceProperty(PimsPropertyOperation operation, string operationName)
        {
            if (operation.SourcePropertyId > 0)
            {
                return _propertyService.GetById(operation.SourcePropertyId);
            }

            var sourceProperty = operation.SourceProperty;
            if (sourceProperty?.Pid == null)
            {
                throw new BadRequestException("A valid source property with PID is required.");
            }

            try
            {
                return _propertyService.GetByPid(sourceProperty.Pid.ToString());
            }
            catch (KeyNotFoundException)
            {
                _logger.LogInformation(
                    "Creating source property in PIMS for PID {Pid} before {OperationName}.",
                    sourceProperty.Pid,
                    operationName);

                // When creating a brand new source property from operation payload,
                // copy region/district from destination if source codes were not provided.
                if (sourceProperty.RegionCode <= 0 && operation.DestinationProperty?.RegionCode > 0)
                {
                    sourceProperty.RegionCode = operation.DestinationProperty.RegionCode;
                }

                if (sourceProperty.DistrictCode <= 0 && operation.DestinationProperty?.DistrictCode > 0)
                {
                    sourceProperty.DistrictCode = operation.DestinationProperty.DistrictCode;
                }

                if (sourceProperty.RegionCode <= 0 || sourceProperty.DistrictCode <= 0)
                {
                    throw new BadRequestException("Source property must include a valid region and district when creating a new property.");
                }

                ValidateRegionDistrict(sourceProperty, "Source property");

                sourceProperty.PropertyId = 0;
                var newSourceProperty = _propertyService.PopulateNewProperty(sourceProperty, sourceProperty.IsOwned, isPropertyOfInterest: false);
                return _propertyService.Add(newSourceProperty, commitTransaction: false);
            }
        }

        private void ValidateRegionDistrict(PimsProperty property, string propertyLabel)
        {
            if (property == null)
            {
                throw new BadRequestException($"{propertyLabel} is required.");
            }

            if (property.RegionCode <= 0 || property.DistrictCode <= 0)
            {
                throw new BadRequestException($"{propertyLabel} must include a valid region and district.");
            }

            var validRegion = _lookupRepository
                .GetAllRegions()
                .Any(r => r.RegionCode == property.RegionCode && !r.IsDisabled);

            if (!validRegion)
            {
                throw new BadRequestException($"{propertyLabel} region code '{property.RegionCode}' is not valid in PIMS.");
            }

            var district = _lookupRepository
                .GetAllDistricts()
                .FirstOrDefault(d => d.DistrictCode == property.DistrictCode && !d.IsDisabled);

            if (district == null)
            {
                throw new BadRequestException($"{propertyLabel} district code '{property.DistrictCode}' is not valid in PIMS.");
            }

            if (district.RegionCode != property.RegionCode)
            {
                throw new BadRequestException($"{propertyLabel} district code '{property.DistrictCode}' does not belong to region code '{property.RegionCode}'.");
            }
        }
    }
}
