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
        /// Create a new instance of a PropertyClassificationType.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Entity.PropertyClassificationType CreatePropertyClassificationType(string id)
        {
            return new Entity.PropertyClassificationType(id, "") { RowVersion = 1 };
        }

        /// <summary>
        /// Creates a default list of PropertyClassificationType.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.PropertyClassificationType> CreateDefaultPropertyClassificationTypes()
        {
            return new List<Entity.PropertyClassificationType>()
            {
                new Entity.PropertyClassificationType("Core Operational", "") { RowVersion = 1 },
                new Entity.PropertyClassificationType("Core Strategic", "") { RowVersion = 1 },
                new Entity.PropertyClassificationType("Surplus Active", "") { RowVersion = 1 },
                new Entity.PropertyClassificationType("Surplus Encumbered", "") { RowVersion = 1 },
                new Entity.PropertyClassificationType("Disposed", "") { RowVersion = 1 },
                new Entity.PropertyClassificationType("Demolished", "") { RowVersion = 1 },
                new Entity.PropertyClassificationType("Subdivided", "") { RowVersion = 1 }
            };
        }
    }
}
