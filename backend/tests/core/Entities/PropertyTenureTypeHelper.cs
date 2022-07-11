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
        public static Entity.PimsPropertyTenureType CreatePropertyTenureType(string id)
        {
            return new Entity.PimsPropertyTenureType(id) { ConcurrencyControlNumber = 1 };
        }

        /// <summary>
        /// Creates a default list of PropertyTenureType.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.PimsPropertyTenureType> CreateDefaultPropertyTenureTypes()
        {
            return new List<Entity.PimsPropertyTenureType>()
            {
                new Entity.PimsPropertyTenureType("HWYROAD") { ConcurrencyControlNumber = 1 },
                new Entity.PimsPropertyTenureType("ADJLAND") { ConcurrencyControlNumber = 1 },
            };
        }
    }
}
