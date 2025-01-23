using System.Threading.Tasks;
using Hangfire;
using Pims.Scheduler.Models;

namespace Pims.Scheduler.Services
{
    public interface IDocumentQueueService
    {
        [DisableConcurrentExecution(timeoutInSeconds: 10 * 30)]
        public Task<ScheduledTaskResponseModel> UploadQueuedDocuments();

        [DisableConcurrentExecution(timeoutInSeconds: 10 * 30)]
        public Task<ScheduledTaskResponseModel> RetryQueuedDocuments();

        [DisableConcurrentExecution(timeoutInSeconds: 10 * 30)]
        public Task<ScheduledTaskResponseModel> QueryProcessingDocuments();
    }
}
