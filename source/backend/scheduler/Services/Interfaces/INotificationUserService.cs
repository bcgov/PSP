using System.Threading.Tasks;
using Hangfire;
using Pims.Scheduler.Models.Base;

namespace Pims.Scheduler.Services.Interfaces
{
    public interface INotificationUserService
    {
        [DisableConcurrentExecution(timeoutInSeconds: 10 * 30)]
        public Task<BaseTaskResponseModel> PushEmailUserNotifications();

        [DisableConcurrentExecution(timeoutInSeconds: 10 * 30)]
        public Task<BaseTaskResponseModel> PushPimsUserNotifications();
    }
}
