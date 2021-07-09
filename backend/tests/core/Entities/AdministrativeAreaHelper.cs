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
        /// Create a new instance of a AdministrativeArea.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="abbreviation"></param>
        /// <returns></returns>
        public static Entity.AdministrativeArea CreateAdministrativeArea(string name, string abbreviation)
        {
            return new Entity.AdministrativeArea(name)
            {
                Id = 1,
                Abbreviation = abbreviation,
                RowVersion = 1
            };
        }

        /// <summary>
        /// Creates a default list of AdministrativeArea.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.AdministrativeArea> CreateDefaultAdministrativeAreas()
        {
            return new List<Entity.AdministrativeArea>()
            {
                new Entity.AdministrativeArea("Abbotsford") { Id = 100, Abbreviation = "ABB", RowVersion = 1 },
                new Entity.AdministrativeArea("Agassiz") { Id = 101, Abbreviation = "AGA", RowVersion = 1 },
                new Entity.AdministrativeArea("Ahousaht") { Id = 102, Abbreviation = "AHO", RowVersion = 1 },
                new Entity.AdministrativeArea("Albert Canyon") { Id = 103, Abbreviation = "ACA", RowVersion = 1 },
                new Entity.AdministrativeArea("Vancouver") { Id = 104, Abbreviation = "VAN", RowVersion = 1 },
                new Entity.AdministrativeArea("Victoria") { Id = 105, Abbreviation = "VIC", RowVersion = 1 }
            };
        }
    }
}
