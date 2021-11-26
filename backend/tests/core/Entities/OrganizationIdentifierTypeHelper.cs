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
        public static Entity.PimsOrgIdentifierType CreateOrganizationIdentifierType(string id)
        {
            return new Entity.PimsOrgIdentifierType(id) { ConcurrencyControlNumber = 1 };
        }

        /// <summary>
        /// Creates a default list of OrganizationIdentifierType.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.PimsOrgIdentifierType> CreateDefaultOrganizationIdentifierTypes()
        {
            return new List<Entity.PimsOrgIdentifierType>()
            {
                new Entity.PimsOrgIdentifierType("IdentifierType 1") { ConcurrencyControlNumber = 1 },
                new Entity.PimsOrgIdentifierType("IdentifierType 2") { ConcurrencyControlNumber = 1 },
            };
        }
    }
}
