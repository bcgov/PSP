using Pims.Dal.Entities;

namespace Pims.Core.Test
{
    /// <summary>
    /// EntityHelper static class, provides helper methods to create test entities.
    /// </summary>
    public static partial class EntityHelper
    {
        /// <summary>
        /// Createa a new instance of Form8.
        /// </summary>
        /// <param name="id">Internal Id.</param>
        /// <param name="acquisitionFileId">AcquisitionFile Id.</param>
        /// <returns>New Form8.</returns>
        public static PimsExpropriationPayment CreateExpropriationPayment(long id = 1, long acquisitionFileId = 1)
        {
            return new PimsExpropriationPayment()
            {
                Internal_Id = id,
                AcquisitionFileId = acquisitionFileId,
            };
        }
    }
}
