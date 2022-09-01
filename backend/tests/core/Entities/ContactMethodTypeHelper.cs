using System.Collections.Generic;
using Entity = Pims.Dal.Entities;

namespace Pims.Core.Test
{
    /// <summary>
    /// EntityHelper static class, provides helper methods to create test entities.
    /// </summary>
    public static partial class EntityHelper
    {
        /// <summary>
        /// Create a new instance of a ContactMethodType.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Entity.PimsContactMethodType CreateContactMethodType(string id)
        {
            return new Entity.PimsContactMethodType(id) { ConcurrencyControlNumber = 1 };
        }

        /// <summary>
        /// Creates a default list of ContactMethodType.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.PimsContactMethodType> CreateDefaultContactMethodTypes()
        {
            return new List<Entity.PimsContactMethodType>()
            {
                new Entity.PimsContactMethodType("Email") { ConcurrencyControlNumber = 1 },
                new Entity.PimsContactMethodType("Phone") { ConcurrencyControlNumber = 1 },
                new Entity.PimsContactMethodType("Mobile") { ConcurrencyControlNumber = 1 },
                new Entity.PimsContactMethodType("Fax") { ConcurrencyControlNumber = 1 },
            };
        }
    }
}
