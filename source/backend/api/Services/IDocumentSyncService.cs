using System.Threading.Tasks;
using Pims.Api.Models.Concepts.Document;
using Pims.Api.Models.Mayan.Sync;

namespace Pims.Api.Services
{
    /// <summary>
    /// IDocumentSyncService interface, defines the functionality for document syncronization services.
    /// </summary>
    public interface IDocumentSyncService
    {
        ExternalBatchResult SyncMayanDocumentTypes(SyncModel model);

        ExternalBatchResult SyncMayanMetadataTypes(SyncModel model);

        Task<DocumentTypeSyncResponse> SyncBackendDocumentTypes(SyncModel model);
    }
}
