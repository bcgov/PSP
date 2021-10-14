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
        public static Entity.Lease CreateLease(int pidOrPin, string lFileNo = null, string tenantFirstName = null, string tenantLastName = null, string motiFirstName = null, string motiLastName = null)
        {
            var lease = new Entity.Lease()
            {
                Id = 1,
                LFileNo = lFileNo,
                RowVersion = 1,
            };
            lease.Properties.Add(new Entity.Property() { PID = pidOrPin });
            lease.Persons.Add(new Entity.Person() { FirstName = tenantFirstName, Surname = tenantLastName });
            lease.MotiName = new Entity.Person() { FirstName = motiFirstName, Surname = motiLastName };
            lease.ProgramType = new Entity.LeaseProgramType() { Id = "testProgramType" };
            lease.PaymentFrequencyType = new Entity.LeasePaymentFrequencyType() { Id = "testFrequencyType" };
            return lease;
        }
    }
}
