using System.ComponentModel.DataAnnotations;

namespace Pims.Scheduler.Http.Configuration
{
    /// <summary>
    /// PimsOptions class, provides a way to store connection information for the PIMS application.
    /// </summary>
    public class PimsOptions
    {
        #region Properties

        /// <summary>
        /// get/set - the internal Uri of the pims server.
        /// </summary>
        [Required(ErrorMessage = "Configuration 'Uri' is required.")]
        public string Uri { get; set; }

        public string Environment { get; set; }
        #endregion
    }
}
