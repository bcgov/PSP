using Pims.Dal.Entities;
using System;
using Entity = Pims.Dal.Entities;

namespace Pims.Core.Test
{
    /// <summary>
    /// EntityHelper static class, provides helper methods to create test entities.
    /// </summary>
    public static partial class EntityHelper
    {
        /// <summary>
        /// Create a new instance of a Take.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Entity.PimsTake CreateTake(long id = 1)
        {
            return new Entity.PimsTake()
            {
                Internal_Id = id,
                TakeId = id,
                AppCreateTimestamp = DateTime.Now,
                AppCreateUserid = "admin",
                AppCreateUserDirectory = string.Empty,
                AppLastUpdateUserDirectory = string.Empty,
                AppLastUpdateUserid = string.Empty,
                DbCreateUserid = string.Empty,
                DbLastUpdateUserid = string.Empty,
                ConcurrencyControlNumber = 1,
                AreaUnitTypeCodeNavigation = new Entity.PimsAreaUnitType() { Id = id.ToString(), DbCreateUserid = "test", DbLastUpdateUserid = "test", AreaUnitTypeCode = "test", Description = "test" },
                LandActTypeCodeNavigation = new Entity.PimsLandActType() { Id = id.ToString(), DbCreateUserid = "test", DbLastUpdateUserid = "test", LandActTypeCode = "test", Description = "test" },
                TakeSiteContamTypeCodeNavigation = new PimsTakeSiteContamType() { Id = id.ToString(), DbCreateUserid = "test", DbLastUpdateUserid = "test", TakeSiteContamTypeCode = "test", Description = "test" },
                TakeStatusTypeCodeNavigation = new PimsTakeStatusType() { Id = id.ToString(), DbCreateUserid = "test", DbLastUpdateUserid = "test", TakeStatusTypeCode = "test", Description = "test" },
                TakeTypeCodeNavigation = new PimsTakeType() { Id = id.ToString(), DbCreateUserid = "test", DbLastUpdateUserid = "test", TakeTypeCode = "test", Description = "test" },
                PropertyAcquisitionFile = new PimsPropertyAcquisitionFile(),
            };
        }
    }
}
