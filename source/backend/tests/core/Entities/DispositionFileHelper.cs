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
            dispositionFile.DispositionFileStatusTypeCodeNavigation = statusType ?? new Entity.PimsDispositionFileStatusType() { Id = "ACTIVE", Description = "Active" };
            dispositionFile.DispositionTypeCodeNavigation = dispositionType ?? new Entity.PimsDispositionType() { Id = "SECTN3" };
            dispositionFile.RegionCodeNavigation = region ?? new Entity.PimsRegion("Northern") { RegionCode = 1, ConcurrencyControlNumber = 1 };
            dispositionFile.RegionCode = dispositionFile.RegionCodeNavigation.RegionCode;
            dispositionFile.DspInitiatingBranchTypeCodeNavigation = new PimsDspInitiatingBranchType() { Id = "Northern" };
            dispositionFile.DspPhysFileStatusTypeCodeNavigation = new PimsDspPhysFileStatusType() { Id = "Terminated" };
            dispositionFile.DispositionFundingTypeCodeNavigation = new PimsDispositionFundingType() { Id = "Funded" };
            dispositionFile.DispositionInitiatingDocTypeCodeNavigation = new PimsDispositionInitiatingDocType() { Id = "Doc" };
            dispositionFile.DispositionStatusTypeCodeNavigation = new PimsDispositionStatusType() { Id = "DRAFT" };

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
