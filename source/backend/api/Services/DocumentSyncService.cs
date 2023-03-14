using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Api.Models;
using Pims.Api.Models.Concepts.Document;
using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Document;
using Pims.Api.Models.Mayan.Metadata;
using Pims.Api.Models.Mayan.Sync;
using Pims.Api.Repositories.Mayan;
using Pims.Core.Http.Configuration;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Api.Services
{
    /// <summary>
    /// DocumentSyncService implementation provides document syncronization between different data sources.
    /// </summary>
    public class DocumentSyncService : BaseService, IDocumentSyncService
    {
        private readonly IEdmsDocumentRepository mayanDocumentRepository;
        private readonly IEdmsMetadataRepository mayanMetadataRepository;
        private readonly IDocumentTypeRepository documentTypeRepository;
        private readonly IOptionsMonitor<AuthClientOptions> keycloakOptions;

        public DocumentSyncService(
            ClaimsPrincipal user,
            ILogger<DocumentSyncService> logger,
            IEdmsDocumentRepository edmsDocumentRepository,
            IEdmsMetadataRepository emdMetadataRepository,
            IDocumentTypeRepository documentTypeRepository,
            IOptionsMonitor<AuthClientOptions> options)
            : base(user, logger)
        {
            this.mayanDocumentRepository = edmsDocumentRepository;
            this.mayanMetadataRepository = emdMetadataRepository;
            this.documentTypeRepository = documentTypeRepository;
            this.keycloakOptions = options;
        }

        public ExternalBatchResult SyncMayanMetadataTypes(SyncModel model)
        {
            this.Logger.LogInformation("Synchronizing Mayan metadata types");
            this.User.ThrowIfNotAuthorizedOrServiceAccount(Permissions.DocumentAdmin, this.keycloakOptions);

            ExternalBatchResult batchResult = new ExternalBatchResult();

            Task<ExternalResult<QueryResult<MetadataType>>> retrieveTask = mayanMetadataRepository.TryGetMetadataTypesAsync(pageSize: 5000);
            retrieveTask.Wait();
            var metadata = retrieveTask.Result;

            // Add the metadata types not in mayan
            IList<Task<ExternalResult<MetadataType>>> createTasks = new List<Task<ExternalResult<MetadataType>>>();
            foreach (var metadataTypeLabel in model.MetadataTypes)
            {
                if (metadata.Payload.Results.FirstOrDefault(x => x.Label == metadataTypeLabel) == null)
                {
                    var newMetadataType = new MetadataType() { Label = metadataTypeLabel, Name = metadataTypeLabel.Replace(' ', '_') };
                    createTasks.Add(mayanMetadataRepository.TryCreateMetadataTypeAsync(newMetadataType));
                }
            }

            Task.WaitAll(createTasks.ToArray());
            foreach (var task in createTasks)
            {
                batchResult.CreatedMetadata.Add(task.Result);
            }

            if (model.RemoveLingeringMetadataTypes)
            {
                // Delete the metadata types that are not on the sync model
                IList<Task<ExternalResult<string>>> deleteTasks = new List<Task<ExternalResult<string>>>();
                foreach (var metadataType in metadata.Payload.Results)
                {
                    if (model.MetadataTypes.IndexOf(metadataType.Label) < 0)
                    {
                        deleteTasks.Add(mayanMetadataRepository.TryDeleteMetadataTypeAsync(metadataType.Id));
                    }
                }

                Task.WaitAll(deleteTasks.ToArray());
                foreach (var task in deleteTasks)
                {
                    batchResult.DeletedMetadata.Add(task.Result);
                }
            }

            return batchResult;
        }

        public ExternalBatchResult SyncMayanDocumentTypes(SyncModel model)
        {
            this.Logger.LogInformation("Synchronizing Mayan document types");
            this.User.ThrowIfNotAuthorizedOrServiceAccount(Permissions.DocumentAdmin, this.keycloakOptions);

            ExternalBatchResult batchResult = new ExternalBatchResult();

            Task<ExternalResult<QueryResult<DocumentType>>> documentTypeTask = mayanDocumentRepository.TryGetDocumentTypesAsync(pageSize: 5000);
            documentTypeTask.Wait();

            ExternalResult<QueryResult<DocumentType>> documentTypesRetrieved = documentTypeTask.Result;

            // Add the document types not in mayan
            IList<Task<ExternalResult<DocumentType>>> createTasks = new List<Task<ExternalResult<DocumentType>>>();
            foreach (var documentTypeModel in model.DocumentTypes)
            {
                if (documentTypesRetrieved.Payload.Results.FirstOrDefault(x => x.Label == documentTypeModel.Label) == null)
                {
                    createTasks.Add(mayanDocumentRepository.TryCreateDocumentTypeAsync(new DocumentType() { Label = documentTypeModel.Label }));
                }
            }
            Task.WhenAll(createTasks.ToArray());
            foreach (var task in createTasks)
            {
                batchResult.CreatedDocumentType.Add(task.Result);
            }

            if (model.RemoveLingeringDocumentTypes)
            {
                // Delete the document types that are not on the sync model
                IList<Task<ExternalResult<string>>> deleteTasks = new List<Task<ExternalResult<string>>>();
                foreach (var documentType in documentTypesRetrieved.Payload.Results)
                {
                    if (model.DocumentTypes.FirstOrDefault(x => x.Label == documentType.Label) == null)
                    {
                        deleteTasks.Add(mayanDocumentRepository.TryDeleteDocumentTypeAsync(documentType.Id));
                    }
                }

                Task.WaitAll(deleteTasks.ToArray());
                foreach (var task in deleteTasks)
                {
                    batchResult.DeletedDocumentType.Add(task.Result);
                }
            }

            SyncDocumentTypeMetadataTypes(model, ref batchResult);

            return batchResult;
        }

        public async Task<DocumentTypeSyncResponse> SyncBackendDocumentTypes(SyncModel model)
        {
            this.Logger.LogInformation("Synchronizing Pims DB and Mayan document types");
            this.User.ThrowIfNotAuthorizedOrServiceAccount(Permissions.DocumentAdmin, this.keycloakOptions);

            ExternalResult<QueryResult<DocumentType>> mayanResult = await mayanDocumentRepository.TryGetDocumentTypesAsync(pageSize: 5000);

            if (mayanResult.Status != ExternalResultStatus.Success && mayanResult.Payload?.Results?.Count == 0)
            {
                return new DocumentTypeSyncResponse();
            }

            IList<PimsDocumentTyp> pimsDocumentTypes = documentTypeRepository.GetAll();

            // Add the document types not in the backend
            IList<PimsDocumentTyp> createdDocumentTypes = new List<PimsDocumentTyp>();
            IList<PimsDocumentTyp> updatedDocumentTypes = new List<PimsDocumentTyp>();
            foreach (var mayanDocumentType in mayanResult.Payload.Results)
            {
                var matchingTypeFromModel = model.DocumentTypes.FirstOrDefault(x => x.Label.Equals(mayanDocumentType.Label, System.StringComparison.OrdinalIgnoreCase));
                if (pimsDocumentTypes.FirstOrDefault(x => x.MayanId == mayanDocumentType.Id) == null)
                {
                    var newPimsDocType = new PimsDocumentTyp() { MayanId = mayanDocumentType.Id, DocumentType = mayanDocumentType.Label, DisplayOrder = matchingTypeFromModel?.DisplayOrder };
                    createdDocumentTypes.Add(documentTypeRepository.Add(newPimsDocType));
                }
                else if (pimsDocumentTypes.FirstOrDefault(x => x.MayanId == mayanDocumentType.Id && (mayanDocumentType.Label != x.DocumentType || x.DisplayOrder != matchingTypeFromModel?.DisplayOrder)) != null)
                {
                    // if the Mayan id is the same but the label or display order has changed, update the document type in PIMS.
                    var updatedPimsDocType = new PimsDocumentTyp() { MayanId = mayanDocumentType.Id, DocumentType = mayanDocumentType.Label, DisplayOrder = matchingTypeFromModel?.DisplayOrder };
                    updatedDocumentTypes.Add(documentTypeRepository.Update(updatedPimsDocType));
                }
            }

            IList<PimsDocumentTyp> removedDocumentTypes = new List<PimsDocumentTyp>();
            if (model.RemoveLingeringDocumentTypes)
            {
                // Delete the document types that are not on the sync model
                foreach (var documentTypeToRemove in pimsDocumentTypes.Concat(createdDocumentTypes))
                {
                    if (mayanResult.Payload.Results.FirstOrDefault(x => x.Id == documentTypeToRemove.MayanId) == null)
                    {
                        // TODO: disable this document type: psp-5702
                    }
                }
            }

            // If there are new doctypes, commit the transaction
            if (createdDocumentTypes.Count > 0 || removedDocumentTypes.Count > 0)
            {
                documentTypeRepository.CommitTransaction();
            }

            return new DocumentTypeSyncResponse() { Added = createdDocumentTypes, Updated = updatedDocumentTypes };
        }

        private void SyncDocumentTypeMetadataTypes(SyncModel model, ref ExternalBatchResult batchResult)
        {
            Task<ExternalResult<QueryResult<DocumentType>>> documentTypeTask = mayanDocumentRepository.TryGetDocumentTypesAsync(pageSize: 5000);
            Task<ExternalResult<QueryResult<MetadataType>>> metadataTypesTask = mayanMetadataRepository.TryGetMetadataTypesAsync(pageSize: 5000);
            Task.WaitAll(documentTypeTask, metadataTypesTask);

            var retrievedMetadataTypes = metadataTypesTask.Result;

            // Check that the metadata types on the documents are the same as the ones in the specified in the model
            IList<Task<ExternalBatchResult>> linkTasks = new List<Task<ExternalBatchResult>>();
            foreach (var documentTypeModel in model.DocumentTypes)
            {
                linkTasks.Add(AsyncLinkDocumentMetadata(documentTypeModel, documentTypeTask.Result.Payload.Results, retrievedMetadataTypes.Payload.Results));
            }

            Task.WaitAll(linkTasks.ToArray());
            foreach (var task in linkTasks)
            {
                batchResult.DeletedDocumentTypeMetadataType.AddRange(task.Result.DeletedDocumentTypeMetadataType);
                batchResult.LinkedDocumentMetadataTypes.AddRange(task.Result.LinkedDocumentMetadataTypes);
            }
        }

        private async Task<ExternalBatchResult> AsyncLinkDocumentMetadata(DocumentTypeModel documentTypeModel, IList<DocumentType> retrievedDocumentTypes, IList<MetadataType> retrievedMetadataTypes)
        {
            ExternalBatchResult batchResult = new ExternalBatchResult();

            var documentType = retrievedDocumentTypes.FirstOrDefault(x => x.Label == documentTypeModel.Label);
            ExternalResult<QueryResult<DocumentTypeMetadataType>> documentTypeMetadataTypes = await mayanDocumentRepository.TryGetDocumentTypeMetadataTypesAsync(documentType.Id, pageSize: 5000);

            // Update the document type's metadata types
            IList<Task<ExternalResult<DocumentTypeMetadataType>>> documentTypeMetadataTypeTasks = new List<Task<ExternalResult<DocumentTypeMetadataType>>>();
            var retrievedLinks = documentTypeMetadataTypes.Payload.Results;
            foreach (var metadataTypeModel in documentTypeModel.MetadataTypes)
            {
                DocumentTypeMetadataType existingDocumentTypeMetadaType = retrievedLinks.FirstOrDefault(x => x.MetadataType.Label == metadataTypeModel.Label);
                if (existingDocumentTypeMetadaType == null)
                {
                    MetadataType metadataType = retrievedMetadataTypes.FirstOrDefault(x => x.Label == metadataTypeModel.Label);
                    if (metadataType != null)
                    {
                        documentTypeMetadataTypeTasks.Add(mayanDocumentRepository.TryCreateDocumentTypeMetadataTypeAsync(documentType.Id, metadataType.Id, metadataTypeModel.Required));
                    }
                    else
                    {
                        batchResult.LinkedDocumentMetadataTypes.Add(
                            new ExternalResult<DocumentTypeMetadataType>()
                            {
                                Message = $"Metadata with label [{metadataTypeModel.Label}] does not exist in Mayan",
                                Status = ExternalResultStatus.Error,
                            });
                    }
                }
                else
                {
                    if (existingDocumentTypeMetadaType.Required != metadataTypeModel.Required)
                    {
                        documentTypeMetadataTypeTasks.Add(mayanDocumentRepository.TryUpdateDocumentTypeMetadataTypeAsync(documentType.Id, existingDocumentTypeMetadaType.Id, metadataTypeModel.Required));
                    }
                }
            }
            Task.WaitAll(documentTypeMetadataTypeTasks.ToArray());
            foreach (var task in documentTypeMetadataTypeTasks)
            {
                batchResult.LinkedDocumentMetadataTypes.Add(task.Result);
            }

            // Get metadata types not on sync model
            IList<Task<ExternalResult<string>>> deleteTasks = new List<Task<ExternalResult<string>>>();
            foreach (var documentTypeMetadataType in documentTypeMetadataTypes.Payload.Results)
            {
                if (documentTypeModel.MetadataTypes.FirstOrDefault(x => x.Label == documentTypeMetadataType.MetadataType.Label) == null)
                {
                    deleteTasks.Add(mayanDocumentRepository.TryDeleteDocumentTypeMetadataTypeAsync(documentType.Id, documentTypeMetadataType.Id));
                }
            }

            Task.WaitAll(deleteTasks.ToArray());
            foreach (var task in deleteTasks)
            {
                batchResult.DeletedDocumentTypeMetadataType.Add(task.Result);
            }

            return batchResult;
        }
    }
}
