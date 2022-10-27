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
        public static Entity.PimsPropertyClassificationType CreatePropertyClassificationType(string id)
        {
            return new Entity.PimsPropertyClassificationType(id) { ConcurrencyControlNumber = 1 };
        }

        /// <summary>
        /// Creates a default list of PropertyClassificationType.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.PimsPropertyClassificationType> CreateDefaultPropertyClassificationTypes()
        {
            return new List<Entity.PimsPropertyClassificationType>()
            {
                new Entity.PimsPropertyClassificationType("Core Operational") { ConcurrencyControlNumber = 1 },
                new Entity.PimsPropertyClassificationType("Core Strategic") { ConcurrencyControlNumber = 1 },
                new Entity.PimsPropertyClassificationType("Surplus Active") { ConcurrencyControlNumber = 1 },
                new Entity.PimsPropertyClassificationType("Surplus Encumbered") { ConcurrencyControlNumber = 1 },
                new Entity.PimsPropertyClassificationType("Disposed") { ConcurrencyControlNumber = 1 },
                new Entity.PimsPropertyClassificationType("Demolished") { ConcurrencyControlNumber = 1 },
                new Entity.PimsPropertyClassificationType("Subdivided") { ConcurrencyControlNumber = 1 },
            };
        }
    }
}
