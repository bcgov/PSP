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
        /// Create a new instance of a BuildingConstructionType.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Entity.BuildingConstructionType CreateBuildingConstructionType(int id, string name)
        {
            return new Entity.BuildingConstructionType(name) { Id = id, RowVersion = 1 };
        }

        /// <summary>
        /// Create a new instance of a BuildingConstructionType.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Entity.BuildingConstructionType CreateBuildingConstructionType(string name)
        {
            return new Entity.BuildingConstructionType(name) { Id = 1, RowVersion = 1 };
        }

        /// <summary>
        /// Creates a default list of BuildingConstructionType.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.BuildingConstructionType> CreateDefaultBuildingConstructionTypes()
        {
            return new List<Entity.BuildingConstructionType>()
            {
                new Entity.BuildingConstructionType("Concrete") { Id = 0, RowVersion = 1 },
                new Entity.BuildingConstructionType("Masonry") { Id = 1, RowVersion = 1 },
                new Entity.BuildingConstructionType("Mixed") { Id = 2, RowVersion = 1 },
                new Entity.BuildingConstructionType("Steel") { Id = 3, RowVersion = 1 },
                new Entity.BuildingConstructionType("Wood") { Id = 4, RowVersion = 1 }
            };
        }
    }
}
