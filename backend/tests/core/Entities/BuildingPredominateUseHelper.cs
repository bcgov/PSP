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
        /// Create a new instance of a BuildingPredominateUse.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Entity.BuildingPredominateUse CreateBuildingPredominateUse(int id, string name)
        {
            return new Entity.BuildingPredominateUse(name) { Id = id, RowVersion = 1 };
        }

        /// <summary>
        /// Create a new instance of a BuildingPredominateUse.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Entity.BuildingPredominateUse CreateBuildingPredominateUse(string name)
        {
            return new Entity.BuildingPredominateUse(name) { Id = 1, RowVersion = 1 };
        }

        /// <summary>
        /// Creates a default list of BuildingPredominateUse.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.BuildingPredominateUse> CreateDefaultBuildingPredominateUses()
        {
            return new List<Entity.BuildingPredominateUse>()
            {
                new Entity.BuildingPredominateUse("Religious") { Id = 0, RowVersion = 1 },
                new Entity.BuildingPredominateUse("Research & Development Facility") { Id = 1, RowVersion = 1 },
                new Entity.BuildingPredominateUse("Residential Detached") { Id = 2, RowVersion = 1 },
                new Entity.BuildingPredominateUse("Residential Multi") { Id = 3, RowVersion = 1 },
                new Entity.BuildingPredominateUse("Retail") { Id = 4, RowVersion = 1 },
                new Entity.BuildingPredominateUse("Senior Housing (Assisted Living / Skilled Nursing)") { Id = 5, RowVersion = 1 },
                new Entity.BuildingPredominateUse("Shelters / Orphanages / Children’s Homes / Halfway Homes") { Id = 6, RowVersion = 1 },
                new Entity.BuildingPredominateUse("Social Assistance Housing") { Id = 7, RowVersion = 1 },
                new Entity.BuildingPredominateUse("Storage") { Id = 8, RowVersion = 1 },
                new Entity.BuildingPredominateUse("Storage Vehicle") { Id = 9, RowVersion = 1 },
                new Entity.BuildingPredominateUse("Trailer Office") { Id = 10, RowVersion = 1 },
                new Entity.BuildingPredominateUse("Trailer Other") { Id = 11, RowVersion = 1 },
                new Entity.BuildingPredominateUse("Training Center") { Id = 12, RowVersion = 1 },
                new Entity.BuildingPredominateUse("Transportation (Airport / Rail / Bus station)") { Id = 13, RowVersion = 1 },
                new Entity.BuildingPredominateUse("University / Collect") { Id = 14, RowVersion = 1 },
                new Entity.BuildingPredominateUse("Warehouse") { Id = 15, RowVersion = 1 },
                new Entity.BuildingPredominateUse("Weigh Station") { Id = 16, RowVersion = 1 },
                new Entity.BuildingPredominateUse("Marina") { Id = 17, RowVersion = 1 }
            };
        }
    }
}

