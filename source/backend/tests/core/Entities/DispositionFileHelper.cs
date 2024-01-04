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
        /// Create a new instance of an Disposition file.
        /// </summary>
        /// <returns></returns>
        public static Entity.PimsDispositionFile CreateDispositionFile(long? dispFileId = null, string name = null, PimsDispositionFileStatusType statusType = null, PimsDispositionType dispositionType = null, PimsRegion region = null)
        {
            var dispositionFile = new Entity.PimsDispositionFile()
            {
                DispositionFileId = dispFileId ?? 1,
                FileName = name ?? "Test Disposition File",
                ConcurrencyControlNumber = 1,
            };
            dispositionFile.DispositionFileStatusTypeCode = "ACTIVE";
            dispositionFile.DispositionFileStatusTypeCodeNavigation = statusType ?? new Entity.PimsDispositionFileStatusType() { Id = "ACTIVE", Description = "Active", DbCreateUserid = "create user", DbLastUpdateUserid = "last user" };
            dispositionFile.DispositionTypeCodeNavigation = dispositionType ?? new Entity.PimsDispositionType() { Id = "SECTN3", Description = "Section 3", DbCreateUserid = "create user", DbLastUpdateUserid = "last user" };
            dispositionFile.RegionCodeNavigation = region ?? new Entity.PimsRegion("Northern") { RegionCode = 1, ConcurrencyControlNumber = 1, Description = "Northern", DbCreateUserid = "create user", DbLastUpdateUserid = "last user" };
            dispositionFile.RegionCode = dispositionFile.RegionCodeNavigation.RegionCode;
            dispositionFile.DspInitiatingBranchTypeCodeNavigation = new PimsDspInitiatingBranchType() { Id = "Northern", DbCreateUserid = "create user", DbLastUpdateUserid = "last user", Description = "description" };
            dispositionFile.DspPhysFileStatusTypeCodeNavigation = new PimsDspPhysFileStatusType() { Id = "Terminated", DbCreateUserid = "create user", DbLastUpdateUserid = "last user", Description = "description" };
            dispositionFile.DispositionFundingTypeCodeNavigation = new PimsDispositionFundingType() { Id = "Funded", DbCreateUserid = "create user", DbLastUpdateUserid = "last user", Description = "description" };
            dispositionFile.DispositionInitiatingDocTypeCodeNavigation = new PimsDispositionInitiatingDocType() { Id = "Doc", DbCreateUserid = "create user", DbLastUpdateUserid = "last user", Description = "description" };
            dispositionFile.DispositionStatusTypeCodeNavigation = new PimsDispositionStatusType() { Id = "DRAFT", DbCreateUserid = "create user", DbLastUpdateUserid = "last user", Description = "description" };

            return dispositionFile;
        }

        /// <summary>
        /// Create a new instance of an Disposition File.
        /// </summary>
        /// <returns></returns>
        public static Entity.PimsDispositionFile CreateDispositionFile(this PimsContext context, long? dispFileId = null, string name = null)
        {
            var statusType = context.PimsDispositionFileStatusTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find Disposition file status type.");
            var dispositionType = context.PimsDispositionTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find Disposition type.");
            var region = context.PimsRegions.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find region.");
            var dispositionFile = EntityHelper.CreateDispositionFile(dispFileId: dispFileId, name: name, statusType: statusType, dispositionType: dispositionType, region: region);
            context.PimsDispositionFiles.Add(dispositionFile);

            return dispositionFile;
        }
    }
}
