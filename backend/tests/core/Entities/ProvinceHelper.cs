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
        /// Create a new instance of a Province.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Entity.Province CreateProvince(long id, string code, string name)
        {
            return new Entity.Province(code, name) { Id = id, RowVersion = 1 };
        }

        /// <summary>
        /// Creates a default list of Province.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.Province> CreateDefaultProvinces()
        {
            return new List<Entity.Province>()
            {
                new Entity.Province("ON", "Ontario") { Id = 1, RowVersion = 1 },
                new Entity.Province("BC", "British Columbia") { Id = 2,  RowVersion = 1 },
            };
        }
    }
}
