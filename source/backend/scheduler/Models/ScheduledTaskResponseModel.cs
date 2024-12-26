using System.Collections.Generic;

namespace Pims.Scheduler.Models
{
    public class ScheduledTaskResponseModel
    {
        public TaskResponseStatusTypes Status { get; set; }

        public string Message { get; set; }

        public IEnumerable<DocumentQueueResponseModel> DocumentQueueResponses { get; set; }
    }
}
