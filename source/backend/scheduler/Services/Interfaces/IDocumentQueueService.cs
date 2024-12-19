using System.Threading.Tasks;

namespace Pims.Scheduler.Services
{
    public interface IDocumentQueueService
    {
        public Task UploadQueuedDocuments();
    }
}
