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
        /// Create a new instance of a PropertyTenureType.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Entity.PropertyTenureType CreatePropertyTenureType(string id)
        {
            return new Entity.PropertyTenureType(id, "") { RowVersion = 1 };
        }

        /// <summary>
        /// Creates a default list of PropertyTenureType.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.PropertyTenureType> CreateDefaultPropertyTenureTypes()
        {
            return new List<Entity.PropertyTenureType>()
            {
                new Entity.PropertyTenureType("Land", "") { RowVersion = 1 },
                new Entity.PropertyTenureType("Building", "") { RowVersion = 1 }
            };
        }
    }
}
