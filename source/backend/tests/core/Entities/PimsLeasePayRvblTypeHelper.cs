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
        public static List<Entity.PimsLeasePayRvblType> CreateDefaultPimsLeasePayRvblTypes()
        {
            return new List<Entity.PimsLeasePayRvblType>()
            {
                new ("PYBLBCTFA") { ConcurrencyControlNumber = 1, Description = "Payable (BCTFA as tenant)", DbCreateUserid = "test", DbLastUpdateUserid = "test" },
                new ("PYBLMOTI") { ConcurrencyControlNumber = 1, Description = "Payable (MOTT as tenant)", DbCreateUserid = "test", DbLastUpdateUserid = "test" },
                new ("RCVBL") { ConcurrencyControlNumber = 1, Description = "Receivable", DbCreateUserid = "test", DbLastUpdateUserid = "test" },
            };
        }
    }
}
