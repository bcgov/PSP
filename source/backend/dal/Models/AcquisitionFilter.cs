namespace Pims.Dal.Entities.Models
{
    public class AcquisitionFilter : PageFilter
    {
        #region Properties

        /// <summary>
        /// get/set - The pid identifier to search by.
        /// </summary>
        public string Pid { get; set; }

        /// <summary>
        /// get/set - The pin identifier to search by.
        /// </summary>
        public string Pin { get; set; }

        /// <summary>
        /// get/set - The address to search by.
        /// </summary>
        public string Address { get; set; }

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

        /// <summary>
        /// get/set - The MOTI person id to search by for acquisition team members.
        /// </summary>
        public string AcquisitionTeamMemberPersonId { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a AcquisitionFilter class.
        /// </summary>
        public AcquisitionFilter()
        {
        }

        #endregion
    }
}
