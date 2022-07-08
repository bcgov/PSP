using System;
using System.Linq;

namespace Pims.Dal.Entities.Helpers
{
    public static class OrganizationExtensions
    {
        /// <summary>
        /// Get the first email address for the organization from their contact methods.
        /// Note this will only return a value if organization.ContactMethods.ContactType is eager loaded into context.
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        public static string GetEmail(this PimsOrganization organization)
        {
            return organization?.PimsContactMethods.OrderBy(cm => cm.IsPreferredMethod).FirstOrDefault(cm => cm.ContactMethodTypeCode == ContactMethodTypes.WorkEmail)?.ContactMethodValue;
        }

        /// <summary>
        /// Get the first phone number for the organization from their contact methods, prioritizing numbers with preferred set.
        /// Note this will only return a value if organization.ContactMethods.ContactType is eager loaded into context.
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        public static string GetLandlinePhoneNumber(this PimsOrganization organization)
        {
            return organization?.PimsContactMethods.OrderBy(cm => cm.IsPreferredMethod).FirstOrDefault(cm => cm.ContactMethodTypeCode == ContactMethodTypes.PersPhone || cm.ContactMethodTypeCode == ContactMethodTypes.WorkPhone)?.ContactMethodValue;
        }

        /// <summary>
        /// Get the first mobile phone number for the organization from their contact methods, prioritizing numbers with preferred set.
        /// Note this will only return a value if organization.ContactMethods.ContactType is eager loaded into context.
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        public static string GetMobilePhoneNumber(this PimsOrganization organization)
        {
            return organization?.PimsContactMethods.OrderBy(cm => cm.IsPreferredMethod).FirstOrDefault(cm => cm.ContactMethodTypeCode == ContactMethodTypes.PerseMobil || cm.ContactMethodTypeCode == ContactMethodTypes.WorkMobil)?.ContactMethodValue;
        }

        /// <summary>
        /// Get the concatenated full name of this organization.
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        public static string GetFirstPersonFullName(this PimsOrganization organization)
        {
            PimsPerson person = organization.GetPersons().FirstOrDefault();
            if (person != null)
            {
                string[] names = { person.Surname, person.FirstName, person.MiddleNames };
                return string.Join(", ", names.Where(n => n != null && n.Trim() != string.Empty));
            }
            return null;
        }

        /// <summary>
        /// Get the mailing address of the organization, or null if the organization does not have a mailing address.
        /// Note this will only return a value if Person.ContactMethods.ContactType is eager loaded into context.
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        public static PimsAddress GetMailingAddress(this PimsOrganization organization)
        {
            return organization?.PimsOrganizationAddresses.FirstOrDefault(a => a?.AddressUsageTypeCode == AddressUsageTypes.Mailing)?.Address;
        }

        /// <summary>
        /// DEPRECATED, either get an address by type, or get all addresses for a person.
        /// Get a single address from this user's address list.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public static PimsAddress GetSingleAddress(this PimsOrganization organization)
        {
            return organization?.PimsOrganizationAddresses.FirstOrDefault()?.Address;
        }
    }
}
