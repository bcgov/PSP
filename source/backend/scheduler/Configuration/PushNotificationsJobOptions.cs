namespace Pims.Scheduler.Configuration
{
    /// <summary>
    /// PushEmailNotificationsJobOptions class, provides a way to store job configuration.
    /// </summary>
    public class PushNotificationsJobOptions
    {
        /// <summary>
        /// get/set - the number of Email User Notifications to pull in a single operation.
        /// </summary>
        public int? EmailNotificationsBatchSize { get; set; }

        /// <summary>
        /// get/set - the number of Email User Notifications MAX retries allowed.
        /// </summary>
        public int? EmailNotificationsMaxRetriesAllowed { get; set; }

        /// <summary>
        /// get/set - the number of PIMS User Notifications to pull in a single operation.
        /// </summary>
        public int? PimsNotificationsBatchSize { get; set; }

        /// <summary>
        /// get/set - the number of PIMS User Notifications MAX retries allowed.
        /// </summary>
        public int? PimsNotificationsMaxRetriesAllowed { get; set; }
    }
}
