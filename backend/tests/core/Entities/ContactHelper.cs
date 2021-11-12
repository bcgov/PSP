using Pims.Dal.Entities;
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
        public static Entity.Contact CreateContact(string id, bool isDisabled = false, Address address = null, Organization organization = null, Person person = null)
        {
            var contact = new Entity.Contact()
            {
                Id = id,
                Address = address,
                FirstName = person?.FirstName ?? "firstName",
                Surname = person?.Surname ?? "surName",
                Summary = organization?.Name ?? person?.GetFullName(),
                IsDisabled = isDisabled,
                Organization = organization,
                Person = person,
                OrganizationName = organization?.Name ?? "organization name",
                Municipality = address.Municipality,
            };
            return contact;
        }

        /// <summary>
        /// Create a new instance of a Contact, using string values
        /// </summary>
        /// <returns></returns>
        public static Entity.Contact CreateContact(string id, bool isDisabled = false, string municipality = null, string firstName = null, string surname = null, string organizationName = null)
        {
            var address = new Address() { Municipality = municipality };
            var organization = organizationName != null ? new Organization() { Name = organizationName, Address = address } : null;
            var person = firstName != null ? new Person() { FirstName = firstName, Surname = surname, Address = address } : null;
            
            return CreateContact(id, isDisabled, address, organization, person);
        }
    }
}
