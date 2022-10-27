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
        /// Create a new instance of a research file.
        /// </summary>
        /// <returns></returns>
        public static Entity.PimsResearchFile CreateResearchFile(long? researchFileId = null, string rfileNumber = null, PimsResearchFileStatusType statusType = null)
        {
            var researchFile = new Entity.PimsResearchFile()
            {
                ResearchFileId = researchFileId ?? 1,
                RfileNumber = rfileNumber ?? "100-000-000",
                ConcurrencyControlNumber = 1,
            };
            researchFile.ResearchFileStatusTypeCodeNavigation = statusType ?? new Entity.PimsResearchFileStatusType() { Id = "fileStatusType" };

            return researchFile;
        }

        /// <summary>
        /// Create a new instance of a Research File.
        /// </summary>
        /// <returns></returns>
        public static Entity.PimsResearchFile CreateResearchFile(this PimsContext context, long? researchFileId = null, string rfileNumber = null)
        {
            var statusType = context.PimsResearchFileStatusTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find research status type.");
            var researchFile = EntityHelper.CreateResearchFile(statusType: statusType, researchFileId: researchFileId, rfileNumber: rfileNumber);
            context.PimsResearchFiles.Add(researchFile);
            return researchFile;
        }
    }
}
