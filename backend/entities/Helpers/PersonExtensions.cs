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
        public static string GetEmail(this Person person)
        {
            return person?.ContactMethods.FirstOrDefault(cm => cm.ContactMethodTypeId == ContactMethodTypes.WorkEmail)?.Value;
        }

        /// <summary>
        /// Get the concatenated full name of this person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public static string GetFullName(this Person person)
        {
            return $"{person.FirstName} {person.MiddleNames} {person.Surname}";
        }
    }
}
