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
        /// Create a new instance of a Country.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Entity.PimsCountry CreateCountry(short id, string code)
        {
            return new Entity.PimsCountry(code) { CountryId = id, ConcurrencyControlNumber = 1 };
        }

        /// <summary>
        /// Creates a default list of Country.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.PimsCountry> CreateDefaultCountries()
        {
            return new List<Entity.PimsCountry>()
            {
                new Entity.PimsCountry("CAN") { CountryId = 1, ConcurrencyControlNumber = 1 },
                new Entity.PimsCountry("USA") { CountryId = 2,  ConcurrencyControlNumber = 1 },
            };
        }
    }
}
