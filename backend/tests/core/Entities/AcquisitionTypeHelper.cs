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
        /// Create a new instance of a PimsAcquisitionType.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Entity.PimsAcquisitionType CreateAcquisitionType(string id)
        {
            return new Entity.PimsAcquisitionType(id) { ConcurrencyControlNumber = 1 };
        }

        /// <summary>
        /// Creates a default list of PimsAcquisitionType.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.PimsAcquisitionType> CreateDefaultAcquisitionTypes()
        {
            return new List<Entity.PimsAcquisitionType>()
            {
                new Entity.PimsAcquisitionType("CONSEN") { ConcurrencyControlNumber = 1 },
                new Entity.PimsAcquisitionType("SECTN3") { ConcurrencyControlNumber = 1 },
                new Entity.PimsAcquisitionType("SECTN6") { ConcurrencyControlNumber = 1 },
            };
        }
    }
}
