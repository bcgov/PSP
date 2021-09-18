using System;
using System.Linq;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
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
        /// Create a new instance of a Property.
        /// </summary>
        /// <param name="pidOrPin"></param>
        /// <param name="lFileNo"></param>
        /// <param name="tenantName"></param>
        /// <returns></returns>
        public static Entity.Lease CreateLease(int pidOrPin, string lFileNo = null, string tenantFirstName = null, string tenantLastName = null)
        {
            var lease = new Entity.Lease()
            {
                Id = 1,
                LFileNo = lFileNo,
                ExpiryDate = new DateTime(2021, 1, 1),
                RowVersion = 1,
            };
            Entity.Address address = new() { Municipality = "municipality", StreetAddress1="address" };
            lease.Properties.Add(new Entity.Property() { PID = pidOrPin, Address = address });
            lease.Tenant = new Entity.Person() { FirstName = $"{tenantLastName}, {tenantFirstName}" };
            return lease;
        }
    }
}
