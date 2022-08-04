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
        /// Create a new instance of a PimsAcquisitionFileStatusType.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Entity.PimsAcquisitionFileStatusType CreateAcquisitionFileStatusType(string id)
        {
            return new Entity.PimsAcquisitionFileStatusType(id) { ConcurrencyControlNumber = 1 };
        }

        /// <summary>
        /// Creates a default list of PimsAcquisitionFileStatusType.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.PimsAcquisitionFileStatusType> CreateDefaultAcquisitionFileStatusTypes()
        {
            return new List<Entity.PimsAcquisitionFileStatusType>()
            {
                new Entity.PimsAcquisitionFileStatusType("ACTIVE") { ConcurrencyControlNumber = 1 },
                new Entity.PimsAcquisitionFileStatusType("ARCHIV") { ConcurrencyControlNumber = 1 },
                new Entity.PimsAcquisitionFileStatusType("CLOSED") { ConcurrencyControlNumber = 1 },
            };
        }
    }
}
