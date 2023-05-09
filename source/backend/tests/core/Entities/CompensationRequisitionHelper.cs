using Pims.Dal.Entities;

namespace Pims.Core.Test
{
    /// <summary>
    /// EntityHelper static class, provides helper methods to create test entities.
    /// </summary>
    public static partial class EntityHelper
    {
        /// <summary>
        /// Create a new instance of CompensationRequisition.
        /// </summary>
        /// <param name="id"> Internal Id.</param>
        /// <param name="acquisitionFileId"> Parent Acquisition File Id.</param>
        /// <param name="fiscalYear"> Fiscal Year.</param>
        /// <returns>New Compensation Requisition Instance.</returns>
        public static PimsCompensationRequisition CreateCompensationRequisition(long id = 1, long acquisitionFileId = 1, string fiscalYear = "2022/2023")
        {
            return new PimsCompensationRequisition()
            {
                Internal_Id = id,
                AcquisitionFileId = acquisitionFileId,
                FiscalYear = fiscalYear,
            };
        }
    }
}
