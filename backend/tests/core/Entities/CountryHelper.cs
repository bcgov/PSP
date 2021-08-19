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
        public static Entity.Country CreateCountry(int id, string code)
        {
            return new Entity.Country(code) { Id = id, RowVersion = 1 };
        }

        /// <summary>
        /// Creates a default list of Country.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.Country> CreateDefaultCountries()
        {
            return new List<Entity.Country>()
            {
                new Entity.Country("CAN") { Id = 1, RowVersion = 1 },
                new Entity.Country("USA") { Id = 2,  RowVersion = 1 },
            };
        }
    }
}
