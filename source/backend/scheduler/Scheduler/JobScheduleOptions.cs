using System.Collections.Generic;

namespace Pims.Scheduler.Rescheduler
{
    public class JobScheduleOptions
    {
        /// <summary>
        /// Schedules. This will override the runtime registration defined in Jobs.Registry module.
        /// </summary>
        public List<JobScheduleOption> Schedules { get; set; } = new List<JobScheduleOption>();
    }
}
