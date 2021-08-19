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
        /// Create a new instance of a ContactMethodType.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Entity.ContactMethodType CreateContactMethodType(string id)
        {
            return new Entity.ContactMethodType(id, "") { RowVersion = 1 };
        }

        /// <summary>
        /// Creates a default list of ContactMethodType.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.ContactMethodType> CreateDefaultContactMethodTypes()
        {
            return new List<Entity.ContactMethodType>()
            {
                new Entity.ContactMethodType("Email", "") { RowVersion = 1 },
                new Entity.ContactMethodType("Phone", "") { RowVersion = 1 },
                new Entity.ContactMethodType("Mobile", "") { RowVersion = 1 },
                new Entity.ContactMethodType("Fax", "") { RowVersion = 1 }
            };
        }
    }
}
