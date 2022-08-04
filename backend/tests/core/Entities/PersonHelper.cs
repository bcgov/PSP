using Pims.Dal;
using Entity = Pims.Dal.Entities;

namespace Pims.Core.Test
{
    /// <summary>
    /// EntityHelper static class, provides helper methods to create test entities.
    /// </summary>
    public static partial class EntityHelper
    {
        /// <summary>
        /// Create a new instance of an Person.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="surname"></param>
        /// <param name="firstName"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static Entity.PimsPerson CreatePerson(long id, string surname, string firstName, Entity.PimsAddress address = null)
        {
            return new Entity.PimsPerson(surname, firstName, address ?? EntityHelper.CreateAddress(id))
            {
                PersonId = id,
                AppCreateUserDirectory = string.Empty,
                AppCreateUserid = string.Empty,
                AppLastUpdateUserDirectory = string.Empty,
                AppLastUpdateUserid = string.Empty,
                DbCreateUserid = string.Empty,
                DbLastUpdateUserid = string.Empty,
                IsDisabled = false,
                ConcurrencyControlNumber = 1,
            };
        }

        /// <summary>
        /// Create a new instance of an Person.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="surname"></param>
        /// <param name="firstName"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static Entity.PimsPerson CreatePerson(this PimsContext context, long id, string surname, string firstName, Entity.PimsAddress address = null)
        {
            address ??= EntityHelper.CreateAddress(context, id, "1234 St");
            return new Entity.PimsPerson(surname, firstName, address)
            {
                PersonId = id,
                ConcurrencyControlNumber = 1,
                AppCreateUserDirectory = string.Empty,
                AppCreateUserid = string.Empty,
                AppLastUpdateUserDirectory = string.Empty,
                AppLastUpdateUserid = string.Empty,
                DbCreateUserid = string.Empty,
                DbLastUpdateUserid = string.Empty,
                IsDisabled = false,
            };
        }
    }
}
