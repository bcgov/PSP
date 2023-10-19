using System.Collections.Generic;
using Pims.Api.Models.Concepts;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface IPropertyService
    {
        PimsProperty GetById(long id);

        List<PimsProperty> GetMultipleById(List<long> ids);

        PimsProperty GetByPid(string pid);

        PimsProperty Update(PimsProperty property);

        IList<PimsPropertyContact> GetContacts(long propertyId);

        PimsPropertyContact GetContact(long propertyId, long contactId);

        PimsPropertyContact CreateContact(PimsPropertyContact propertyContact);

        PimsPropertyContact UpdateContact(PimsPropertyContact propertyContact);

        bool DeleteContact(long propertyContactId);

        PropertyManagementModel GetPropertyManagement(long propertyId);

        PropertyManagementModel UpdatePropertyManagement(PimsProperty property);

        IList<PimsPropPropActivity> GetManagementActivities(long propertyId);

        bool DeleteManagementActivity(long propertyId, long managementActivityId);
    }
}
