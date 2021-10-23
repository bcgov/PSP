using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static string GetEmail(this Organization organization)
        {
            return organization?.ContactMethods.OrderBy(cm => cm.IsPreferredMethod).FirstOrDefault(cm => cm.ContactMethodTypeId == ContactMethodTypes.WorkEmail)?.Value;
        }

        /// <summary>
        /// Get the first phone number for the organization from their contact methods, prioritizing numbers with preferred set.
        /// Note this will only return a value if organization.ContactMethods.ContactType is eager loaded into context.
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        public static string GetLandlinePhoneNumber(this Organization organization)
        {
            return organization?.ContactMethods.OrderBy(cm => cm.IsPreferredMethod).FirstOrDefault(cm => cm.ContactMethodTypeId == ContactMethodTypes.PersPhone || cm.ContactMethodTypeId == ContactMethodTypes.WorkPhone)?.Value;
        }

        /// <summary>
        /// Get the first mobile phone number for the organization from their contact methods, prioritizing numbers with preferred set.
        /// Note this will only return a value if organization.ContactMethods.ContactType is eager loaded into context.
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        public static string GetMobilePhoneNumber(this Organization organization)
        {
            return organization?.ContactMethods.OrderBy(cm => cm.IsPreferredMethod).FirstOrDefault(cm => cm.ContactMethodTypeId == ContactMethodTypes.PerseMobil || cm.ContactMethodTypeId == ContactMethodTypes.WorkMobil)?.Value;
        }

        /// <summary>
        /// Get the concatenated full name of this organization
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        public static string GetFullName(this Organization organization)
        {
            Person person = organization.Persons.FirstOrDefault();
            if (person != null)
            {
                string[] names = { person.Surname, person.FirstName, person.MiddleNames };
                return String.Join(", ", names.Where(n => n != null && n.Trim() != String.Empty));
            }
            return null;
        }
    }
}
