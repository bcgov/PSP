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
        /// Create a new instance of a TierLevel.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Entity.TierLevel CreateTierLevel(int id, string name)
        {
            return new Entity.TierLevel(name) { Id = id, RowVersion = 1 };
        }

        /// <summary>
        /// Create a new instance of a TierLevel.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Entity.TierLevel CreateTierLevel(string name)
        {
            return new Entity.TierLevel(name) { Id = 1, RowVersion = 1 };
        }

        /// <summary>
        /// Creates a default list of TierLevel.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.TierLevel> CreateDefaultTierLevels()
        {
            return new List<Entity.TierLevel>()
            {
                new Entity.TierLevel("Tier 1") { Id = 1, RowVersion = 1 },
                new Entity.TierLevel("Tier 2") { Id = 2, RowVersion = 1 },
                new Entity.TierLevel("Tier 3") { Id = 3, RowVersion = 1 },
                new Entity.TierLevel("Tier 4") { Id = 4, RowVersion = 1 },
            };
        }
    }
}
