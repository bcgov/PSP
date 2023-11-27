using Pims.Api.Models.Mayan.Sync;

namespace Pims.Api.Services
{
    /// <summary>
    /// IDocumentSyncService interface, defines the functionality for document syncronization services.
    /// </summary>
    public interface IDocumentSyncService
    {
        DocumentSyncResponse SyncPimsDocumentTypes(SyncModel model);

        ExternalBatchResult SyncMayanMetadataTypes(SyncModel model);

        ExternalBatchResult MigrateMayanMetadataTypes(SyncModel model);

        ExternalBatchResult SyncPimsToMayan(SyncModel model);
    }
}
