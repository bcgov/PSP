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
        public static Entity.PropertyType CreatePropertyType(string id)
        {
            return new Entity.PropertyType(id, "") { RowVersion = 1 };
        }

        /// <summary>
        /// Creates a default list of PropertyType.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.PropertyType> CreateDefaultPropertyTypes()
        {
            return new List<Entity.PropertyType>()
            {
                new Entity.PropertyType("Land", "") { RowVersion = 1 },
                new Entity.PropertyType("Building", "") { RowVersion = 1 }
            };
        }
    }
}
