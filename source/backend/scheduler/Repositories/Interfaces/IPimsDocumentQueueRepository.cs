using System.Collections.Generic;
using System.Threading.Tasks;
using Pims.Api.Models.Concepts.Document;
using Pims.Api.Models.Requests.Http;
using Pims.Dal.Entities.Models;

namespace Pims.Scheduler.Repositories
{
    /// <summary>
    /// IPimsDocumentQueueRepository interface, defines the functionality for a repository that interacts with the pims document queue api.
    /// </summary>
    public interface IPimsDocumentQueueRepository
    {
        Task<ExternalResponse<DocumentQueueModel>> GetById(long documentQueueId);

        Task<ExternalResponse<DocumentQueueModel>> UploadQueuedDocument(DocumentQueueModel document);

        Task<ExternalResponse<DocumentQueueModel>> PollQueuedDocument(DocumentQueueModel document);

        Task<ExternalResponse<DocumentQueueModel>> UpdateQueuedDocument(long documentQueueId, DocumentQueueModel document);

        Task<ExternalResponse<List<DocumentQueueModel>>> SearchQueuedDocumentsAsync(DocumentQueueFilter filter);
    }
}
