namespace Pims.Dal.Entities.Models
{
    public class AcquisitionFilter : PageFilter
    {
        #region Properties

        /// <summary>
        /// get/set - The status of the acquisition file,.
        /// </summary>
        public string AcquisitionFileStatusTypeCode { get; set; }

        /// <summary>
        /// get/set - The acquisition file name or the file number, search for both simultaneously.
        /// </summary>
        public string AcquisitionFileNameOrNumber { get; set; }

        /// <summary>
        /// get/set - The MOTI project name or the project number, search for both simultaneously.
        /// </summary>
        public string ProjectNameOrNumber { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a AcquisitionFilter class. test
        /// </summary>
        public AcquisitionFilter()
        {
        }

        #endregion
    }
}
