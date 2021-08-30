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
        public static Entity.Region CreateRegion(int id, string code)
        {
            return new Entity.Region(code) { Id = id, RowVersion = 1 };
        }

        /// <summary>
        /// Creates a default list of Region.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.Region> CreateDefaultRegions()
        {
            return new List<Entity.Region>()
            {
                new Entity.Region("Northern") { Id = 1, RowVersion = 1 },
                new Entity.Region("Southern Interior") { Id = 2,  RowVersion = 1 },
                new Entity.Region("South Coast") { Id = 3,  RowVersion = 1 }
            };
        }
    }
}
