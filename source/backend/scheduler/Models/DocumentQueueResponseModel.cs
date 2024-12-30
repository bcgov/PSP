using Pims.Api.Models.CodeTypes;

namespace Pims.Scheduler.Models
{
    public class DocumentQueueResponseModel
    {
        public DocumentQueueStatusTypes DocumentQueueStatus { get; set; }

        public string Message { get; set; }
    }
}
