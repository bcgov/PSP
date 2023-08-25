using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Entity = Pims.Dal.Entities;

namespace Pims.Core.Test
{
    /// <summary>
    /// EntityHelper static class, provides helper methods to create test entities.
    /// </summary>
    public static partial class EntityHelper
    {
        /// <summary>
        /// Create a new instance of a Contact, using entities
        /// </summary>
        /// <returns></returns>
        public static Entity.PimsContactMgrVw CreateContact(string id, bool isDisabled = false, PimsAddress address = null, PimsOrganization organization = null, PimsPerson person = null)
        {
            var contact = new Entity.PimsContactMgrVw()
            {
                Id = id,
                Address = address,
                FirstName = person?.FirstName ?? "firstName",
                Surname = person?.Surname ?? "surName",
                Summary = organization?.OrganizationName ?? person?.GetFullName(),
                IsDisabled = isDisabled,
                Organization = organization,
                OrganizationId = organization?.Internal_Id,
                Person = person,
                PersonId = person?.Internal_Id,
                OrganizationName = organization?.OrganizationName ?? string.Empty,
                MunicipalityName = address.MunicipalityName,
            };
            return contact;
        }

        /// <summary>
        /// Create a new instance of a Contact, using string values
        /// </summary>
        /// <returns></returns>
        public static Entity.PimsContactMgrVw CreateContact(string id, bool isDisabled = false, string municipality = null, string firstName = null, string surname = null, string organizationName = null)
        {
            var address = new PimsAddress() { MunicipalityName = municipality };
            var organization = organizationName != null ? new PimsOrganization() { OrganizationName = organizationName, PimsOrganizationAddresses = new List<PimsOrganizationAddress>() { new PimsOrganizationAddress() { Address = address } } } : null;
            var person = firstName != null ? new PimsPerson() { FirstName = firstName, Surname = surname, PimsPersonAddresses = new List<PimsPersonAddress>() { new PimsPersonAddress() { Address = address } } } : null;

            return CreateContact(id, isDisabled, address, organization, person);
        }
    }
}
