using System.Collections.Generic;
using Pims.Dal;
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
        public static Entity.OrganizationType CreateOrganizationType(string id)
        {
            return new Entity.OrganizationType(id, "") { RowVersion = 1 };
        }

        /// <summary>
        /// Creates a default list of OrganizationType.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.OrganizationType> CreateDefaultOrganizationTypes()
        {
            return new List<Entity.OrganizationType>()
            {
                new Entity.OrganizationType("Type 1", "") { RowVersion = 1 },
                new Entity.OrganizationType("Type 2", "") { RowVersion = 1 },
            };
        }
    }
}
