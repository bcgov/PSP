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
        /// Create a new instance of a Province.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        public static Entity.Province CreateProvince(int id, string code, Entity.Country country = null)
        {
            country ??= EntityHelper.CreateCountry(1, "CAN");
            return new Entity.Province(code, country) { Id = id, RowVersion = 1 };
        }

        /// <summary>
        /// Creates a default list of Province.
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        public static List<Entity.Province> CreateDefaultProvinces(Entity.Country country = null)
        {
            country ??= EntityHelper.CreateCountry(1, "CAN");
            return new List<Entity.Province>()
            {
                new Entity.Province("ON", country) { Id = 1, RowVersion = 1 },
                new Entity.Province("BC", country) { Id = 2,  RowVersion = 1 },
            };
        }

        /// <summary>
        /// Create a new instance of a Province.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        public static Entity.Province CreateProvince(this PimsContext context, int id, string code, Entity.Country country = null)
        {
            country ??= context.Countries.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find a country.");
            return new Entity.Province(code, country) { Id = id, RowVersion = 1 };
        }

    }
}
