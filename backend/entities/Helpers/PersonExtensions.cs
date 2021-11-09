using System;
using System.Linq;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PersonExtensions static class, provides an extensions methods for person entities.
    /// </summary>
    public static class PersonExtensions
    {
        /// <summary>
        /// Get the first email address for the person from their contact methods.
        /// Note this will only return a value if Person.ContactMethods.ContactType is eager loaded into context.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public static string GetWorkEmail(this Person person)
        {
            return person?.ContactMethods.OrderBy(cm => cm.IsPreferredMethod).FirstOrDefault(cm => cm.ContactMethodTypeId == ContactMethodTypes.WorkEmail)?.Value;
        }

        /// <summary>
        /// Get the first email address for the person from their contact methods, preferring work emails.
        /// Note this will only return a value if Person.ContactMethods.ContactType is eager loaded into context.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public static string GetEmail(this Person person)
        {
            return person?.ContactMethods.OrderBy(cm => cm.ContactMethodTypeId == "WORKEMAIL" ? 1 : 0)
                .FirstOrDefault(cm => cm.ContactMethodTypeId == ContactMethodTypes.WorkEmail || cm.ContactMethodTypeId == ContactMethodTypes.PerseEmail)?.Value;
        }

        /// <summary>
        /// Get the first phone number for the person from their contact methods, prioritizing numbers with preferred set.
        /// Note this will only return a value if Person.ContactMethods.ContactType is eager loaded into context.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public static string GetLandlinePhoneNumber(this Person person)
        {
            return person?.ContactMethods.OrderBy(cm => cm.IsPreferredMethod).FirstOrDefault(cm => cm.ContactMethodTypeId == ContactMethodTypes.PersPhone || cm.ContactMethodTypeId == ContactMethodTypes.WorkPhone)?.Value;
        }

        /// <summary>
        /// Get the first mobile phone number for the person from their contact methods, prioritizing numbers with preferred set.
        /// Note this will only return a value if Person.ContactMethods.ContactType is eager loaded into context.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public static string GetMobilePhoneNumber(this Person person)
        {
            return person?.ContactMethods.OrderBy(cm => cm.IsPreferredMethod).FirstOrDefault(cm => cm.ContactMethodTypeId == ContactMethodTypes.PerseMobil || cm.ContactMethodTypeId == ContactMethodTypes.WorkMobil)?.Value;
        }

        /// <summary>
        /// Get the concatenated full name of this person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public static string GetFullName(this Person person)
        {
            string[] names = { person.FirstName, person.MiddleNames, person.Surname };
            return String.Join(" ", names.Where(n => n != null && n.Trim() != String.Empty));
        }

        /// <summary>
        /// Get the mailing address of the person, or null if the person does not have a mailing address.
        /// Note this will only return a value if Person.ContactMethods.ContactType is eager loaded into context.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public static Address GetMailingAddress(this Person person)
        {
            return person?.Address?.AddressTypeId == "MAILADDR" ? person.Address : null;
        }
    }
}
