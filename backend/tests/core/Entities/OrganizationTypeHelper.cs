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
        /// Create a new instance of a OrganizationType.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Entity.PimsOrganizationType CreateOrganizationType(string id)
        {
            return new Entity.PimsOrganizationType(id) { ConcurrencyControlNumber = 1 };
        }

        /// <summary>
        /// Creates a default list of OrganizationType.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.PimsOrganizationType> CreateDefaultOrganizationTypes()
        {
            return new List<Entity.PimsOrganizationType>()
            {
                new Entity.PimsOrganizationType("Type 1") { ConcurrencyControlNumber = 1 },
                new Entity.PimsOrganizationType("Type 2") { ConcurrencyControlNumber = 1 },
            };
        }
    }
}
