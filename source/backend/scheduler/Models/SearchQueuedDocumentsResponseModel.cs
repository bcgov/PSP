using System.Collections.Generic;
using Pims.Api.Models.Concepts.Document;
using Pims.Api.Models.Requests.Http;

namespace Pims.Scheduler.Models
{
    public class SearchQueuedDocumentsResponseModel
    {
        public ExternalResponse<List<DocumentQueueModel>> SearchResults { get; set; }

        public ScheduledTaskResponseModel ScheduledTaskResponseModel { get; set; }
    }
}
