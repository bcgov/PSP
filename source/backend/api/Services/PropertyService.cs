using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using Pims.Api.Constants;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Models.Concepts.Property;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
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
        private readonly IHistoricalNumberRepository _historicalNumberRepository;
        private readonly IPropertyContactRepository _propertyContactRepository;
        private readonly IPropertyActivityRepository _propertyActivityRepository;
        private readonly ICoordinateTransformService _coordinateService;
        private readonly IPropertyLeaseRepository _propertyLeaseRepository;
        private readonly IMapper _mapper;
        private readonly ILookupRepository _lookupRepository;

        public PropertyService(
            ClaimsPrincipal user,
            ILogger<PropertyService> logger,
            IPropertyRepository propertyRepository,
            IHistoricalNumberRepository historicalNumberRepository,
            IPropertyContactRepository propertyContactRepository,
            IPropertyActivityRepository propertyActivityRepository,
            ICoordinateTransformService coordinateService,
            IPropertyLeaseRepository propertyLeaseRepository,
            IMapper mapper,
            ILookupRepository lookupRepository)
        {
            _user = user;
            _logger = logger;
            _propertyRepository = propertyRepository;
            _historicalNumberRepository = historicalNumberRepository;
            _propertyContactRepository = propertyContactRepository;
            _propertyActivityRepository = propertyActivityRepository;
            _coordinateService = coordinateService;
            _propertyLeaseRepository = propertyLeaseRepository;
            _mapper = mapper;
            _lookupRepository = lookupRepository;
        }

        public PimsProperty GetById(long id)
        {
            _logger.LogInformation("Getting property with id {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.PropertyView);

            var property = _propertyRepository.GetById(id);
            return TransformPropertyToLatLong(property);
        }

        public List<PimsProperty> GetMultipleById(List<long> ids)
        {
            _logger.LogInformation("Getting multiple properties by id");
            _user.ThrowIfNotAuthorized(Permissions.PropertyView);

            List<PimsProperty> properties = _propertyRepository.GetAllByIds(ids);
            foreach (PimsProperty property in properties)
            {
                TransformPropertyToLatLong(property);
            }

            return properties;
        }

        public PimsProperty GetByPid(string pid)
        {
            _logger.LogInformation("Getting property with pid {pid}", pid);
            _user.ThrowIfNotAuthorized(Permissions.PropertyView);

            // return property spatial location in lat/long (4326)
            var property = _propertyRepository.GetByPid(pid);
            return TransformPropertyToLatLong(property);
        }

        public PimsProperty GetByPin(int pin)
        {
            _logger.LogInformation("Getting property with pin {pin}", pin);
            _user.ThrowIfNotAuthorized(Permissions.PropertyView);

            // return property spatial location in lat/long (4326)
            var property = _propertyRepository.GetByPin(pin);
            return TransformPropertyToLatLong(property);
        }

        public PimsProperty Update(PimsProperty property, bool commitTransaction = true)
        {
            _logger.LogInformation("Updating property with id {id}", property.Internal_Id);
            _user.ThrowIfNotAuthorized(Permissions.PropertyEdit);

            // convert spatial location from lat/long (4326) to BC Albers (3005) for database storage
            var geom = property.Location;
            if (geom.SRID != SpatialReference.BCALBERS)
            {
                var newCoords = _coordinateService.TransformCoordinates(geom.SRID, SpatialReference.BCALBERS, geom.Coordinate);
                property.Location = GeometryHelper.CreatePoint(newCoords, SpatialReference.BCALBERS);
            }

            var newProperty = _propertyRepository.Update(property);
            if (commitTransaction)
            {
                _propertyRepository.CommitTransaction();
            }

            return GetById(newProperty.Internal_Id);
        }

        public PimsProperty RetireProperty(PimsProperty property, bool commitTransaction = true)
        {
            _logger.LogInformation("Retiring property with id {id}", property.Internal_Id);
            _user.ThrowIfNotAuthorized(Permissions.PropertyEdit);

            var retiredProperty = _propertyRepository.RetireProperty(property);
            if (commitTransaction)
            {
                _propertyRepository.CommitTransaction();
            }
            return GetById(retiredProperty.Internal_Id); ;
        }

        public IList<PimsPropertyContact> GetContacts(long propertyId)
        {
            _logger.LogInformation("Retrieving property contacts...");
            _user.ThrowIfNotAuthorized(Permissions.PropertyView);

            return _propertyContactRepository.GetContactsByProperty(propertyId);
        }

        public PimsPropertyContact GetContact(long propertyId, long contactId)
        {
            _logger.LogInformation("Retrieving single property contact...");
            _user.ThrowIfNotAuthorized(Permissions.PropertyView);

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
            var leaseCount = propertyLeases.Count();
            var firstLease = leaseCount == 1 ? propertyLeases.First().Lease : null;
            var leaseExpiryDate = firstLease is not null ? firstLease.GetExpiryDate()?.FilterSqlMinDate() : null;

            var propertyManagement = _mapper.Map<PropertyManagementModel>(property);
            propertyManagement.RelatedLeases = leaseCount;
            propertyManagement.LeaseExpiryDate = leaseExpiryDate.HasValue ? DateOnly.FromDateTime(leaseExpiryDate.Value) : null;

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

        public IList<PimsPropertyActivity> GetActivities(long propertyId)
        {
            _logger.LogInformation("Getting property management activities for property with id {propertyId}", propertyId);
            _user.ThrowIfNotAuthorized(Permissions.ManagementView, Permissions.PropertyView);

            return _propertyActivityRepository.GetActivitiesByProperty(propertyId);
        }

        public PimsPropertyActivity GetActivity(long propertyId, long activityId)
        {
            _logger.LogInformation("Retrieving single property Activity...");
            _user.ThrowIfNotAuthorized(Permissions.ManagementView, Permissions.PropertyView);

            var propertyActivity = _propertyActivityRepository.GetActivity(activityId);

            if (propertyActivity.PimsPropPropActivities.Any(x => x.PropertyId == propertyId))
            {
                return propertyActivity;
            }

            throw new BadRequestException("Activity with the given id does not match the property id");
        }

        public PimsPropertyActivity CreateActivity(PimsPropertyActivity propertyActivity)
        {
            _logger.LogInformation("Creating property Activity...");
            _user.ThrowIfNotAuthorized(Permissions.ManagementAdd, Permissions.PropertyEdit);

            if (propertyActivity.PropMgmtActivityStatusTypeCode == null)
            {
                propertyActivity.PropMgmtActivityStatusTypeCode = "NOTSTARTED";
            }

            var propertyActivityResult = _propertyActivityRepository.Create(propertyActivity);
            _propertyActivityRepository.CommitTransaction();

            return propertyActivityResult;
        }

        public PimsPropertyActivity UpdateActivity(long propertyId, long activityId, PimsPropertyActivity propertyActivity)
        {
            _logger.LogInformation("Updating property Activity...");
            _user.ThrowIfNotAuthorized(Permissions.ManagementEdit, Permissions.PropertyEdit);

            if (!propertyActivity.PimsPropPropActivities.Any(x => x.PropertyId == propertyId && x.PimsPropertyActivityId == activityId)
                || propertyActivity.PimsPropertyActivityId != activityId)
            {
                throw new BadRequestException("Invalid activity identifiers.");
            }

            var propertyActivityResult = _propertyActivityRepository.Update(propertyActivity);
            _propertyActivityRepository.CommitTransaction();

            return propertyActivityResult;
        }

        public bool DeleteActivity(long activityId)
        {
            _logger.LogInformation("Deleting Management Activity with id {activityId}", activityId);
            _user.ThrowIfNotAuthorized(Permissions.ManagementDelete, Permissions.PropertyEdit);

            var propertyManagementActivity = _propertyActivityRepository.GetActivity(activityId);

            if (!propertyManagementActivity.PropMgmtActivityStatusTypeCode.Equals(PropertyActivityStatusTypeCode.NOTSTARTED.ToString()))
            {
                throw new BadRequestException($"PropertyManagementActivity can not be deleted since it has already started");
            }

            var success = _propertyActivityRepository.TryDelete(activityId);
            _propertyRepository.CommitTransaction();

            return success;
        }

        public PimsProperty PopulateNewProperty(PimsProperty property, bool isOwned = false, bool isPropertyOfInterest = true)
        {
            property.PropertyClassificationTypeCode = "UNKNOWN";
            property.PropertyDataSourceEffectiveDate = DateOnly.FromDateTime(System.DateTime.Now);
            property.PropertyDataSourceTypeCode = "PMBC";

            property.PropertyTypeCode = "UNKNOWN";

            property.PropertyStatusTypeCode = "UNKNOWN";
            property.SurplusDeclarationTypeCode = "UNKNOWN";

            property.IsOwned = isOwned;

            if (property.Address != null)
            {
                var provinceId = _lookupRepository.GetAllProvinces().FirstOrDefault(p => p.ProvinceStateCode == "BC")?.Id;
                if (provinceId.HasValue)
                {
                    property.Address.ProvinceStateId = provinceId.Value;
                }
                property.Address.CountryId = _lookupRepository.GetAllCountries().FirstOrDefault(p => p.CountryCode == "CA")?.Id;
            }

            // convert spatial location from lat/long (4326) to BC Albers (3005) for database storage
            var geom = property.Location;
            if (geom.SRID != SpatialReference.BCALBERS)
            {
                var newCoords = _coordinateService.TransformCoordinates(geom.SRID, SpatialReference.BCALBERS, geom.Coordinate);
                property.Location = GeometryHelper.CreatePoint(newCoords, SpatialReference.BCALBERS);
            }

            // apply similar logic to the boundary
            var boundaryGeom = property.Boundary;
            if (boundaryGeom != null && boundaryGeom.SRID != SpatialReference.BCALBERS)
            {
                _coordinateService.TransformGeometry(boundaryGeom.SRID, SpatialReference.BCALBERS, boundaryGeom);
            }

            return property;
        }

        public void UpdateLocation(PimsProperty incomingProperty, ref PimsProperty propertyToUpdate, IEnumerable<UserOverrideCode> overrideCodes)
        {
            if (propertyToUpdate.Location == null || propertyToUpdate.Boundary == null)
            {
                if (overrideCodes.Contains(UserOverrideCode.AddLocationToProperty))
                {
                    var needsUpdate = false;

                    // convert spatial location from lat/long (4326) to BC Albers (3005) for database storage
                    var geom = incomingProperty.Location;
                    if (geom.SRID != SpatialReference.BCALBERS)
                    {
                        var newCoords = _coordinateService.TransformCoordinates(geom.SRID, SpatialReference.BCALBERS, geom.Coordinate);
                        propertyToUpdate.Location = GeometryHelper.CreatePoint(newCoords, SpatialReference.BCALBERS);
                        needsUpdate = true;
                    }

                    // apply similar logic to the boundary
                    var boundaryGeom = incomingProperty.Boundary;
                    if (boundaryGeom != null && boundaryGeom.SRID != SpatialReference.BCALBERS)
                    {
                        _coordinateService.TransformGeometry(boundaryGeom.SRID, SpatialReference.BCALBERS, boundaryGeom);
                        propertyToUpdate.Boundary = boundaryGeom;
                        needsUpdate = true;
                    }

                    if (needsUpdate)
                    {
                        _propertyRepository.Update(propertyToUpdate, overrideLocation: true);
                    }
                }
                else
                {
                    throw new UserOverrideException(UserOverrideCode.AddLocationToProperty, "The selected property already exists in the system's inventory. However, the record is missing spatial details.\n\n To add the property, the spatial details for this property will need to be updated. The system will attempt to update the property record with spatial information from the current selection.");
                }
            }
        }

        public T PopulateNewFileProperty<T>(T fileProperty)
        {
            // TODO: Remove this casting when LOCATION gets added to all remaining file-property types (research, disposition, lease)
            if (fileProperty is PimsPropertyAcquisitionFile acquisitionFileProperty)
            {
                // convert spatial location from lat/long (4326) to BC Albers (3005) for database storage
                var geom = acquisitionFileProperty.Location;
                if (geom is not null && geom.SRID != SpatialReference.BCALBERS)
                {
                    var newCoords = _coordinateService.TransformCoordinates(geom.SRID, SpatialReference.BCALBERS, geom.Coordinate);
                    acquisitionFileProperty.Location = GeometryHelper.CreatePoint(newCoords, SpatialReference.BCALBERS);
                }
            }

            return fileProperty;
        }

        public void UpdateFilePropertyLocation<T>(T incomingFileProperty, T filePropertyToUpdate)
            where T : IWithPropertyEntity
        {
            // TODO: Remove this casting when LOCATION gets added to all remaining file-property types (research, disposition, lease)
            if (incomingFileProperty is PimsPropertyAcquisitionFile incomingAcquisitionProperty
                && filePropertyToUpdate is PimsPropertyAcquisitionFile acquisitionPropertyToUpdate)
            {
                // convert spatial location from lat/long (4326) to BC Albers (3005) for database storage
                var geom = incomingAcquisitionProperty.Location;
                if (geom is not null && geom.SRID != SpatialReference.BCALBERS)
                {
                    var newCoords = _coordinateService.TransformCoordinates(geom.SRID, SpatialReference.BCALBERS, geom.Coordinate);
                    acquisitionPropertyToUpdate.Location = GeometryHelper.CreatePoint(newCoords, SpatialReference.BCALBERS);
                }
            }
        }

        public IList<PimsHistoricalFileNumber> GetHistoricalNumbersForPropertyId(long propertyId)
        {

            _logger.LogInformation("Retrieving all historical numbers for property with id {id}", propertyId);
            _user.ThrowIfNotAuthorized(Permissions.PropertyView);

            return _historicalNumberRepository.GetAllByPropertyId(propertyId);
        }

        public IList<PimsHistoricalFileNumber> UpdateHistoricalFileNumbers(long propertyId, IEnumerable<PimsHistoricalFileNumber> pimsHistoricalNumbers)
        {

            _logger.LogInformation("Updating historical numbers for property with id {id}", propertyId);
            _user.ThrowIfNotAuthorized(Permissions.PropertyEdit);

            bool duplicateType = pimsHistoricalNumbers.Where(x => x.HistoricalFileNumberTypeCode != HistoricalFileNumberTypes.OTHER.ToString())
                .GroupBy(p => (p.HistoricalFileNumberTypeCode, p.HistoricalFileNumber)).Any(g => g.Count() > 1);

            bool duplicateOtherType = pimsHistoricalNumbers.Where(x => x.HistoricalFileNumberTypeCode == HistoricalFileNumberTypes.OTHER.ToString())
                .GroupBy(p => (p.OtherHistFileNumberTypeCode, p.HistoricalFileNumber)).Any(g => g.Count() > 1);

            if (duplicateType || duplicateOtherType)
            {
                throw new DuplicateEntityException("You cannot add a duplicate historical number.");
            }

            _historicalNumberRepository.UpdateHistoricalFileNumbers(propertyId, pimsHistoricalNumbers);
            _historicalNumberRepository.CommitTransaction();

            return GetHistoricalNumbersForPropertyId(propertyId);
        }

        /// <inheritdoc />
        public List<T> TransformAllPropertiesToLatLong<T>(List<T> fileProperties)
            where T : IWithPropertyEntity
        {
            foreach (var fileProperty in fileProperties)
            {
                // TODO: Remove this casting when LOCATION gets added to all remaining file-property types (research, disposition, lease)
                if (fileProperty is PimsPropertyAcquisitionFile acquisitionFileProperty && acquisitionFileProperty.Location is not null)
                {
                    acquisitionFileProperty.Location = TransformCoordinates(acquisitionFileProperty.Location);
                }

                TransformPropertyToLatLong(fileProperty.Property);
            }

            return fileProperties;
        }

        /// <inheritdoc />
        public PimsProperty TransformPropertyToLatLong(PimsProperty property)
        {
            // transform property location (map pin)
            if (property?.Location != null)
            {
                property.Location = TransformCoordinates(property.Location);
            }

            // transform property boundary in-place (polygon/multipolygon)
            if (property?.Boundary != null)
            {
                _coordinateService.TransformGeometry(SpatialReference.BCALBERS, SpatialReference.WGS84, property.Boundary);
            }

            return property;
        }

        private Point TransformCoordinates(Geometry location)
        {
            // return property spatial location in lat/long (4326)
            var newCoords = _coordinateService.TransformCoordinates(SpatialReference.BCALBERS, SpatialReference.WGS84, location.Coordinate);
            return GeometryHelper.CreatePoint(newCoords, SpatialReference.WGS84);
        }
    }
}
