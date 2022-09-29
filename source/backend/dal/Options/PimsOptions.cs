using Microsoft.EntityFrameworkCore;

namespace Pims.Dal
{
    /// <summary>
    /// PimsOptions class, provides a way to configure the data access layer.
    /// </summary>
    public class PimsOptions : DbContextOptions<PimsContext>
    {
        #region Properties

        /// <summary>
        /// get/set - Service account configuration details.
        /// </summary>
        public ServiceAccountOptions ServiceAccount { get; set; } = new ServiceAccountOptions();

        /// <summary>
        /// get/set - Global project configuration settings.
        /// </summary>
        public ProjectOptions Project { get; set; } = new ProjectOptions();

        /// <summary>
        /// get/set - Environmental configuration settings.
        /// These are included in the notifications model.
        /// </summary>
        public EnvironmentOptions Environment { get; set; } = new EnvironmentOptions();

        /// <summary>
        /// get/set - Access request configuration settings.
        /// </summary>
        public AccessRequestOptions AccessRequest { get; set; } = new AccessRequestOptions();

        /// <summary>
        /// get/set - Notification configuration settings.
        /// </summary>
        public NotificationOptions Notifications { get; set; } = new NotificationOptions();

        /// <summary>
        /// get/set - The tenant code to identify the environmental configuration settings.
        /// </summary>
        public string Tenant { get; set; }

        /// <summary>
        /// get/set - The email address for the help desk.
        /// </summary>
        public string HelpDeskEmail { get; set; }
        #endregion
    }
}
