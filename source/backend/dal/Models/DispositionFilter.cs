namespace Pims.Dal.Entities.Models
{
    public class DispositionFilter : PageFilter
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
        /// get/set - The disposition file name or the file number or the legacy reference number, search for all simultaneously.
        /// </summary>
        public string FileNameOrNumberOrReference { get; set; }

        /// <summary>
        /// get/set - The status of the disposition file.
        /// </summary>
        public string DispositionFileStatusCode { get; set; }

        /// <summary>
        /// get/set - The disposition status.
        /// </summary>
        public string DispositionStatusCode { get; set; }

        /// <summary>
        /// get/set - The type of the disposition.
        /// </summary>
        public string DispositionTypeCode { get; set; }

        /// <summary>
        /// get/set - The MOTI person id to search by for disposition team members.
        /// </summary>
        public long? TeamMemberPersonId { get; set; }

        /// <summary>
        /// get/set - The MOTI Organization id to search by for disposition team members.
        /// </summary>
        public long? TeamMemberOrganizationId { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a AcquisitionFilter class.
        /// </summary>
        public DispositionFilter()
            : base()
        {
        }

        #endregion
    }
}
