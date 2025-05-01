using System;
using System.Linq;
using Pims.Dal;
using Pims.Dal.Entities;
using Entity = Pims.Dal.Entities;

namespace Pims.Core.Test
{
    /// <summary>
    /// EntityHelper static class, provides helper methods to create test entities.
    /// </summary>
    public static partial class EntityHelper
    {
        /// <summary>
        /// Create a new instance of an Management file.
        /// </summary>
        /// <returns></returns>
        public static Entity.PimsManagementFile CreateManagementFile(long? managementFileId = null, string name = null, PimsManagementFileStatusType statusType = null)
        {
            var managementFile = new Entity.PimsManagementFile()
            {
                ManagementFileId = managementFileId ?? 1,
                FileName = name ?? "Test Management File",
                ConcurrencyControlNumber = 1,
            };
            managementFile.ManagementFileStatusTypeCode = "ACTIVE";
            managementFile.ManagementFileStatusTypeCodeNavigation = statusType ?? new Entity.PimsManagementFileStatusType() { Id = "ACTIVE", Description = "Active", DbCreateUserid = "create user", DbLastUpdateUserid = "last user" };
            managementFile.ManagementFileProgramTypeCodeNavigation = new PimsManagementFileProgramType() { Id = "Program", DbCreateUserid = "create user", DbLastUpdateUserid = "last user", Description = "description" };
            managementFile.AcquisitionFundingTypeCodeNavigation = new PimsAcquisitionFundingType() { Id = "Funded", DbCreateUserid = "create user", DbLastUpdateUserid = "last user", Description = "description" };

            return managementFile;
        }

        /// <summary>
        /// Create a new instance of an Management File.
        /// </summary>
        /// <returns></returns>
        public static Entity.PimsManagementFile CreateManagementFile(this PimsContext context, long? managementFileId = null, string name = null)
        {
            var statusType = context.PimsManagementFileStatusTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find Management file status type.");
            var managementFile = EntityHelper.CreateManagementFile(managementFileId: managementFileId, name: name, statusType: statusType);
            context.PimsManagementFiles.Add(managementFile);

            return managementFile;
        }
    }
}
