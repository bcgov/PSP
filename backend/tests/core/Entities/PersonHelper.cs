using System;
using System.Linq;
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
        public static Entity.Person CreatePerson(long id, string surname, string firstName, Entity.Address address = null)
        {
            return new Entity.Person(surname, firstName, address ?? EntityHelper.CreateAddress(id))
            {
                Id = id,
                RowVersion = 1
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
        public static Entity.Person CreatePerson(this PimsContext context, long id, string surname, string firstName, Entity.Address address = null)
        {
            address ??= EntityHelper.CreateAddress(context, id, "1234 St");
            return new Entity.Person(surname, firstName, address)
            {
                Id = id,
                RowVersion = 1
            };
        }
    }
}

