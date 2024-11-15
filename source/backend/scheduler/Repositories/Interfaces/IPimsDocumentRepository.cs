using System.Collections.Generic;
using System.Threading.Tasks;
using Pims.Api.Models.Concepts.Document;
using Pims.Api.Models.Requests.Http;
using Pims.Dal.Entities.Models;

namespace Pims.Scheduler.Repositories.Pims
{
    /// <summary>
    /// IPimsDocumentQueueRepository interface, defines the functionality for a pims repository.
    /// </summary>
    public interface IPimsDocumentQueueRepository
    {
        Task<ExternalResponse<List<DocumentQueueModel>>> SearchQueuedDocumentsAsync(DocumentQueueFilter filter);
    }
}
