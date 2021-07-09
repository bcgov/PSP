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
        /// Create a new instance of a BuildingOccupantType.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Entity.BuildingOccupantType CreateBuildingOccupantType(int id, string name)
        {
            return new Entity.BuildingOccupantType(name) { Id = id, RowVersion = 1 };
        }

        /// <summary>
        /// Create a new instance of a BuildingOccupantType.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Entity.BuildingOccupantType CreateBuildingOccupantType(string name)
        {
            return new Entity.BuildingOccupantType(name) { Id = 1, RowVersion = 1 };
        }

        /// <summary>
        /// Creates a default list of BuildingOccupantType.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.BuildingOccupantType> CreateDefaultBuildingOccupantTypes()
        {
            return new List<Entity.BuildingOccupantType>()
            {
                new Entity.BuildingOccupantType("Leased") { Id = 1, RowVersion = 1 },
                new Entity.BuildingOccupantType("Occupied By Owning Ministry") { Id = 2, RowVersion = 1 },
                new Entity.BuildingOccupantType("Unoccupied") { Id = 3, RowVersion = 1 }
            };
        }
    }
}
