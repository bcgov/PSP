namespace Pims.Scheduler.Rescheduler
{
    /// <summary>
    /// JobSchedule.
    /// </summary>
    public class JobScheduleOption
    {
        /// <summary>
        /// Recurring JobId.
        /// </summary>
        public string JobId { get; set; } = default!;

        /// <summary>
        /// Cron Expression.
        /// </summary>
        public string Cron { get; set; } = default!;

        /// <summary>
        /// If enabled == false, the job will be removed.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// TimezoneId.
        /// </summary>
        public string TimeZoneId { get; set; }
    }
}
