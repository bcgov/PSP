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
        /// Create a new instance of a Region.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Entity.PimsRegion CreateRegion(short id, string code)
        {
            return new Entity.PimsRegion(code) { RegionCode = id, ConcurrencyControlNumber = 1 };
        }

        /// <summary>
        /// Creates a default list of Region.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.PimsRegion> CreateDefaultRegions()
        {
            return new List<Entity.PimsRegion>()
            {
                new Entity.PimsRegion("Northern") { RegionCode = 1, ConcurrencyControlNumber = 1 },
                new Entity.PimsRegion("Southern Interior") { RegionCode = 2,  ConcurrencyControlNumber = 1 },
                new Entity.PimsRegion("South Coast") { RegionCode = 3,  ConcurrencyControlNumber = 1 },
            };
        }
    }
}
