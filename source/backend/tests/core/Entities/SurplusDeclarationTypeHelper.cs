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
        /// Creates a default list of PimsAcquisitionType.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.PimsSurplusDeclarationType> CreateDefaultSurplusDeclarationTypes()
        {
            return new List<Entity.PimsSurplusDeclarationType>()
            {
                new ("UNKNOWN") { ConcurrencyControlNumber = 1, Description = "Unknown", DbCreateUserid = "test", DbLastUpdateUserid = "test" },
                new ("NO") { ConcurrencyControlNumber = 1, Description = "No", DbCreateUserid = "test", DbLastUpdateUserid = "test" },
                new ("EXPIRED") { ConcurrencyControlNumber = 1, Description = "Expired", DbCreateUserid = "test", DbLastUpdateUserid = "test" },
                new ("YES") { ConcurrencyControlNumber = 1, Description = "Yes", DbCreateUserid = "test", DbLastUpdateUserid = "test" },
            };
        }
    }
}
