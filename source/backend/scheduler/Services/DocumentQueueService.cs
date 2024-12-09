using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.CodeTypes;
using Pims.Core.Api.Services;
using Pims.Scheduler.Repositories.Pims;

namespace Pims.Scheduler.Services
{
    public class DocumentQueueService : BaseService, IDocumentQueueService
    {
        private readonly ILogger _logger;
        private readonly IPimsDocumentQueueRepository _pimsDocumentQueueRepository;

        public DocumentQueueService(
            ILogger<DocumentQueueService> logger,
            IPimsDocumentQueueRepository pimsDocumentQueueRepository)
            : base(null, logger)
        {
            _logger = logger;
            _pimsDocumentQueueRepository = pimsDocumentQueueRepository;
        }

        public async Task UploadQueuedDocuments()
        {
            var queuedDocuments = await _pimsDocumentQueueRepository.SearchQueuedDocumentsAsync(new Dal.Entities.Models.DocumentQueueFilter() { Quantity = 50, DocumentQueueStatusTypeCode = DocumentQueueStatusTypes.PENDING.ToString() });
            _logger.LogInformation("retrieved {queuedDocuments} documents", queuedDocuments?.Payload?.Count);
        }
    }
}
