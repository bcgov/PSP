namespace Pims.Dal.Entities.Models
{
    public class ManagementFilter : PageFilter
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
        /// get/set - The management file name or the file number or the legacy reference number, search for all simultaneously.
        /// </summary>
        public string FileNameOrNumberOrReference { get; set; }

        /// <summary>
        /// get/set - The status of the management file.
        /// </summary>
        public string ManagementFileStatusCode { get; set; }

        /// <summary>
        /// get/set - The status of the management file.
        /// </summary>
        public string ManagementFilePurposeCode { get; set; }

        /// <summary>
        /// get/set - The MOTI project name or the project number, search for both simultaneously.
        /// </summary>
        public string ProjectNameOrNumber { get; set; }

        /// <summary>
        /// get/set - The MOTI person id to search by for management team members.
        /// </summary>
        public long? TeamMemberPersonId { get; set; }

        /// <summary>
        /// get/set - The MOTI Organization id to search by for management team members.
        /// </summary>
        public long? TeamMemberOrganizationId { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a AcquisitionFilter class.
        /// </summary>
        public ManagementFilter()
            : base()
        {
        }

        #endregion
    }
}
