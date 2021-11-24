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
        public static Entity.PimsDistrict CreateDistrict(short id, string code, Entity.PimsRegion region = null)
        {
            region ??= EntityHelper.CreateRegion(id, "Region 1");
            return new Entity.PimsDistrict(code, region) { DistrictCode = id, ConcurrencyControlNumber = 1 };
        }

        /// <summary>
        /// Creates a default list of District.
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public static List<Entity.PimsDistrict> CreateDefaultDistricts(Entity.PimsRegion region = null)
        {
            region ??= EntityHelper.CreateRegion(1, "Region 1");
            return new List<Entity.PimsDistrict>()
            {
                new Entity.PimsDistrict("District 1", region) { DistrictCode = 1, ConcurrencyControlNumber = 1 },
                new Entity.PimsDistrict("District 2", region) { DistrictCode = 2,  ConcurrencyControlNumber = 1 },
            };
        }

        /// <summary>
        /// Create a new instance of a District.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public static Entity.PimsDistrict CreateDistrict(this PimsContext context, short id, string code, Entity.PimsRegion region = null)
        {
            region ??= context.PimsRegions.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find a region.");
            return new Entity.PimsDistrict(code, region) { DistrictCode = id, ConcurrencyControlNumber = 1 };
        }
    }
}
