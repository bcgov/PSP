using System.Collections.Generic;
using System.Threading.Tasks;
using Pims.Api.Models.Mayan.Sync;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    /// <summary>
    /// IDocumentSyncService interface, defines the functionality for document syncronization services.
    /// </summary>
    public interface IDocumentSyncService
    {
        ExternalBatchResult SyncMayanDocumentTypes(SyncModel model);

        ExternalBatchResult SyncMayanMetadataTypes(SyncModel model);

        Task<IList<PimsDocumentTyp>> SyncBackendDocumentTypes();
    }
}
