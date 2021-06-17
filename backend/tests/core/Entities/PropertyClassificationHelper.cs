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
        /// Create a new instance of a PropertyClassification.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="isVisible"></param>
        /// <returns></returns>
        public static Entity.PropertyClassification CreatePropertyClassification(int id, string name, bool isVisible = true)
        {
            return new Entity.PropertyClassification(name, isVisible) { Id = id, RowVersion = 1 };
        }

        /// <summary>
        /// Create a new instance of a PropertyClassification.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isVisible"></param>
        /// <returns></returns>
        public static Entity.PropertyClassification CreatePropertyClassification(string name, bool isVisible = true)
        {
            return new Entity.PropertyClassification(name, isVisible) { Id = 1, RowVersion = 1 };
        }

        /// <summary>
        /// Creates a default list of PropertyClassification.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.PropertyClassification> CreateDefaultPropertyClassifications()
        {
            return new List<Entity.PropertyClassification>()
            {
                new Entity.PropertyClassification("Core Operational") { Id = 0, RowVersion = 1 },
                new Entity.PropertyClassification("Core Strategic") { Id = 1, RowVersion = 1 },
                new Entity.PropertyClassification("Surplus Active") { Id = 2, RowVersion = 1 },
                new Entity.PropertyClassification("Surplus Encumbered") { Id = 3, RowVersion = 1 },
                new Entity.PropertyClassification("Disposed", false) { Id = 4, RowVersion = 1 },
                new Entity.PropertyClassification("Demolished", false) { Id = 5, RowVersion = 1 },
                new Entity.PropertyClassification("Subdivided", false) { Id = 6, RowVersion = 1 }
            };
        }
    }
}
