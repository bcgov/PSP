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
        /// Create a new instance of a OrganizationIdentifierType.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Entity.OrganizationIdentifierType CreateOrganizationIdentifierType(string id)
        {
            return new Entity.OrganizationIdentifierType(id, "") { RowVersion = 1 };
        }

        /// <summary>
        /// Creates a default list of OrganizationIdentifierType.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.OrganizationIdentifierType> CreateDefaultOrganizationIdentifierTypes()
        {
            return new List<Entity.OrganizationIdentifierType>()
            {
                new Entity.OrganizationIdentifierType("IdentifierType 1", "") { RowVersion = 1 },
                new Entity.OrganizationIdentifierType("IdentifierType 2", "") { RowVersion = 1 },
            };
        }
    }
}
