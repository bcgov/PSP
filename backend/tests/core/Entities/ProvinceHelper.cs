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
        public static Entity.PimsProvinceState CreateProvince(short id, string code, Entity.PimsCountry country = null)
        {
            country ??= EntityHelper.CreateCountry(1, "CAN");
            return new Entity.PimsProvinceState(code, country) { ProvinceStateId = id, ConcurrencyControlNumber = 1 };
        }

        /// <summary>
        /// Creates a default list of Province.
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        public static List<Entity.PimsProvinceState> CreateDefaultProvinces(Entity.PimsCountry country = null)
        {
            country ??= EntityHelper.CreateCountry(1, "CAN");
            return new List<Entity.PimsProvinceState>()
            {
                new Entity.PimsProvinceState("ON", country) { ProvinceStateId = 1, ConcurrencyControlNumber = 1 },
                new Entity.PimsProvinceState("BC", country) { ProvinceStateId = 2,  ConcurrencyControlNumber = 1 },
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
        public static Entity.PimsProvinceState CreateProvince(this PimsContext context, short id, string code, Entity.PimsCountry country = null)
        {
            country ??= context.PimsCountries.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find a country.");
            return new Entity.PimsProvinceState(code, country) { ProvinceStateId = id, ConcurrencyControlNumber = 1 };
        }
    }
}
