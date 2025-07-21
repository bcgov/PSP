using System.Collections.Generic;
using Pims.Api.Models.Concepts.Property;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;

namespace Pims.Api.Services
{
    public interface IPropertyService
    {
        PimsProperty GetById(long id);

        List<PimsProperty> GetMultipleById(List<long> ids);

        PimsProperty GetByPid(string pid);

        PimsProperty GetByPin(int pin);

        PimsProperty Update(PimsProperty property, bool commitTransaction = true);

        PimsProperty RetireProperty(PimsProperty property, bool commitTransaction = true);

        IList<PimsPropertyContact> GetContacts(long propertyId);

        PimsPropertyContact GetContact(long propertyId, long contactId);

        PimsPropertyContact CreateContact(PimsPropertyContact propertyContact);

        PimsPropertyContact UpdateContact(PimsPropertyContact propertyContact);

        bool DeleteContact(long propertyContactId);

        PropertyManagementModel GetPropertyManagement(long propertyId);

        PropertyManagementModel UpdatePropertyManagement(PimsProperty property);

        IList<PimsManagementActivity> GetActivities(long propertyId);

        IList<PimsManagementActivity> GetFileActivities(long managementFileId);

        PimsManagementActivity GetActivity(long activityId);

        IEnumerable<PimsManagementActivity> GetActivitiesByPropertyIds(IEnumerable<long> propertyIds);

        PimsManagementActivity CreateActivity(PimsManagementActivity managementActivity);

        PimsManagementActivity UpdateActivity(PimsManagementActivity managementActivity);

        bool DeleteActivity(long activityId);

        bool DeleteFileActivity(long managementFileId, long activityId);

        PimsProperty PopulateNewProperty(PimsProperty property, bool isOwned = false, bool isPropertyOfInterest = true);

        void UpdateLocation(PimsProperty incomingProperty, ref PimsProperty propertyToUpdate, IEnumerable<UserOverrideCode> overrideCodes, bool allowRetired = false);

        T PopulateNewFileProperty<T>(T fileProperty)
            where T : IFilePropertyEntity;

        void UpdateFilePropertyLocation<T>(T incomingFileProperty, T filePropertyToUpdate)
            where T : IFilePropertyEntity;

        IList<PimsHistoricalFileNumber> GetHistoricalNumbersForPropertyId(long propertyId);

        IList<PimsHistoricalFileNumber> UpdateHistoricalFileNumbers(long propertyId, IEnumerable<PimsHistoricalFileNumber> pimsHistoricalNumbers);

        /// <summary>
        /// Returns the spatial location and boundary polygons in lat/long (4326) for a given property.
        /// The spatial values will be modified in-place.
        /// </summary>
        /// <param name="property">The property to re-project.</param>
        /// <returns>The property with transformed spatial locations.</returns>
        PimsProperty TransformPropertyToLatLong(PimsProperty property);

        /// <summary>
        /// Returns the spatial location and boundary polygons in lat/long (4326) for a list of file properties.
        /// The spatial values will be modified in-place.
        /// </summary>
        /// <param name="fileProperties">The file properties to re-project.</param>
        /// <returns>The file properties with transformed spatial locations.</returns>
        List<T> TransformAllPropertiesToLatLong<T>(List<T> fileProperties)
            where T : IFilePropertyEntity;
    }
}
