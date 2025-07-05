using System.Collections.Generic;
using System.Threading.Tasks;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Api.Services
{
    /// <summary>
    /// IDocumentService interface, defines the functionality for document queue services.
    /// </summary>
    public interface IDocumentQueueService
    {
        public PimsDocumentQueue GetById(long documentQueueId);

        public IEnumerable<PimsDocumentQueue> SearchDocumentQueue(DocumentQueueFilter filter);

        public Task<PimsDocumentQueue> Update(PimsDocumentQueue documentQueue);

        public Task<PimsDocumentQueue> PollForDocument(PimsDocumentQueue documentQueue);

        public Task<PimsDocumentQueue> Upload(PimsDocumentQueue documentQueue);
    }
}
