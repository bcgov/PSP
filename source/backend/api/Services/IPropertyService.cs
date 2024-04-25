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

        IList<PimsPropertyActivity> GetActivities(long propertyId);

        PimsPropertyActivity GetActivity(long propertyId, long activityId);

        PimsPropertyActivity CreateActivity(PimsPropertyActivity propertyActivity);

        PimsPropertyActivity UpdateActivity(long propertyId, long activityId, PimsPropertyActivity propertyActivity);

        bool DeleteActivity(long activityId);

        PimsProperty PopulateNewProperty(PimsProperty property, bool isOwned = false, bool isPropertyOfInterest = true);

        void UpdateLocation(PimsProperty acquisitionProperty, ref PimsProperty propertyToUpdate, IEnumerable<UserOverrideCode> overrideCodes);
    }
}
