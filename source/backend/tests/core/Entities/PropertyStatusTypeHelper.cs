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
        /// Create a new instance of a PimsPropertyStatusType.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Entity.PimsPropertyStatusType CreatePropertyStatusType(string id)
        {
            return new Entity.PimsPropertyStatusType(id) { ConcurrencyControlNumber = 1 };
        }

        /// <summary>
        /// Creates a default list of PimsPropertyStatusType.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.PimsPropertyStatusType> CreateDefaultPropertyStatusTypes()
        {
            return new List<Entity.PimsPropertyStatusType>()
            {
                new Entity.PimsPropertyStatusType("FEESIMP") { ConcurrencyControlNumber = 1 },
                new Entity.PimsPropertyStatusType("CROWNLND") { ConcurrencyControlNumber = 1 },
            };
        }
    }
}
