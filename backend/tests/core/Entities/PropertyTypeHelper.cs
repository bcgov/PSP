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
        /// Create a new instance of a PropertyType.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Entity.PimsPropertyType CreatePropertyType(string id)
        {
            return new Entity.PimsPropertyType(id) { ConcurrencyControlNumber = 1 };
        }

        /// <summary>
        /// Creates a default list of PropertyType.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.PimsPropertyType> CreateDefaultPropertyTypes()
        {
            return new List<Entity.PimsPropertyType>()
            {
                new Entity.PimsPropertyType("Land") { ConcurrencyControlNumber = 1 },
                new Entity.PimsPropertyType("Building") { ConcurrencyControlNumber = 1 },
            };
        }
    }
}
