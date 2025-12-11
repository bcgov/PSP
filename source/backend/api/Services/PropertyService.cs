using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using Pims.Api.Constants;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Models.Concepts.Property;
using Pims.Core.Api.Exceptions;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Helpers;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;

namespace Pims.Api.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IHistoricalNumberRepository _historicalNumberRepository;
        private readonly IPropertyContactRepository _propertyContactRepository;
        private readonly IManagementActivityRepository _managementActivityRepository;
        private readonly ICoordinateTransformService _coordinateService;
        private readonly IPropertyLeaseRepository _propertyLeaseRepository;
        private readonly IDocumentFileService _documentFileService;
        private readonly IMapper _mapper;
        private readonly ILookupRepository _lookupRepository;

        public PropertyService(
            ClaimsPrincipal user,
            ILogger<PropertyService> logger,
            IPropertyRepository propertyRepository,
            IHistoricalNumberRepository historicalNumberRepository,
            IPropertyContactRepository propertyContactRepository,
            IManagementActivityRepository managementActivityRepository,
            ICoordinateTransformService coordinateService,
            IPropertyLeaseRepository propertyLeaseRepository,
            IDocumentFileService documentFileService,
            IMapper mapper,
            ILookupRepository lookupRepository)
        {
            _user = user;
            _logger = logger;
            _propertyRepository = propertyRepository;
            _historicalNumberRepository = historicalNumberRepository;
            _propertyContactRepository = propertyContactRepository;
            _managementActivityRepository = managementActivityRepository;
            _coordinateService = coordinateService;
            _propertyLeaseRepository = propertyLeaseRepository;
            _documentFileService = documentFileService;
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
            if (geom != null && geom.SRID != SpatialReference.BCALBERS)
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
            return GetById(retiredProperty.Internal_Id);
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

            PropertyHasActiveLease(propertyLeases, out bool hasActiveLease, out bool hasActiveExpiryDate);
            propertyManagement.HasActiveLease = hasActiveLease;
            propertyManagement.ActiveLeaseHasExpiryDate = hasActiveExpiryDate;

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

        public IList<PimsManagementActivity> GetActivities(long propertyId)
        {
            _logger.LogInformation("Getting property management activities for property with id {propertyId}", propertyId);
            _user.ThrowIfNotAllAuthorized(Permissions.ManagementView, Permissions.PropertyView);

            return _managementActivityRepository.GetActivitiesByProperty(propertyId);
        }

        public IList<PimsManagementActivity> GetFileActivities(long managementFileId)
        {
            _logger.LogInformation("Getting property management activities for management file with id {managementFileId}", managementFileId);
            _user.ThrowIfNotAllAuthorized(Permissions.ManagementView, Permissions.PropertyView);

            return _managementActivityRepository.GetActivitiesByManagementFile(managementFileId);
        }

        public PimsManagementActivity GetActivity(long activityId)
        {
            _logger.LogInformation("Retrieving property Activity with Id: {ActivityId}", activityId);
            _user.ThrowIfNotAllAuthorized(Permissions.ManagementView, Permissions.PropertyView);

            return _managementActivityRepository.GetActivity(activityId);
        }

        public IEnumerable<PimsManagementActivity> GetActivitiesByPropertyIds(IEnumerable<long> propertyIds)
        {
            _logger.LogInformation("Retrieving multiple property Activities... {propertyIds}", propertyIds);
            _user.ThrowIfNotAllAuthorized(Permissions.ManagementView, Permissions.PropertyView);

            var managementFilePropertyIds = _managementActivityRepository.GetActivitiesByPropertyIds(propertyIds);

            return managementFilePropertyIds;
        }

        public PimsManagementActivity CreateActivity(PimsManagementActivity managementActivity)
        {
            _logger.LogInformation("Creating property Activity...");
            _user.ThrowIfNotAllAuthorized(Permissions.ManagementAdd, Permissions.PropertyEdit);

            if (managementActivity.MgmtActivityStatusTypeCode == null)
            {
                managementActivity.MgmtActivityStatusTypeCode = ManagementActivityStatusTypes.NOTSTARTED.ToString();
            }

            var managementActivityResult = _managementActivityRepository.Create(managementActivity);
            _managementActivityRepository.CommitTransaction();

            return managementActivityResult;
        }

        public PimsManagementActivity UpdateActivity(PimsManagementActivity managementActivity)
        {
            managementActivity.ThrowIfNull(nameof(managementActivity));
            _logger.LogInformation("Updating property Activity with Id: {ActivityId}", managementActivity.Internal_Id);
            _user.ThrowIfNotAllAuthorized(Permissions.ManagementEdit, Permissions.PropertyEdit);

            var managementActivityResult = _managementActivityRepository.Update(managementActivity);
            _managementActivityRepository.CommitTransaction();

            return managementActivityResult;
        }

        public bool DeleteFileActivity(long managementFileId, long activityId)
        {
            _logger.LogInformation("Deleting Management Activity with id {activityId} for file {managementFileId}", activityId, managementFileId);
            _user.ThrowIfNotAllAuthorized(Permissions.ManagementDelete, Permissions.PropertyEdit);

            var managementActivity = _managementActivityRepository.GetActivity(activityId);

            if (managementActivity.ManagementFileId != managementFileId)
            {
                throw new BadRequestException("Activity with the given id does not match the management file id");
            }

            return DeleteActivity(activityId);
        }

        public bool DeleteActivity(long activityId)
        {
            _logger.LogInformation("Deleting Management Activity with id {activityId}", activityId);
            _user.ThrowIfNotAllAuthorized(Permissions.ManagementDelete, Permissions.PropertyEdit);

            var propertyManagementActivity = _managementActivityRepository.GetActivity(activityId);

            if (!propertyManagementActivity.MgmtActivityStatusTypeCode.Equals(ManagementActivityStatusTypeCode.NOTSTARTED.ToString()))
            {
                throw new BadRequestException($"PropertyManagementActivity can not be deleted given it has already started");
            }

            var activityDocuments = _documentFileService.GetFileDocuments<PimsMgmtActivityDocument>(FileType.ManagementActivity, activityId);
            if (activityDocuments.Count > 0)
            {
                throw new BadRequestException($"PropertyManagementActivity can not be deleted. There is at least one document related to it.");
            }

            var success = _managementActivityRepository.TryDelete(activityId);
            _propertyRepository.CommitTransaction();

            return success;
        }

        public PimsProperty PopulateNewProperty(PimsProperty property, bool isOwned = false, bool isPropertyOfInterest = true)
        {
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
            if (geom != null && geom.SRID != SpatialReference.BCALBERS)
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

        public void UpdateLocation(PimsProperty incomingProperty, ref PimsProperty propertyToUpdate, IEnumerable<UserOverrideCode> overrideCodes, bool allowRetired = false)
        {
            if (propertyToUpdate.Location == null || propertyToUpdate.Boundary == null)
            {
                if (overrideCodes.Contains(UserOverrideCode.AddLocationToProperty))
                {
                    var needsUpdate = false;

                    // convert spatial location from lat/long (4326) to BC Albers (3005) for database storage
                    var geom = incomingProperty.Location;
                    if (geom != null && geom.SRID != SpatialReference.BCALBERS)
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
                        _propertyRepository.Update(propertyToUpdate, overrideLocation: true, allowRetired: allowRetired);
                    }
                }
                else
                {
                    throw new UserOverrideException(UserOverrideCode.AddLocationToProperty, "The selected property already exists in the system's inventory. However, the record is missing spatial details.\n\n To add the property, the spatial details for this property will need to be updated. The system will attempt to update the property record with spatial information from the current selection.");
                }
            }
        }

        /// <inheritdoc />
        public T PopulateNewFileProperty<T>(T fileProperty)
            where T : IFilePropertyEntity
        {
            // convert spatial location from lat/long (4326) to BC Albers (3005) for database storage
            var geom = fileProperty.Location;
            if (geom is not null && geom.SRID != SpatialReference.BCALBERS)
            {
                var newCoords = _coordinateService.TransformCoordinates(geom.SRID, SpatialReference.BCALBERS, geom.Coordinate);
                fileProperty.Location = GeometryHelper.CreatePoint(newCoords, SpatialReference.BCALBERS);
            }

            return fileProperty;
        }

        /// <inheritdoc />
        public void UpdateFilePropertyLocation<T>(T incomingFileProperty, T filePropertyToUpdate)
            where T : IFilePropertyEntity
        {
            // convert spatial location from lat/long (4326) to BC Albers (3005) for database storage
            var geom = incomingFileProperty.Location;
            if (geom is not null && geom.SRID != SpatialReference.BCALBERS)
            {
                var newCoords = _coordinateService.TransformCoordinates(geom.SRID, SpatialReference.BCALBERS, geom.Coordinate);
                filePropertyToUpdate.Location = GeometryHelper.CreatePoint(newCoords, SpatialReference.BCALBERS);
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
            where T : IFilePropertyEntity
        {
            foreach (var fileProperty in fileProperties)
            {
                if (fileProperty.Location is not null)
                {
                    fileProperty.Location = TransformCoordinates(fileProperty.Location);
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

        private static void PropertyHasActiveLease(IEnumerable<PimsPropertyLease> propertyLeases, out bool hasActiveLease, out bool hasActiveExpiryDate)
        {
            hasActiveLease = false;
            hasActiveExpiryDate = false;

            List<PimsLease> activeLeaseList = propertyLeases.Select(x => x.Lease).Where(y => y.LeaseStatusTypeCode == LeaseStatusTypes.ACTIVE.ToString()).ToList();
            foreach (var agreement in activeLeaseList)
            {
                if (!agreement.TerminationDate.HasValue)
                {
                    var latestRenewal = agreement.PimsLeaseRenewals.Where(x => x.IsExercised == true).OrderByDescending(x => x.CommencementDt).FirstOrDefault();
                    if (latestRenewal is null) // No Renewal - Check only Lease dates.
                    {
                        if (agreement.OrigExpiryDate.HasValue && agreement.OrigExpiryDate.Value.Date >= DateTime.Now.Date)
                        {
                            hasActiveLease = hasActiveExpiryDate = true;
                        }
                        else if (!agreement.OrigExpiryDate.HasValue)
                        {
                            hasActiveLease = true;
                        }
                    }
                    else
                    {
                        if (agreement.OrigExpiryDate.HasValue && latestRenewal.ExpiryDt.HasValue)
                        {
                            hasActiveLease = hasActiveExpiryDate = agreement.OrigExpiryDate.Value.Date >= DateTime.Now.Date || latestRenewal.ExpiryDt.Value.Date >= DateTime.Now.Date;
                        }
                        else if (agreement.OrigExpiryDate.HasValue && !latestRenewal.ExpiryDt.HasValue)
                        {
                            hasActiveLease = true;
                        }
                        else if (!agreement.OrigExpiryDate.HasValue && latestRenewal.ExpiryDt.HasValue)
                        {
                            hasActiveLease = latestRenewal.ExpiryDt.Value.Date >= DateTime.Now.Date;
                        }
                        else
                        {
                            hasActiveLease = true;
                        }
                    }
                }
            }
        }

        private Point TransformCoordinates(Geometry location)
        {
            // return property spatial location in lat/long (4326)
            var newCoords = _coordinateService.TransformCoordinates(SpatialReference.BCALBERS, SpatialReference.WGS84, location.Coordinate);
            return GeometryHelper.CreatePoint(newCoords, SpatialReference.WGS84);
        }
    }
}
