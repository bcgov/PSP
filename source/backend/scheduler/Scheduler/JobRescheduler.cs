using System;
using System.Linq;
using CsvHelper.Configuration;
using Hangfire;
using Hangfire.Storage;

namespace Pims.Scheduler.Rescheduler
{
    public class JobRescheduler : IJobRescheduler
    {
        private readonly JobStorage _jobStorage;
        private readonly IRecurringJobManager _recurringJobManager;

        public JobRescheduler(JobStorage jobStorage, IRecurringJobManager recurringJobManager)
        {
            _jobStorage = jobStorage;
            _recurringJobManager = recurringJobManager;
        }

        public void LoadSchedules(JobScheduleOptions options)
        {
            var storageConnection = _jobStorage.GetConnection();

            foreach (var scheduling in options.Schedules)
            {
                var recurringJob = storageConnection.GetRecurringJobs().FirstOrDefault(j => j.Id == scheduling.JobId);
                if (recurringJob == null)
                {
                    continue;
                }

                if (!scheduling.IsEnabled)
                {
                    _recurringJobManager.RemoveIfExists(scheduling.JobId);
                    continue;
                }

                var timezoneId = scheduling.TimeZoneId ?? recurringJob.TimeZoneId ?? TimeZoneInfo.Local.Id;
                var timezone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
                if (timezone == null)
                {
                    throw new ConfigurationException($"Unable to find TimeZoneInfo : {timezoneId}");
                }

                _recurringJobManager.AddOrUpdate(
                    recurringJob.Id,
                    recurringJob.Job,
                    scheduling.Cron ?? recurringJob.Cron,
                    new RecurringJobOptions() { TimeZone = timezone });
            }
        }
    }
}
