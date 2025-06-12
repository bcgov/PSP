namespace Pims.Dal.Entities.Models
{
    public class ManagementActivityFilter : PageFilter
    {
        public ManagementActivityFilter()
            : base()
        {
        }

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
        /// get/set - The Activity Type.
        /// </summary>
        public string ActivityTypeCode { get; set; }

        /// <summary>
        /// get/set - The Activity Status Code.
        /// </summary>
        public string ActivityStatusCode { get; set; }

        /// <summary>
        /// get/set - The MOTI project name or the project number, search for both simultaneously.
        /// </summary>
        public string ProjectNameOrNumber { get; set; }
    }
}
