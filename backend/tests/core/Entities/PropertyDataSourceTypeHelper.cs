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
        /// Create a new instance of a DataSourceType.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Entity.PimsDataSourceType CreateDataSourceType(string id)
        {
            return new Entity.PimsDataSourceType(id) { ConcurrencyControlNumber = 1 };
        }

        /// <summary>
        /// Creates a default list of DataSourceType.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.PimsDataSourceType> CreateDefaultDataSourceTypes()
        {
            return new List<Entity.PimsDataSourceType>()
            {
                new Entity.PimsDataSourceType("LIS") { ConcurrencyControlNumber = 1 },
                new Entity.PimsDataSourceType("PAIMS") { ConcurrencyControlNumber = 1 },
            };
        }
    }
}
