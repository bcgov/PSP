using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Api.Services
{
    /// <summary>
    /// IDocumentService interface, defines the functionality for document queue services.
    /// </summary>
    public interface IDocumentQueueService
    {
        public IEnumerable<PimsDocumentQueue> SearchDocumentQueue(DocumentQueueFilter filter);
    }
}
