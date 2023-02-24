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
        /// Create a new instance of an Acquisition file.
        /// </summary>
        /// <returns></returns>
        public static Entity.PimsAcquisitionFile CreateAcquisitionFile(long? acqFileId = null, string name = null, PimsAcquisitionFileStatusType statusType = null, PimsAcquisitionType acquisitionType = null, PimsRegion region = null)
        {
            var acquisitionFile = new Entity.PimsAcquisitionFile()
            {
                AcquisitionFileId = acqFileId ?? 1,
                FileName = name ?? "Test Acquisition File",
                ConcurrencyControlNumber = 1,
            };
            acquisitionFile.AcquisitionFileStatusTypeCode = "ACTIVE";
            acquisitionFile.AcquisitionFileStatusTypeCodeNavigation = statusType ?? new Entity.PimsAcquisitionFileStatusType() { Id = "ACTIVE", Description = "Active" };
            acquisitionFile.AcquisitionTypeCodeNavigation = acquisitionType ?? new Entity.PimsAcquisitionType() { Id = "SECTN3" };
            acquisitionFile.RegionCodeNavigation = region ?? new Entity.PimsRegion("Northern") { RegionCode = 1, ConcurrencyControlNumber = 1 };
            acquisitionFile.RegionCode = acquisitionFile.RegionCodeNavigation.RegionCode;

            return acquisitionFile;
        }

        /// <summary>
        /// Create a new instance of an Acquisition File.
        /// </summary>
        /// <returns></returns>
        public static Entity.PimsAcquisitionFile CreateAcquisitionFile(this PimsContext context, long? acqFileId = null, string name = null)
        {
            var statusType = context.PimsAcquisitionFileStatusTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find acquisition file status type.");
            var acquisitionType = context.PimsAcquisitionTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find acquisition type.");
            var region = context.PimsRegions.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find region.");
            var acquisitionFile = EntityHelper.CreateAcquisitionFile(acqFileId: acqFileId, name: name, statusType: statusType, acquisitionType: acquisitionType, region: region);
            context.PimsAcquisitionFiles.Add(acquisitionFile);

            return acquisitionFile;
        }
    }
}
