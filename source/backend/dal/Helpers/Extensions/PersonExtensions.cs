using System.Linq;
using Pims.Dal.Entities;

namespace Pims.Dal.Helpers.Extensions
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
        public static string GetWorkEmail(this PimsPerson person)
        {
            return person?.PimsContactMethods?.OrderByDescending(cm => cm.IsPreferredMethod).FirstOrDefault(cm => cm.ContactMethodTypeCode == ContactMethodTypes.WorkEmail)?.ContactMethodValue;
        }

        /// <summary>
        /// Get the first email address for the person from their contact methods, preferring work emails.
        /// Note this will only return a value if Person.ContactMethods.ContactType is eager loaded into context.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public static string GetEmail(this PimsPerson person)
        {
            return person?.PimsContactMethods.OrderBy(cm => cm.ContactMethodTypeCode == "WORKEMAIL" ? 0 : 1).ThenByDescending(cm => cm.IsPreferredMethod)
                .FirstOrDefault(cm => cm.ContactMethodTypeCode == ContactMethodTypes.WorkEmail || cm.ContactMethodTypeCode == ContactMethodTypes.PerseEmail)?.ContactMethodValue;
        }

        /// <summary>
        /// Get the concatenated full name of this person.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public static string GetFullName(this PimsPerson person, bool addSuffix = false)
        {
            if (person == null)
            {
                return string.Empty;
            }

            string[] names = { person.FirstName, person.MiddleNames, person.Surname };

            if (addSuffix && !string.IsNullOrEmpty(person.NameSuffix))
            {
                names = names.Append(person.NameSuffix).ToArray();
            }

            return string.Join(" ", names.Where(n => n != null && n.Trim() != string.Empty));
        }
    }
}
