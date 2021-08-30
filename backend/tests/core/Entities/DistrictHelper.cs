using System;
using System.Collections.Generic;
using System.Linq;
using Pims.Dal;
using Entity = Pims.Dal.Entities;

namespace Pims.Core.Test
{
    /// <summary>
    /// EntityHelper static class, provides helper methods to create test entities.
    /// </summary>
    public static partial class EntityHelper
    {
        /// <summary>
        /// Create a new instance of a District.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public static Entity.District CreateDistrict(int id, string code, Entity.Region region = null)
        {
            region ??= EntityHelper.CreateRegion(id, "Region 1");
            return new Entity.District(code, region) { Id = id, RowVersion = 1 };
        }

        /// <summary>
        /// Creates a default list of District.
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public static List<Entity.District> CreateDefaultDistricts(Entity.Region region = null)
        {
            region ??= EntityHelper.CreateRegion(1, "Region 1");
            return new List<Entity.District>()
            {
                new Entity.District("District 1", region) { Id = 1, RowVersion = 1 },
                new Entity.District("District 2", region) { Id = 2,  RowVersion = 1 },
            };
        }

        /// <summary>
        /// Create a new instance of a District.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public static Entity.District CreateDistrict(this PimsContext context, int id, string code, Entity.Region region = null)
        {
            region ??= context.Regions.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find a region.");
            return new Entity.District(code, region) { Id = id, RowVersion = 1 };
        }
    }
}
