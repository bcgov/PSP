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
        /// Create a new instance of a PropertyDataSourceType.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Entity.PimsPropertyDataSourceType CreatePropertyDataSourceType(string id)
        {
            return new Entity.PimsPropertyDataSourceType(id) { ConcurrencyControlNumber = 1 };
        }

        /// <summary>
        /// Creates a default list of PropertyDataSourceType.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.PimsPropertyDataSourceType> CreateDefaultPropertyDataSourceTypes()
        {
            return new List<Entity.PimsPropertyDataSourceType>()
            {
                new Entity.PimsPropertyDataSourceType("LIS") { ConcurrencyControlNumber = 1 },
                new Entity.PimsPropertyDataSourceType("PAIMS") { ConcurrencyControlNumber = 1 }
            };
        }
    }
}
