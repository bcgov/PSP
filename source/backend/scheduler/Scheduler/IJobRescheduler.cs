namespace Pims.Scheduler.Rescheduler
{
    public interface IJobRescheduler
    {
        /// <summary>
        /// Load from configuration and override the schdule set via code.
        /// If configuration is null, it will get from the singleton instance of JobConfiguration
        /// </summary>
        void LoadSchedules(JobScheduleOptions options);
    }
}
