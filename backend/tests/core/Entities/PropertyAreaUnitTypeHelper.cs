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
        /// Create a new instance of a PropertyAreaUnitType.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Entity.PropertyAreaUnitType CreatePropertyAreaUnitType(string id)
        {
            return new Entity.PropertyAreaUnitType(id, "") { RowVersion = 1 };
        }

        /// <summary>
        /// Creates a default list of PropertyAreaUnitType.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.PropertyAreaUnitType> CreateDefaultPropertyAreaUnitTypes()
        {
            return new List<Entity.PropertyAreaUnitType>()
            {
                new Entity.PropertyAreaUnitType("Land", "") { RowVersion = 1 },
                new Entity.PropertyAreaUnitType("Building", "") { RowVersion = 1 }
            };
        }
    }
}
