using System.Collections.Generic;
using Pims.Scheduler.Models.Base;

namespace Pims.Scheduler.Models
{
    public class ScheduledTaskResponseModel : BaseTaskResponseModel
    {
        public IEnumerable<DocumentQueueResponseModel> DocumentQueueResponses { get; set; }
    }
}
