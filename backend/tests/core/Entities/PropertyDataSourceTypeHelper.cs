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
        public static Entity.PropertyDataSourceType CreatePropertyDataSourceType(string id)
        {
            return new Entity.PropertyDataSourceType(id, "") { RowVersion = 1 };
        }

        /// <summary>
        /// Creates a default list of PropertyDataSourceType.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.PropertyDataSourceType> CreateDefaultPropertyDataSourceTypes()
        {
            return new List<Entity.PropertyDataSourceType>()
            {
                new Entity.PropertyDataSourceType("LIS", "") { RowVersion = 1 },
                new Entity.PropertyDataSourceType("PAIMS", "") { RowVersion = 1 }
            };
        }
    }
}
