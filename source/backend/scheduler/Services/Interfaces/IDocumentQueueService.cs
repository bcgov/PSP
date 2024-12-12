using System.Threading.Tasks;
using Pims.Scheduler.Models;

namespace Pims.Scheduler.Services
{
    public interface IDocumentQueueService
    {
        public Task<ScheduledTaskResponseModel> UploadQueuedDocuments();

        public Task<ScheduledTaskResponseModel> RetryQueuedDocuments();

        public Task<ScheduledTaskResponseModel> QueryProcessingDocuments();
    }
}
