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
        public static Entity.PimsAreaUnitType CreatePropertyAreaUnitType(string id)
        {
            return new Entity.PimsAreaUnitType(id) { ConcurrencyControlNumber = 1 };
        }

        /// <summary>
        /// Creates a default list of PropertyAreaUnitType.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.PimsAreaUnitType> CreateDefaultPropertyAreaUnitTypes()
        {
            return new List<Entity.PimsAreaUnitType>()
            {
                new Entity.PimsAreaUnitType("Land") { ConcurrencyControlNumber = 1 },
                new Entity.PimsAreaUnitType("Building") { ConcurrencyControlNumber = 1 },
            };
        }
    }
}
